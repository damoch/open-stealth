using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Assets.Scripts.Prefabs.WorlsObjects
{
    public class ItemsTools  {
        public static Dictionary<string, bool> GetItemsList()
        {
            Dictionary<string, bool> items = LoadItems();

            //TODO - that dictionary should be parsable
            //   items.Add("LEVEL1", false);
            //   items.Add("LEVEL2", false);
            //   items.Add("LEVEL3", false);

            return items;
        }

        private static Dictionary<string, bool> LoadItems()
        {
            var result = new Dictionary<string, bool>();
            var path = "Assets\\Resources\\Items.xml";
            var reader = new XmlTextReader(path);
            var serializer = new XmlSerializer(typeof(object));
            var deserialized = (IEnumerable)serializer.Deserialize(reader);

            //foreach (var thing in deserialized)
            //{
                
            //}

            return result;

        }

        public static void SetupItemsForScene(Dictionary<string, bool> items)
        {
            if (items == null || items.Count==0)
            {
                return;
            }
            var itemObjects = GameObject.FindGameObjectsWithTag("Item");

            for(int i = 0; i<itemObjects.Length; i++)
            {
                var go = itemObjects[i];
                var ic = go.GetComponent<Item>();
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
}
