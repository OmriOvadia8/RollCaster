using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;


namespace SM_Core
{
    public class SMSaveManager
    {
        public void Save(ISMSaveData saveData)
        {
            var saveID = saveData.GetType().FullName;

            var saveJson = JsonConvert.SerializeObject(saveData);

            var path = $"{Application.persistentDataPath}/{saveID}.dbSave";

            File.WriteAllText(path, saveJson);
        }

        public void Load<T>(Action<T> onComplete) where T : ISMSaveData
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

        public bool HasData<T>() where T : ISMSaveData
        {
            var saveID = typeof(T).FullName;
            var path = $"{Application.persistentDataPath}/{saveID}.dbSave";
            return File.Exists(path);
        }
    }

    public interface ISMSaveData
    {
    }
}