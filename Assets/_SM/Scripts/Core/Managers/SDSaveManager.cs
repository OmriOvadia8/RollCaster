using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;


namespace SD_Core
{
    public class SDSaveManager
    {
        public void Save(ISDSaveData saveData)
        {
            var saveID = saveData.GetType().FullName;

            var saveJson = JsonConvert.SerializeObject(saveData);

            var path = $"{Application.persistentDataPath}/{saveID}.dbSave";

            File.WriteAllText(path, saveJson);
        }

        public void Load<T>(Action<T> onComplete) where T : ISDSaveData
        {
            if (!HasData<T>())
            {
                onComplete.Invoke(default);
                return;
            }

            var saveID = typeof(T).FullName;
            var path = $"{Application.persistentDataPath}/{saveID}.dbSave";

            var saveJson = File.ReadAllText(path);
            var saveData = JsonConvert.DeserializeObject<T>(saveJson);

            onComplete.Invoke(saveData);

        }

        public bool HasData<T>() where T : ISDSaveData
        {
            var saveID = typeof(T).FullName;
            var path = $"{Application.persistentDataPath}/{saveID}.dbSave";
            return File.Exists(path);
        }
    }

    public interface ISDSaveData
    {
    }
}