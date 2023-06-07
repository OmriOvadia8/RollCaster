using System.Collections.Generic;
using UnityEngine;

namespace SM_Core
{
    public class SMPoolManager
    {
        private Dictionary<PoolNames, SMPool> Pools = new();

        private Transform rootPools;

        public SMPoolManager()
        {
            rootPools = new GameObject("PoolsHolder").transform;
            Object.DontDestroyOnLoad(rootPools);
        }

        public void InitPool(string resourceName, int amount, Transform parentTransform, int maxAmount = 100)
        {
            var original = Resources.Load<SMPoolable>(resourceName);
            InitPool(original, amount, parentTransform, maxAmount);
        }

        public void InitPool(SMPoolable original, int amount, Transform parentTransform, int maxAmount)
        {
            SMManager.Instance.FactoryManager.MultiCreateAsync(original, Vector3.zero, amount,
                (List<SMPoolable> list) =>
                {
                    foreach (var poolable in list)
                    {
                        poolable.name = original.name;
                        poolable.transform.SetParent(parentTransform, false);
                        poolable.gameObject.SetActive(false);
                    }

                    var pool = new SMPool
                    {
                        AllPoolables = new Queue<SMPoolable>(list),
                        UsedPoolables = new Queue<SMPoolable>(),
                        AvailablePoolables = new Queue<SMPoolable>(list),
                        MaxPoolables = maxAmount
                    };

                    Pools.Add(original.poolName, pool);
                });
        }

        public SMPoolable GetPoolable(PoolNames poolName)
        {
            if (Pools.TryGetValue(poolName, out SMPool pool))
            {
                if (pool.AvailablePoolables.TryDequeue(out SMPoolable poolable))
                {
                    SMDebug.Log($"GetPoolable - {poolName}");

                    poolable.OnTakenFromPool();

                    pool.UsedPoolables.Enqueue(poolable);
                    poolable.gameObject.SetActive(true);
                    return poolable;
                }

                //Create more
                SMDebug.Log($"pool - {poolName} no enough poolables, used poolables {pool.UsedPoolables.Count}");

                return null;
            }

            SMDebug.Log($"pool - {poolName} wasn't initialized");
            return null;
        }

        public void ReturnPoolable(SMPoolable poolable)
        {
            if (Pools.TryGetValue(poolable.poolName, out SMPool pool))
            {
                pool.AvailablePoolables.Enqueue(poolable);
                poolable.OnReturnedToPool();
                poolable.gameObject.SetActive(false);
            }
        }

        public void DestroyPool(PoolNames name)
        {
            if (Pools.TryGetValue(name, out SMPool pool))
            {
                foreach (var poolable in pool.AllPoolables)
                {
                    poolable.PreDestroy();
                    ReturnPoolable(poolable);
                }

                foreach (var poolable in pool.AllPoolables)
                {
                    Object.Destroy(poolable);
                }

                pool.AllPoolables.Clear();
                pool.AvailablePoolables.Clear();
                pool.UsedPoolables.Clear();

                Pools.Remove(name);
            }
        }
    }

    public class SMPool
    {
        public Queue<SMPoolable> AllPoolables = new();
        public Queue<SMPoolable> UsedPoolables = new();
        public Queue<SMPoolable> AvailablePoolables = new();

        public int MaxPoolables = 100;
    }

    public enum PoolNames
    {
        MoneyToast = 0,
        SpendMoneyToast = 1
    }
}