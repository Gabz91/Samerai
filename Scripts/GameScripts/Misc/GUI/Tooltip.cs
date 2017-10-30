using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace GameScripts
{
    public class Tooltip
    {
        string data;
        GameObject gameObject;

        public Tooltip()
        {
            gameObject = GameObject.Find("Tooltip");
            gameObject.SetActive(false);
        }

        public void Update()
        {
            if (gameObject.activeSelf)
            {
                gameObject.transform.position = Input.mousePosition - new Vector3(100, 100);
            }
        }

        public void Activate(Item item)
        {
            ConstructDataString(item);
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void ConstructDataString(Item item)
        {
            data = "<b>" + item.Name + "</b>\n\n" + item.Description + "\nPower: " + ((Weapon)item).Power + ".";
            gameObject.transform.GetComponentInChildren<Text>().text = data;
        }
    }

}