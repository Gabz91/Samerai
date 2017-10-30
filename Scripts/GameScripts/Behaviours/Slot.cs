using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace GameScripts
{
    //Handles the items dropped the inventory and the equipment panel
    //TODO fix issue when swapping two items from equip and inventory and vice versa
    public class Slot : MonoBehaviour, IDropHandler
    {
        public int ID;
        Inventory inventory;
        Equipment equipment;
        ResourceManager rsMng;

        void Start()
        {
            rsMng = Game.rsMng;
            inventory = rsMng.inventory;
            equipment = rsMng.equipment;            
        }

        //public void OnDrop(PointerEventData eventData)
        //{
        //    Transform itemDroppedTrans = eventData.pointerDrag.transform;
        //    ItemData itemDroppedData = itemDroppedTrans.GetComponent<ItemData>();
        //    if (this.transform.childCount != 0)
        //    {
        //        Transform itemSwappedTrans = this.transform.GetChild(0);
        //        ItemData itemSwappedData = itemSwappedTrans.GetComponent<ItemData>();
        //        int tempInvID = itemSwappedData.invSlotID;
        //        int tempEquipID = itemSwappedData.equipSlotID;
        //        itemSwappedData.invSlotID = itemDroppedData.invSlotID;
        //        itemSwappedData.equipSlotID = itemDroppedData.equipSlotID;
        //        if (itemDroppedData.invSlotID != -1)
        //        {
        //            itemSwappedTrans.SetParent(rsMng.invSlots[itemDroppedData.invSlotID].transform);
        //            itemSwappedTrans.position = rsMng.invSlots[itemDroppedData.invSlotID].transform.position;
        //        }
        //        else
        //        {
        //            itemSwappedTrans.SetParent(rsMng.equipSlots[itemDroppedData.equipSlotID].transform);
        //            itemSwappedTrans.position = rsMng.equipSlots[itemDroppedData.equipSlotID].transform.position;
        //        }
        //        itemDroppedData.invSlotID = tempInvID;
        //        itemDroppedData.equipSlotID = tempEquipID;
        //    }
        //    if (transform.parent.name.Equals("InventoryPanel"))
        //    {
        //        Debug.Log(this.transform.GetSiblingIndex());
        //        itemDroppedData.invSlotID = this.transform.GetSiblingIndex();
        //        itemDroppedData.equipSlotID = -1;
        //    }
        //    else
        //    {
        //        itemDroppedData.equipSlotID = this.transform.GetSiblingIndex();
        //        itemDroppedData.invSlotID = -1;
        //    }
        //    rsMng.hud.UpdateCharacterStats();
        //}

        public void OnDrop(PointerEventData eventData)
        {
            ItemData itemDroppedData = eventData.pointerDrag.GetComponent<ItemData>();
            //If the item is being dropped in the inventory
            if (transform.parent.name.Equals("InventoryPanel"))
            {
                if (rsMng.invSlots[ID].transform.childCount == 0)
                {
                    //If item was equipped
                    if (itemDroppedData.equipSlotID != -1)
                    {
                        rsMng.player.RemoveEquipStats(itemDroppedData.item);
                        equipment.items[itemDroppedData.equipSlotID] = null;
                    }
                    else
                        inventory.items[itemDroppedData.invSlotID] = null;

                    itemDroppedData.invSlotID = ID;
                    itemDroppedData.equipSlotID = -1;
                    inventory.items[ID] = itemDroppedData.item;
                }
                else
                {
                    //Put this in ItemData ?
                    Transform itemSwappedTrans = this.transform.GetChild(0);
                    ItemData itemSwappedData = itemSwappedTrans.GetComponent<ItemData>();
                    //Set the item to swap
                    itemSwappedData.invSlotID = itemDroppedData.invSlotID;
                    itemSwappedData.equipSlotID = itemDroppedData.equipSlotID;
                    if (itemDroppedData.equipSlotID != -1) //If the item was Equipped
                    {
                        //Swap items from equip to inventory
                        equipment.items[itemDroppedData.equipSlotID] = itemSwappedData.item;
                        inventory.items[ID] = itemDroppedData.item;
                        itemSwappedTrans.transform.SetParent(rsMng.equipSlots[itemDroppedData.equipSlotID].transform);
                        itemSwappedTrans.transform.position = rsMng.equipSlots[itemDroppedData.equipSlotID].transform.position;
                        rsMng.player.RemoveEquipStats(itemDroppedData.item);
                        rsMng.player.AddEquipStats(itemSwappedData.item);
                    }
                    else
                    {
                        //Swap items in the inventory
                        inventory.items[itemDroppedData.invSlotID] = itemSwappedData.item;
                        inventory.items[ID] = itemDroppedData.item;
                        itemSwappedTrans.transform.SetParent(rsMng.invSlots[itemDroppedData.invSlotID].transform);
                        itemSwappedTrans.transform.position = rsMng.invSlots[itemDroppedData.invSlotID].transform.position;
                    }
                    //Set the item to drop
                    itemDroppedData.invSlotID = ID;
                    itemDroppedData.equipSlotID = -1;
                }
            }
            //If the item is being equipped
            else
            {
                if (rsMng.equipSlots[ID].transform.childCount == 0)
                {
                    if (itemDroppedData.invSlotID != -1)
                    {
                        rsMng.player.AddEquipStats(itemDroppedData.item);
                        inventory.items[itemDroppedData.invSlotID] = null;
                    }
                    else
                        equipment.items[itemDroppedData.equipSlotID] = null;

                    itemDroppedData.equipSlotID = ID;
                    itemDroppedData.invSlotID = -1;
                    equipment.items[ID] = itemDroppedData.item;
                }
                else
                {
                    //Put this in ItemData ?
                    Transform itemSwappedTrans = this.transform.GetChild(0);
                    ItemData itemSwappedData = itemSwappedTrans.GetComponent<ItemData>();
                    //Set the item to swap
                    itemSwappedData.equipSlotID = itemDroppedData.equipSlotID;
                    itemSwappedData.invSlotID = itemDroppedData.invSlotID;
                    if (itemDroppedData.equipSlotID != -1) //If the item was Equipped
                    {
                        //Swap items in the equipment
                        equipment.items[itemDroppedData.equipSlotID] = itemSwappedData.item;
                        equipment.items[ID] = itemDroppedData.item;
                        itemSwappedTrans.transform.SetParent(rsMng.equipSlots[itemDroppedData.equipSlotID].transform);
                        itemSwappedTrans.transform.position = rsMng.equipSlots[itemDroppedData.equipSlotID].transform.position;
                    }
                    else
                    {
                        //Swap items from inventory to equipment
                        inventory.items[itemDroppedData.invSlotID] = itemSwappedData.item;
                        equipment.items[ID] = itemDroppedData.item;
                        itemSwappedTrans.transform.SetParent(rsMng.invSlots[itemDroppedData.invSlotID].transform);
                        itemSwappedTrans.transform.position = rsMng.invSlots[itemDroppedData.invSlotID].transform.position;
                        rsMng.player.AddEquipStats(itemDroppedData.item);
                        rsMng.player.RemoveEquipStats(itemSwappedData.item);
                    }
                    //Set the item to drop
                    itemDroppedData.equipSlotID = ID;
                    itemDroppedData.invSlotID = -1;
                }
            }
            rsMng.hud.UpdateCharacterStats(); //TODO delete/change it?            
        }

    }
}
