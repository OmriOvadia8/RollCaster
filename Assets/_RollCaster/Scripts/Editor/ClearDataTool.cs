using System.IO;
using UnityEditor;
using UnityEngine;

namespace DB_Core
{
    /// <summary>
    /// Provides a Unity editor tool to clear all game data.
    /// </summary>
    public class ClearDataTool
    {
        /// <summary>
        /// Clears all data saved in files that contain "SD" in their name located in the persistent data path of the application,
        /// as well as all data saved using PlayerPrefs.
        /// </summary>
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
