using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MiniJSON;

public class SaveController : MonoBehaviour {

    static string USER = "USER";
    static string ITEMS = "ITEMS";
    static string CONTINUE = "CONTINUE";
    static string OPTIONS = "OPTIONS";
    static string NONE = "NONE";

    public static bool savedGameExists(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static string getUserName()
    {
        return savedGameExists(USER) ? PlayerPrefs.GetString(USER) : NONE;
    }

    public static void setUserName(string name)
    {
        PlayerPrefs.SetString(USER, name);
    }

    public static void saveGame(string key, SaveDataDTO save)
    {
        string saveData = JsonUtility.ToJson(save);
        string itemData = Json.Serialize(save.ItemsAquired);
        PlayerPrefs.SetString(key, saveData);
        PlayerPrefs.SetString(key + ITEMS, itemData);
    }


    public static SaveDataDTO getSavedGame(string key)
    {
        string saveData = PlayerPrefs.GetString(key);
        string itemData = PlayerPrefs.GetString(key + ITEMS);
        SaveDataDTO save = JsonUtility.FromJson<SaveDataDTO>(saveData);
        var items = Json.Deserialize(itemData);
        Dictionary<string, bool> itemsDict = toDictionary(items);
        save.ItemsAquired = itemsDict;
        return save;
    }

    private static Dictionary<string, bool> toDictionary(object items)
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

    internal static void setContinueFlag(string saveName)
    {
        PlayerPrefs.SetString(CONTINUE, saveName);
    }
    public static string getContinueFlag()
    {
        if (!PlayerPrefs.HasKey(CONTINUE))
        {
            return NONE;
        }
        return PlayerPrefs.GetString(CONTINUE);
    }
}
