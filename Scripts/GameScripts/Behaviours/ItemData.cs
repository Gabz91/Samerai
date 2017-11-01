using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

namespace GameScripts
{
    public class ItemData : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        Transform itemsPool;
        public Item item;
        public int invSlotID = -1;
        public int equipSlotID = -1;
        public bool isItemBeingUsed = false;

        Tooltip tooltip;
        private Vector2 offset;

        void Start()
        {
            tooltip = Game.rsMng.tooltip;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (item != null)
            {
                offset = eventData.position - new Vector2(transform.position.x, transform.position.y);
                transform.SetParent(GameObject.Find("CharacterPanel").transform); //Makes the item on top of all the UI elements
                transform.position = eventData.position - offset;
                transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (item != null)
            {
                transform.position = eventData.position - offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (invSlotID != -1)
            {
                transform.SetParent(Game.rsMng.invSlots[invSlotID].transform);
                transform.position = Game.rsMng.invSlots[invSlotID].transform.position;
            }
            else
            {
                transform.SetParent(Game.rsMng.equipSlots[equipSlotID].transform);
                transform.position = Game.rsMng.equipSlots[equipSlotID].transform.position;
            }
            transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltip.Activate(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.Deactivate();
        }
    }
}
