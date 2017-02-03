using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MiniJSON;

public class SaveController : MonoBehaviour {

    static string _user = "USER";
    static string _items = "ITEMS";
    static string _continue = "CONTINUE";
    static string _options = "OPTIONS";
    static string _none = "NONE";

    public static bool SavedGameExists(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static string GetUserName()
    {
        return SavedGameExists(_user) ? PlayerPrefs.GetString(_user) : _none;
    }

    public static void SetUserName(string name)
    {
        PlayerPrefs.SetString(_user, name);
    }

    public static void SaveGame(string key, SaveDataDto save)
    {
        string saveData = JsonUtility.ToJson(save);
        string itemData = Json.Serialize(save.ItemsAquired);
        PlayerPrefs.SetString(key, saveData);
        PlayerPrefs.SetString(key + _items, itemData);
    }


    public static SaveDataDto GetSavedGame(string key)
    {
        string saveData = PlayerPrefs.GetString(key);
        string itemData = PlayerPrefs.GetString(key + _items);
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
        PlayerPrefs.SetString(_continue, saveName);
    }
    public static string GetContinueFlag()
    {
        if (!PlayerPrefs.HasKey(_continue))
        {
            return _none;
        }
        return PlayerPrefs.GetString(_continue);
    }
}
