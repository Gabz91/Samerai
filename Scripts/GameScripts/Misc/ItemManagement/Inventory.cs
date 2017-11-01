using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts
{
    [System.Serializable]
    public class Inventory
    {
        static Inventory instance;
        public static Inventory Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Inventory");
                    instance = new Inventory();
                }
                instance.Initialize();
                return instance;
            }
        }

        public const int slotsAmount = 12;
        public Item[] items;

        public float itemOffset = 10;
        public Vector2 slotSize;

        void Initialize()
        {
            items = new Item[slotsAmount];
            slotSize = ResourceManager.Instance.inventoryPanel.GetComponent<GridLayoutGroup>().cellSize;
        }

        public void ToggleInventory()
        {
            ResourceManager.Instance.inventoryPanel.SetActive(!ResourceManager.Instance.inventoryPanel.activeSelf);
			if (Time.timeScale == 0) {
				Time.timeScale = 1;
			} else {
				Time.timeScale = 0;
			}
		}
    }

    [System.Serializable]
    public class Equipment
    {
        static Equipment instance;
        public static Equipment Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Equipment");
                    instance = new Equipment();
                }
                instance.Initialize();
                return instance;
            }
        }

        public const int slotsAmount = 3;
        public Item[] items;

        public float itemOffset = 10;
        public Vector2 slotSize;

        void Initialize()
        {
            items = new Item[slotsAmount];
            slotSize = ResourceManager.Instance.equipmentPanel.GetComponent<GridLayoutGroup>().cellSize;
        }
    }
}