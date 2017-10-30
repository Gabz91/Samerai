using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class PlayerController
    {
        //Movement fields
        public static float X_AXIS = 0;
        public static float Z_AXIS = 0;
        float x, y, z;
        float gravity = 9.8f;
        public float speed = 2.5f;
        public float jumpSpeed = 4.0f;

        bool facingRight = true;
        public static bool isJumping = false;
        public static bool isDodging = false;
        public static bool isAttacking = false;
        public static bool isUsingTech = false;

        public static CharacterController characterCtrl;
        public static Animator playerAnmtr;
        Transform transform;

        public static int noOfClicks = 0;
        //Time when last button was clicked
        float lastClickedTime = 0;
        //Delay between clicks for which clicks will be considered as combo
        float maxComboDelay = 1;

        float invulnerabilityTime = 2f;
        public bool isInvulnerable = false;

        // Use this for initialization
        public PlayerController(Transform playerTrans)
        {
            transform = playerTrans;
            characterCtrl = transform.GetComponent<CharacterController>();
            playerAnmtr = transform.GetComponent<Animator>();
        }

        // Update is called once per frame
        public void Update()
        {
            if (Time.time - lastClickedTime > maxComboDelay)
                noOfClicks = 0;
            if (Input.GetKeyDown(KeyCode.E))
                Dodge();
            if (Input.GetKeyDown(KeyCode.Q) && !isDodging) 
                Attack();
            if (Input.GetKeyDown(KeyCode.Alpha1))
                playerAnmtr.Play("technique");
            if (Input.GetKeyDown(KeyCode.I))
                Inventory.Instance.ToggleInventory();

            GetControllerInput();
        }

        void GetControllerInput()
        {
            if (Application.platform == RuntimePlatform.PS4)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                    isJumping = true;
                else
                    isJumping = false;
                if (Input.GetKeyDown(KeyCode.JoystickButton3))
                    Dodge();
                if (Input.GetKeyDown(KeyCode.JoystickButton1) && !isDodging)
                    Attack();
                if (Input.GetKeyDown(KeyCode.JoystickButton2))
                    playerAnmtr.Play("technique");
                if (Input.GetKeyDown(KeyCode.JoystickButton7))
                    Inventory.Instance.ToggleInventory();
            }
            else if (Input.GetJoystickNames() != null)
            {
                if (Input.GetKeyDown(KeyCode.JoystickButton1))
                    isJumping = true;
                else
                    isJumping = false;
                if (Input.GetKeyDown(KeyCode.JoystickButton3))
                    Dodge();
                if (Input.GetKeyDown(KeyCode.JoystickButton2) && !isDodging)
                    Attack();
                if (Input.GetKeyDown(KeyCode.JoystickButton0))
                    playerAnmtr.Play("technique");
                if (Input.GetKeyDown(KeyCode.JoystickButton9))
                    Inventory.Instance.ToggleInventory();
            }
            
        }

        public void FixedUpdate()
        {
            GetMovement();
        }

        void GetMovement()
        {
            x = 0;
            z = 0;

            x = (Input.GetAxis("Horizontal") + X_AXIS) * speed;
            z = (Input.GetAxis("Vertical")*1.5f + Z_AXIS) * speed;

            if (characterCtrl.isGrounded)
            {
                y = 0; // grounded character has vSpeed = 0...
                if ((Input.GetKeyDown("space") || isJumping) && !isDodging)
                { // unless it jumps:
                    y = jumpSpeed;
                }
            }

            y -= gravity * Time.deltaTime;


            Vector3 vel = new Vector3(x, y, z);

            //Reduces the speed on the Z axis when the player is attacking
            //if (isAttacking)
            //    vel = new Vector3(transform.right.x/2, y, z/3);
            if (isDodging)
                vel = new Vector3(transform.right.x*4, y, z);
            if (isUsingTech)
                vel = Vector3.zero;

            characterCtrl.Move(vel * Time.deltaTime);

            playerAnmtr.SetFloat("vSpeed", vel.y);
            playerAnmtr.SetFloat("Speed", Mathf.Abs(vel.x));
            playerAnmtr.SetBool("Ground", characterCtrl.isGrounded);

            if (vel.x > 0 && !facingRight)
                Flip();
            else if (vel.x < 0 && facingRight)
                Flip();
        }        

        void Flip()
        {
            facingRight = !facingRight;
            transform.Rotate(new Vector3(0, (facingRight ? 180 : -180), 0));
        }

        public void Attack()
        {
            //Record time of last button click
            if (Time.time - lastClickedTime > 0.2f && noOfClicks < 3)
            {
                noOfClicks++;
                lastClickedTime = Time.time;
            }
            if (noOfClicks == 1 && !isAttacking)
            {
                playerAnmtr.Play("attack1");
                isAttacking = true;
            }
        }

        public IEnumerator InvulnerabilityGap()
        {
            isInvulnerable = true;
            yield return new WaitForSeconds(invulnerabilityTime);
            isInvulnerable = false;
        }

        public static void Dodge()
        {
            if (characterCtrl.isGrounded)
                playerAnmtr.Play("dodge");
        }
    }
}

