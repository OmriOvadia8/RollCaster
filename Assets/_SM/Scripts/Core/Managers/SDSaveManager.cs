using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;


namespace SD_Core
{
    /// <summary>
    /// Manages the saving and loading of game data.
    /// </summary>
    public class SDSaveManager
    {
        /// <summary>
        /// Saves the given game data to a local file.
        /// </summary>
        /// <param name="saveData">The data to be saved.</param>
        public void Save(ISDSaveData saveData)
        {
            var saveID = saveData.GetType().FullName;
            var saveJson = JsonConvert.SerializeObject(saveData);
            var path = $"{Application.persistentDataPath}/{saveID}.sdSave";
            File.WriteAllText(path, saveJson);
        }

        /// <summary>
        /// Loads game data from a local file.
        /// </summary>
        /// <typeparam name="T">The type of the data to be loaded.</typeparam>
        /// <param name="onComplete">The action to be executed once the data is loaded.</param>
        public void Load<T>(Action<T> onComplete) where T : ISDSaveData
        {
            if (!HasData<T>())
            {
                onComplete.Invoke(default);
                return;
            }

            var saveID = typeof(T).FullName;
            var path = $"{Application.persistentDataPath}/{saveID}.sdSave";

            var saveJson = File.ReadAllText(path);
            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);
        }

        /// <summary>
        /// Checks if the data of the specified type exists in the local storage.
        /// </summary>
        /// <typeparam name="T">The type of the data to check.</typeparam>
        /// <returns>Returns true if the data exists, otherwise false.</returns>
        public bool HasData<T>() where T : ISDSaveData
        {
            var saveID = typeof(T).FullName;
            var path = $"{Application.persistentDataPath}/{saveID}.sdSave";
            return File.Exists(path);
        }
    }

    /// <summary>
    /// The interface that represents the data to be saved or loaded.
    /// </summary>
    public interface ISDSaveData
    {
    }
}
