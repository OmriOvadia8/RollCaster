using UnityEngine;

namespace SD_Core
{
    public class SDMonoManager
    {
        private SDMonoManagerObject monoObject;

        public SDMonoManager()
        {
            var temp = new GameObject("MonoManager");
            monoObject = temp.AddComponent<SDMonoManagerObject>();
            Object.DontDestroyOnLoad(monoObject);
        }
    }
}