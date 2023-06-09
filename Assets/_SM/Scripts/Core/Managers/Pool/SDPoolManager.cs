using System.Collections.Generic;
using UnityEngine;

namespace SD_Core
{
    public class SDPoolManager
    {
        private Dictionary<PoolNames, SDPool> Pools = new();

        private Transform rootPools;

        public SDPoolManager()
        {
            rootPools = new GameObject("PoolsHolder").transform;
            Object.DontDestroyOnLoad(rootPools);
        }

        public void InitPool(string resourceName, int amount, Transform parentTransform, int maxAmount = 100)
        {
            var original = Resources.Load<SDPoolable>(resourceName);
            InitPool(original, amount, parentTransform, maxAmount);
        }

        public void InitPool(SDPoolable original, int amount, Transform parentTransform, int maxAmount)
        {
            SDManager.Instance.FactoryManager.MultiCreateAsync(original, Vector3.zero, amount,
                (List<SDPoolable> list) =>
                {
                    foreach (var poolable in list)
                    {
                        poolable.name = original.name;
                        poolable.transform.SetParent(parentTransform, false);
                        poolable.gameObject.SetActive(false);
                    }

                    var pool = new SDPool
                    {
                        AllPoolables = new Queue<SDPoolable>(list),
                        UsedPoolables = new Queue<SDPoolable>(),
                        AvailablePoolables = new Queue<SDPoolable>(list),
                        MaxPoolables = maxAmount
                    };

                    Pools.Add(original.poolName, pool);
                });
        }

        public SDPoolable GetPoolable(PoolNames poolName)
        {
            if (Pools.TryGetValue(poolName, out SDPool pool))
            {
                if (pool.AvailablePoolables.TryDequeue(out SDPoolable poolable))
                {
                    SDDebug.Log($"GetPoolable - {poolName}");

                    poolable.OnTakenFromPool();

                    pool.UsedPoolables.Enqueue(poolable);
                    poolable.gameObject.SetActive(true);
                    return poolable;
                }

                //Create more
                SDDebug.Log($"pool - {poolName} no enough poolables, used poolables {pool.UsedPoolables.Count}");

                return null;
            }

            SDDebug.Log($"pool - {poolName} wasn't initialized");
            return null;
        }

        public void ReturnPoolable(SDPoolable poolable)
        {
            if (Pools.TryGetValue(poolable.poolName, out SDPool pool))
            {
                pool.AvailablePoolables.Enqueue(poolable);
                poolable.OnReturnedToPool();
                poolable.gameObject.SetActive(false);
            }
        }

        public void DestroyPool(PoolNames name)
        {
            if (Pools.TryGetValue(name, out SDPool pool))
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

    public class SDPool
    {
        public Queue<SDPoolable> AllPoolables = new();
        public Queue<SDPoolable> UsedPoolables = new();
        public Queue<SDPoolable> AvailablePoolables = new();

        public int MaxPoolables = 100;
    }

    public enum PoolNames
    {
        MoneyToast = 0,
        SpendMoneyToast = 1
    }
}