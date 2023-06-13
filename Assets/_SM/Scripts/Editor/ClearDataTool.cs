using System.IO;
using UnityEditor;
using UnityEngine;

namespace DB_Core
{
    public class ClearDataTool
    {
        [MenuItem("SD/ClearData")]

        public static void ClearAllDataTool()
        {
            var path = Application.persistentDataPath;
            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                if (file.Contains("SD"))
                {
                    File.Delete(file);
                }
            }

            PlayerPrefs.DeleteAll();
        }
    }
}