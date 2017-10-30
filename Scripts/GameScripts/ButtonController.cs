using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts {
    public class ButtonController : MonoBehaviour {

        PlayerController character;
        Color originColor;

    void Start()
        {
            //character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            originColor = transform.GetComponent<Image>().color;
        }

        //To make it lighter make a script for all the buttons 
        //and use a list of buttons to detect which button was pressed
        void OnTouchDown()
        {
            if (name.Equals("JumpBtn"))
                PlayerController.isJumping = true;
            if (name.Equals("AttackBtn"))
                ResourceManager.Instance.playerCtrl.Attack();
            if (name.Equals("DodgeBtn"))
                PlayerController.Dodge();
            if (name.Equals("TechniqueBtn"))
                PlayerController.playerAnmtr.Play("technique");
            if (name.Equals("InventoryBtn"))
                ResourceManager.Instance.inventory.ToggleInventory();
            if (name.Equals("AddItemBtn"))
                ResourceManager.Instance.AddItemInv(Random.Range(0, 2), ItemType.WEAPON);
            if (name.Equals("SaveBtn"))
                SaveSlotButtonHandler.SaveGame();
            if (name.Equals("LoadBtn"))
            {
                SceneSelector.Instance.LoadSceneByName("LoadScene");
                AudioManager.Instance.PlaySound("select");
            }
            if (name.Equals("ResumeBtn"))
            {
                SceneSelector.Instance.LoadSceneByName("MainScene");
                AudioManager.Instance.PlaySound("back");
            }
            transform.GetComponent<Image>().color = Color.Lerp(originColor, Color.red, 0.4f);
        }

        void OnTouchUp()
        {
            if (name.Equals("JumpBtn"))
                PlayerController.isJumping = false;

            transform.GetComponent<Image>().color = originColor;
        }

        void OnTouchStay()
        {
            transform.GetComponent<Image>().color = Color.Lerp(originColor, Color.red, 0.4f);
        }

        void OnTouchExit()
        {
            transform.GetComponent<Image>().color = originColor;
        }

        public void GetTouchInput(string input)
        {
            if (input.Equals("OnTouchDown"))
                OnTouchDown();
            else if (input.Equals("OnTouchUp"))
                OnTouchUp();
            else if (input.Equals("OnTouchStay"))
                OnTouchStay();
            else if (input.Equals("OnTouchExit"))
                OnTouchExit();
            else
                throw new System.Exception("Incorrect touch input!");
        }
    }
}
