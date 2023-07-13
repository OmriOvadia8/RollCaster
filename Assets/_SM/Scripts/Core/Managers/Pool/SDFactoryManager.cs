using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SD_Core
{
    /// <summary>
    /// Manages object instantiation in the game.
    /// </summary>
    public class SDFactoryManager
    {
        /// <summary>
        /// Asynchronously creates an instance of a specified type and invokes a callback when the object is created.
        /// </summary>
        /// <param name="name">Name of the resource.</param>
        /// <param name="pos">Position for the new object.</param>
        /// <param name="onCreated">Action to perform when the object is created.</param>
        /// <typeparam name="T">Type of the object.</typeparam>
        public void CreateAsync<T>(string name, Vector3 pos, Action<T> onCreated) where T : Object
        {
            var original = Resources.Load<T>(name);
            CreateAsync(original, pos, onCreated);
        }

        /// <summary>
        /// Asynchronously creates a clone of the provided object and invokes a callback when the object is created.
        /// </summary>
        /// <param name="origin">Original object to clone.</param>
        /// <param name="pos">Position for the new object.</param>
        /// <param name="onCreated">Action to perform when the object is created.</param>
        /// <typeparam name="T">Type of the object.</typeparam>
        public void CreateAsync<T>(T origin, Vector3 pos, Action<T> onCreated) where T : Object
        {
            var clone = Object.Instantiate(origin, pos, Quaternion.identity);
            onCreated?.Invoke(clone);
        }

        /// <summary>
        /// Asynchronously creates multiple clones of the provided object and invokes a callback when all objects are created.
        /// </summary>
        /// <param name="origin">Original object to clone.</param>
        /// <param name="pos">Position for the new objects.</param>
        /// <param name="amount">Number of objects to create.</param>
        /// <param name="onCreated">Action to perform when all objects are created.</param>
        /// <typeparam name="T">Type of the objects.</typeparam>
        public void MultiCreateAsync<T>(T origin, Vector3 pos, int amount, Action<List<T>> onCreated) where T : Object
        {
            List<T> createdObjects = new List<T>();

            for (var i = 0; i < amount; i++)
            {
                CreateAsync(origin, pos, OnCreated);
            }

            void OnCreated(T createdObject)
            {
                createdObjects.Add(createdObject);

                if (createdObjects.Count == amount)
                {
                    onCreated?.Invoke(createdObjects);
                }
            }
        }
    }
}
