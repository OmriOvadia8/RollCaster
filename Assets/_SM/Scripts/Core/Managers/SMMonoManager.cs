using UnityEngine;

namespace SM_Core
{
    public class SMMonoManager
    {
        private SMMonoManagerObject monoObject;

        public SMMonoManager()
        {
            var temp = new GameObject("MonoManager");
            monoObject = temp.AddComponent<SMMonoManagerObject>();
            Object.DontDestroyOnLoad(monoObject);
        }
    }
}