using System.Collections.Generic;
using Assets.Scripts.Data;
using MiniJSON;
using UnityEngine;

namespace Assets.Scripts.Modules
{
    public class SaveController : MonoBehaviour {

        public static string User = "USER";
        public static string Items = "ITEMS";
        public static string Continue = "CONTINUE";
        public static string Options = "OPTIONS";
        public static string None = "NONE";

        public static bool SavedGameExists(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public static string GetUserName()
        {
            return SavedGameExists(User) ? PlayerPrefs.GetString(User) : None;
        }

        public static void SetUserName(string name)
        {
            PlayerPrefs.SetString(User, name);
        }

        public static void SaveGame(string key, SaveDataDto save)
        {
            string saveData = JsonUtility.ToJson(save);
            string itemData = Json.Serialize(save.ItemsAquired);
            PlayerPrefs.SetString(key, saveData);
            PlayerPrefs.SetString(key + Items, itemData);
        }


        public static SaveDataDto GetSavedGame(string key)
        {
            string saveData = PlayerPrefs.GetString(key);
            string itemData = PlayerPrefs.GetString(key + Items);
            SaveDataDto save = JsonUtility.FromJson<SaveDataDto>(saveData);
            var items = Json.Deserialize(itemData);
            Dictionary<string, bool> itemsDict = ToDictionary(items);
            save.ItemsAquired = itemsDict;
            return save;
        }

        private static Dictionary<string, bool> ToDictionary(object items)
        {
            Dictionary<string, bool> result = new Dictionary<string, bool>();
            Dictionary<string, object> input = new Dictionary<string, object>();

            input = (Dictionary<string, object>)items;
            if (input != null)
            {
                foreach (KeyValuePair<string, object> entry in input)
                {
                    result.Add(entry.Key, (bool)entry.Value);
                }
            }
        

            return result;
        }

        internal static void SetContinueFlag(string saveName)
        {
            PlayerPrefs.SetString(Continue, saveName);
        }
        public static string GetContinueFlag()
        {
            if (!PlayerPrefs.HasKey(Continue))
            {
                return None;
            }
            return PlayerPrefs.GetString(Continue);
        }
    }
}
