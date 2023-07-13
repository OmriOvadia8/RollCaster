using System.Collections.Generic;
using UnityEngine;

namespace SD_Core
{   
    /// <summary>
    /// Manages the creation and recycling of objects to improve game performance.
    /// </summary>
    public class SDPoolManager
    {
        private Dictionary<PoolNames, SDPool> Pools = new();

        private Transform rootPools;

        public SDPoolManager()
        {
            rootPools = new GameObject("PoolsHolder").transform;
            Object.DontDestroyOnLoad(rootPools);
        }

        /// <summary>
        /// Initializes a pool of objects based on resource name.
        /// </summary>
        public void InitPool(string resourceName, int amount, RectTransform parentTransform, int maxAmount = 100)
        {
            var original = Resources.Load<SDPoolable>(resourceName);
            InitPool(original, amount, parentTransform, maxAmount);
            SDDebug.Log(original + "LOADED");
        }

        /// <summary>
        /// Initializes a pool of objects based on a provided original object.
        /// </summary>
        public void InitPool(SDPoolable original, int amount, RectTransform parentTransform, int maxAmount)
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


        /// <summary>
        /// Gets an available object from the specified pool.
        /// </summary>
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

        /// <summary>
        /// Returns a poolable object to its pool.
        /// </summary>
        public void ReturnPoolable(SDPoolable poolable)
        {
            if (Pools.TryGetValue(poolable.poolName, out SDPool pool))
            {
                pool.AvailablePoolables.Enqueue(poolable);
                poolable.OnReturnedToPool();
                poolable.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Destroys a pool and all its objects.
        /// </summary>
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
        DamageAnim1 = 1,
        DamageAnim2 = 2,
        DamageAnim3 = 3,
        LevelUpToast = 6,
        XPToast = 7,
        EarnPointsToast = 8,
        SpendPointsToast = 9,
        FailAdToast = 10,
    }
}