using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsTools  {
    public static Dictionary<string, bool> GetItemsList()
    {
        Dictionary<string, bool> items = new Dictionary<string, bool>();

        //TODO - that dictionary should be parsable
        items.Add("LEVEL1", false);
        items.Add("LEVEL2", false);
        items.Add("LEVEL3", false);

        return items;
    }

    public static void SetupItemsForScene(Dictionary<string, bool> items)
    {
        var itemObjects = GameObject.FindGameObjectsWithTag("Item");

        for(int i = 0; i<itemObjects.Length; i++)
        {
            var go = itemObjects[i];
            var ic = go.GetComponent<ItemController>();
            string code = ic.KeyCode;

            if (items[code])
            {
                go.transform.position = new Vector3(go.transform.position.x, 
                                                    30f,
                                                    go.transform.position.z);
            }
            else
            {
                go.transform.position = new Vector3(go.transform.position.x,
                                                     0f,
                                                     go.transform.position.z);
            }
        } 
    }

    public static int CountScore(Dictionary<string, bool> items)
    {
        if(items == null)
        {
            return 0;
        }
        else
        {
            int result = 0;
            foreach (KeyValuePair<string, bool> entry in items)
            {
                result += entry.Value ? 1 : 0;
            }
            return result;
        }
    }
}
