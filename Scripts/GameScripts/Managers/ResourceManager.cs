using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts
{

    /// <summary>
    /// Manages the lifetime of all the resources used in the game.
    /// Loads every resource needed accordingly to the scene.
    /// TODO change the way the resources are disposed. Either use an unload method to dispose the resources as soon as they are not needed anymore.
    /// </summary>
    public class ResourceManager
    {
        /// <summary>
        /// The singleton ResourceManager instance.
        /// </summary>
        static ResourceManager instance;
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("Resourcemanager");
                    instance = new ResourceManager();
                }
                return instance;
            }
        }

        ///////////////MenuScene/////////////////////////////////////////////
        //Audio                                                            
        public AudioManager audioMng;                                      
        public AudioClip select;                                           
        //GameObjects                                                      
        public GameObject savedGamePrefab, newGamePrefab;                                 
        public GameObject audioMngmGameObject;

        ///////////////MainScene/////////////////////////////////////////////
        //Audio           
        public AudioClip jump, attack, life;
        //Gameobjects and Transforms
        public GameObject playerPrefab, foePrefab, soulPrefab, slotPrefab, itemPrefab, kunaiPrefab, fireballPrefab;  //Prefabs
        public GameObject inventoryPanel, equipmentPanel, characterStatusPanel, stagePanel;
        public Transform playerTrans;
        public Transform foesPool, soulsPool, kunaisPool; //Pools of objects
        //Misc
        public Inventory inventory;
        public Equipment equipment;
        public ItemDatabase itemDB;
        public HUD hud;
        public Tooltip tooltip;
        public PlayerCharacter player;
        public PlayerController playerCtrl;
        public BoxCollider floorColl;

        ///////////////Shared//////////////////////////////////
        //Scenes prefabs
        public GameObject mainScenePrefab, loadScenePrefab;
        //Lists
        public GameObject[] invSlots = new GameObject[Inventory.slotsAmount];
        public GameObject[] equipSlots = new GameObject[Equipment.slotsAmount];

        /// <summary>
        /// The private ontructor of the singleton class.
        /// Initializes the menu scene needed at the start of the game.
        /// </summary>
        private ResourceManager()
        {
            LoadMenuScene();
        }

        /// <summary>
        /// Loads the resources needed within the menu scene.
        /// </summary>
        public void LoadMenuScene()
        {
            //Loading resources
            LoadMenuGameObjects();
            LoadMenuAudio();
            //Initialization
            audioMng = AudioManager.SetInstance(audioMngmGameObject);
        }

        /// <summary>
        /// Load the resources needed within the main scene.
        /// Initializes an instance for most of the classes needed in the main scene. 
        /// If the game was loaded, it sets those instances to previously saved instances instead.
        /// </summary>
        public void LoadMainScene()
        {
            //Loading resources
            LoadMainGameObjects();
            LoadMainAudio();
            //Classes initialization. Change this for a better rusable code.
            if (MemoryCard.Instance.isDataLoaded)
                player = MemoryCard.Instance.CurrentGameState.playerState;
            else
                player = new PlayerCharacter();
            if (playerCtrl == null)
                playerCtrl = new PlayerController(playerTrans);
            itemDB = ItemDatabase.Instance;
            InitializeInvEquip();
            if (tooltip == null)
                tooltip = new Tooltip();
            if (hud == null)
                hud = new HUD(characterStatusPanel, stagePanel);
            else
                hud.UpdatePlayer();
        }

        void LoadMainAudio()
        {
            if (jump == null)
            {
                //AudioClips
                jump = (AudioClip)Resources.Load("Audio/SFX/jump", typeof(AudioClip));
                attack = (AudioClip)Resources.Load("Audio/SFX/attack", typeof(AudioClip));
                life = (AudioClip)Resources.Load("Audio/SFX/life", typeof(AudioClip));
            }
        }

        void LoadMainGameObjects()
        {
            if (playerPrefab == null)
            {
                //Prefabs
                playerPrefab = (GameObject)Resources.Load("Prefabs/Characters/PlayerCharacter", typeof(GameObject));
                foePrefab = (GameObject)Resources.Load("Prefabs/Characters/FoeCtrl", typeof(GameObject));
                soulPrefab = (GameObject)Resources.Load("Prefabs/Soul", typeof(GameObject));
                itemPrefab = (GameObject)Resources.Load("Prefabs/Item", typeof(GameObject));
                slotPrefab = (GameObject)Resources.Load("Prefabs/Slot", typeof(GameObject));
                kunaiPrefab = (GameObject)Resources.Load("Prefabs/Projectiles/Kunai", typeof(GameObject));
                fireballPrefab = (GameObject)Resources.Load("Prefabs/Projectiles/Fireball", typeof(GameObject));
                //GameObjects
                inventoryPanel = GameObject.Find("InventoryPanel");
                equipmentPanel = GameObject.Find("EquipmentPanel");
                characterStatusPanel = GameObject.Find("CharacterStatus");
                stagePanel = GameObject.Find("StagePanel");
                //Transforms
                GameObject playerCharacter = GameObject.Instantiate(playerPrefab, GameObject.Find("MainScene").transform.Find("Gameplay"));
                playerCharacter.name = "PlayerCharacter";
                playerTrans = playerCharacter.transform;

                //Temp to relocate
                foesPool = GameObject.FindGameObjectWithTag("FoesPool").transform;
                floorColl = GameObject.FindGameObjectWithTag("Floor").GetComponent<BoxCollider>();
                soulsPool = GameObject.FindGameObjectWithTag("SoulsPool").transform;
                kunaisPool = GameObject.FindGameObjectWithTag("KunaisPool").transform;
            }
        }

        void LoadMenuAudio()
        {
            //AudioClips
            select = (AudioClip)Resources.Load("Audio/SFX/select", typeof(AudioClip));
        }

        void LoadMenuGameObjects()
        {
            //Prefabs
            if (Application.platform == RuntimePlatform.Android)
                mainScenePrefab = (GameObject)Resources.Load("Prefabs/Scenes/MainSceneMobile", typeof(GameObject));
            else
                mainScenePrefab = (GameObject)Resources.Load("Prefabs/Scenes/MainScene", typeof(GameObject));
            loadScenePrefab = (GameObject)Resources.Load("Prefabs/Scenes/LoadScene", typeof(GameObject));
            savedGamePrefab = (GameObject)Resources.Load("Prefabs/Menu/SavedGame", typeof(GameObject));
            newGamePrefab = (GameObject)Resources.Load("Prefabs/Menu/NewGame", typeof(GameObject));
            //GameObjects
            audioMngmGameObject = GameObject.Find("AudioManager");
        }

        void InitializeInvEquip()
        {
            if (invSlots[0] == null)
            {
                for (int i = 0; i < Inventory.slotsAmount; i++)
                {
                    invSlots[i] = (GameObject.Instantiate(slotPrefab, inventoryPanel.transform));
                    invSlots[i].name = "Slot";
                    invSlots[i].GetComponent<Slot>().ID = i;
                }
                inventoryPanel.SetActive(false);

                for (int i = 0; i < Equipment.slotsAmount; i++)
                {
                    equipSlots[i] = (GameObject.Instantiate(slotPrefab, equipmentPanel.transform));
                    equipSlots[i].name = "Slot";
                    equipSlots[i].GetComponent<Slot>().ID = i;
                }
            }

            
            if (MemoryCard.Instance.isDataLoaded)
            {
                inventory = MemoryCard.Instance.CurrentGameState.inventoryState;
                equipment = MemoryCard.Instance.CurrentGameState.equipmentState;
            }
            else
            {
                //Initialize default state of inventory
                inventory = Inventory.Instance;
                equipment = Equipment.Instance;
            }
            SetInventory();
            SetEquipment();
        }

        public void AddItemInv(int id, ItemType type)
        {
            Item newItem = itemDB.GetItemByID(id, type);
            for (int i = 0; i < invSlots.Length; i++)
            {
                if (invSlots[i].transform.childCount == 0)
                {
                    inventory.items[i] = newItem;
                    GameObject itemObj = GameObject.Instantiate(itemPrefab, invSlots[i].transform);
                    ItemData itemData = itemObj.GetComponent<ItemData>();
                    itemData.invSlotID = i;
                    itemData.item = newItem;
                    itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Icons/" + newItem.Slug); //Change this to some local method to get the icon. Iterate through a List<Sprite> to get the right sprite.
                    itemObj.name = newItem.Name;

                    ((RectTransform)itemObj.transform).sizeDelta = new Vector2(inventory.slotSize.x - inventory.itemOffset, inventory.slotSize.y - inventory.itemOffset);
                    break;
                }
                if (i == Inventory.slotsAmount - 1)
                {
                    Debug.Log("The inventory is full!");
                }
            }
        }

        public void SetInventory()
        {
            for (int i = 0; i < Inventory.slotsAmount; i++)
            {
                Item newItem = inventory.items[i];
                if (newItem != null && newItem.Name != "" && newItem.Name != null)
                {
                    newItem = itemDB.GetItemByID(newItem.id, newItem.type);
                    inventory.items[i] = newItem;
                    GameObject itemObj;
                    if (invSlots[i].transform.childCount == 0)
                        itemObj = GameObject.Instantiate(itemPrefab, invSlots[i].transform);
                    else
                        itemObj = invSlots[i].transform.GetChild(0).gameObject;
                    ItemData itemData = itemObj.GetComponent<ItemData>();
                    itemData.invSlotID = i;
                    itemData.item = newItem;
                    itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Icons/" + newItem.Slug);
                    itemObj.name = newItem.Name;
                    ((RectTransform)itemObj.transform).sizeDelta = new Vector2(inventory.slotSize.x - inventory.itemOffset, inventory.slotSize.y - inventory.itemOffset);
                }
                else
                {
                    inventory.items[i] = null;
                    if (invSlots[i].transform.childCount > 0)
                        GameObject.Destroy(invSlots[i].transform.GetChild(0).gameObject);
                }
            }
        }

        public void SetEquipment()
        {
            GameObject itemObj;
            for (int i = 0; i < Equipment.slotsAmount; i++)
            {
                Item newItem = equipment.items[i];
                if (newItem != null && newItem.Name != "" && newItem.Name != null)
                {
                    newItem = itemDB.GetItemByID(newItem.id, newItem.type);
                    equipment.items[i] = newItem;
                    if (equipSlots[i].transform.childCount == 0)
                        itemObj = GameObject.Instantiate(itemPrefab, equipSlots[i].transform);
                    else
                        itemObj = equipSlots[i].transform.GetChild(0).gameObject;
                    ItemData itemData = itemObj.GetComponent<ItemData>();
                    itemData.equipSlotID = i;
                    itemData.item = newItem;
                    itemObj.GetComponent<Image>().sprite = Resources.Load<Sprite>("Art/Icons/" + newItem.Slug);
                    itemObj.name = newItem.Name;
                    ((RectTransform)itemObj.transform).sizeDelta = new Vector2(equipment.slotSize.x - equipment.itemOffset, equipment.slotSize.y - equipment.itemOffset);
                }
                else
                {
                    equipment.items[i] = null;
                    if (equipSlots[i].transform.childCount > 0)
                        GameObject.Destroy(equipSlots[i].transform.GetChild(0).gameObject);
                }
            }
        }
    }
}
