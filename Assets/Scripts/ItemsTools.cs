using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemsTools  {
    public static Dictionary<string, bool> getItemsList()
    {
        Dictionary<string, bool> items = new Dictionary<string, bool>();

        //za każdym razem jak dodaje się nowy przedmiot w poziomie trzeba go umieścić w tym słowniku
        items.Add("LEVEL1", false);
        items.Add("LEVEL2", false);
        items.Add("LEVEL3", false);

        return items;
    }

    public static void setupItemsForScene(Dictionary<string, bool> items)
    {
        GameObject[] itemObjects = GameObject.FindGameObjectsWithTag("Item");

        for(int i = 0; i<itemObjects.Length; i++)
        {
            GameObject go = itemObjects[i];
            ItemController ic = go.GetComponent<ItemController>();
            string code = ic.keyCode;

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

    public static int countScore(Dictionary<string, bool> items)
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
