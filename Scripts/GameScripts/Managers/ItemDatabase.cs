using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using System.IO;

namespace GameScripts
{

    public class ItemDatabase
    {

        static ItemDatabase instance;
        public static ItemDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("ItemDatabase");
                    instance = new ItemDatabase();
                }
                return instance;
            }
        }

        private List<Item> itemBlueprints = new List<Item>();
        private List<Item> weaponBlueprints = new List<Item>();
        private JsonData itemData;

        // Use this for initialization
        private ItemDatabase()
        {
			string filePath = Application.streamingAssetsPath + "/JsonAssets/items.json";
			string jsonString;

			if (Application.platform == RuntimePlatform.Android)
			{
				WWW reader = new WWW(filePath);
				while (!reader.isDone) { }

				jsonString = reader.text;
			}
			else
			{
				jsonString = File.ReadAllText(filePath);
			}

			itemData = JsonMapper.ToObject(jsonString);
        }

        public Item GetItemByID(int id, ItemType type)
        {
            Item newItem;
            if (type == ItemType.WEAPON)
            {
                newItem = new Weapon(itemData["Weapons"][id]["name"].ToString(), itemData["Weapons"][id]["slug"].ToString(), itemData["Weapons"][id]["description"].ToString(), (int)itemData["Weapons"][id]["power"]);
                newItem.type = ItemType.WEAPON;
                newItem.id = id;
            }
            else
            {
                newItem = new Item(itemData["Items"][id]["name"].ToString(), itemData["Items"][id]["slug"].ToString(), itemData["Items"][id]["description"].ToString(), (int)itemData["Iems"][id]["value"]);
                newItem.type = ItemType.ITEM;
                newItem.id = id;
            }

            return newItem;
        }
    }
}