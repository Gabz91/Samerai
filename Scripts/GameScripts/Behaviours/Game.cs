using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenericScripts;

namespace GameScripts
{
    public class Game : MonoSingleton<Game>
    {

        //Constants
        private const int MAX_ENEMIES_ON_STAGE = 20;
        //Temp
        public static int stage = 0;
        GameObject stageComplete;
        GameState currentGame;
        public PlayerCharacter player;
		bool isMainSceneLoaded = false;

        
        //Position fields
        public static float positionX;
        public static float positionZ;

        //Pools
        PoolOfObjects foesPool, soulsPool, itemsPool;
        //Enemy fields
        GameObject newFoe, newSoul, newItem;
        public static int enemiesKilled = 0;
        public static int enemiesToKill = 0;
        public static int enemiesSpawned = 0;
        /// <summary>
        /// Determines how much time before the next foe is spawned
        /// </summary>
        float spawnDelay = 4f;

        //Managers
        public static ResourceManager rsMng;

        void Start()
        {
            rsMng = ResourceManager.Instance;
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.P))
            //{
            //    ParticleSystem ps = GameObject.Find("SmokeEmit").GetComponent<ParticleSystem>();
            //    ps.Stop();
            //    ps.Play();
            //}
            if (Application.platform == RuntimePlatform.PS4 && SceneSelector.Instance.currentScene.name == "MenuScene" || Input.GetJoystickNames() != null)
            {
                SceneSelector.Instance.Update();
            }
            if (SceneSelector.Instance.currentScene.name == "MainScene" && isMainSceneLoaded)
            {
                rsMng.playerCtrl.Update();
                //Tooltip
                rsMng.tooltip.Update();
            }
        }

        void FixedUpdate()
        {
            if (SceneSelector.Instance.currentScene.name == "MainScene" && isMainSceneLoaded)
            {
                //Player Movement
                rsMng.playerCtrl.FixedUpdate();
            }
        }

        void InitNewStage()
        {
            Debug.Log("NEWSTAGE");
            stage++;
            enemiesToKill = 1 + (int)(1.5f * stage);
            enemiesKilled = 0;
            enemiesSpawned = 0;
            rsMng.hud.UpdateStageStats();
            StartCoroutine("SpawnNextFoe");
        }

        void SetGame(int profileNumber)
        {
            SaveSlotButtonHandler.LoadGameState(profileNumber);
            currentGame = MemoryCard.Instance.CurrentGameState;

            SceneSelector.Instance.LoadSceneByName("MainScene");
            rsMng.LoadMainScene();
            player = rsMng.player;

            soulsPool = new PoolOfObjects(rsMng.soulsPool, rsMng.soulPrefab);
            foesPool = new PoolOfObjects(rsMng.foesPool, rsMng.foePrefab);
            itemsPool = new PoolOfObjects(rsMng.itemsPool, rsMng.itemPrefab);
            if (stageComplete == null)
            {
                stageComplete = GameObject.Find("StageCompleteText");
                stageComplete.SetActive(false);
            }

            rsMng.hud.UpdateCharacterStats();

            //TODO Try to save enemyspawned
            stage = currentGame.stage;
            enemiesToKill = currentGame.enemiesToKill;
            enemiesKilled = currentGame.enemiesKilled;
            enemiesSpawned = enemiesKilled;

            //Reset the pools
            foesPool.EmptyPool();
            soulsPool.EmptyPool();
            rsMng.hud.UpdateStageStats();
            StartCoroutine("SpawnNextFoe");
        }

        public void StartGame(int profileNumber)
        {
            SetGame(profileNumber);
            isMainSceneLoaded = true;
            AudioManager.Instance.PlaySound("select");
        }

        public void LoadGame(int profileNumber)
        {
            SetGame(profileNumber);
            isMainSceneLoaded = true;
            AudioManager.Instance.PlaySound("select");
        }

        public void HitThePlayer(int damage)
        {
            if (!rsMng.playerCtrl.isInvulnerable)
            {
                Debug.Log("An enemy hit you!");
                player.InflictDamage(damage);
                ResourceManager.Instance.hud.UpdateCharacterStats();
                StartCoroutine(rsMng.playerCtrl.InvulnerabilityGap());
            }            
        }

        //Generates a new foe in the stage
        void SpawnFoe()
        {
            int typeRndFactor = Random.Range(0, 2);
            positionX = Random.Range(rsMng.floorColl.center.x - rsMng.floorColl.bounds.extents.x, rsMng.floorColl.size.x);
            positionZ = Random.Range(rsMng.floorColl.center.z - rsMng.floorColl.bounds.extents.z, rsMng.floorColl.size.z);

            //Test for the particles emitter
            //Transform pst = GameObject.Find("SmokeEmit").transform;
            //pst.position = new Vector3(positionX, -0.5f, positionZ);
            //ParticleSystem ps = pst.GetComponent<ParticleSystem>();
            //Debug.Log(pst.name);
            //ps.Stop();
            //ps.Play();

            newFoe = foesPool.GetObject();
            newFoe.GetComponentInChildren<EnemyController>().foe = new NPC((NPCType)typeRndFactor, 1 + stage);
            newFoe.transform.position = new Vector3(positionX, 0, positionZ);
            enemiesSpawned++;
            if (enemiesSpawned < enemiesToKill && enemiesSpawned < MAX_ENEMIES_ON_STAGE)
                StartCoroutine("SpawnNextFoe");
        }

        IEnumerator SpawnNextFoe()
        {
            yield return new WaitForSeconds(spawnDelay);
            if (enemiesSpawned < enemiesToKill && enemiesSpawned < MAX_ENEMIES_ON_STAGE)
                SpawnFoe();
        }

        IEnumerator StageCompletition()
        {
            stageComplete.SetActive(true);
            yield return new WaitForSeconds(stageComplete.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            stageComplete.SetActive(false);
            InitNewStage();
        }

        public void KillFoe(GameObject foeObj)
        {
            NPC foe = foeObj.GetComponentInChildren<EnemyController>().foe;
            player.GainExp(foe.EXP);
            SpawnSoul(foeObj);

            //Drop chance
            int dropFactor = Random.Range(0, 2);
            if (dropFactor == 1)
                SpawnItem(foeObj);

            rsMng.hud.UpdateCharacterStats();
            foesPool.DisableObject(foeObj);
            enemiesKilled++;            
            if (enemiesKilled == enemiesToKill)
                StartCoroutine("StageCompletition");
            rsMng.hud.UpdateStageStats();
            
        }

        void SpawnSoul(GameObject foeSlained)
        {
            NPC foe = foeSlained.GetComponentInChildren<EnemyController>().foe;
            newSoul = soulsPool.GetObject();
            newSoul.GetComponent<Collectible>().value = foe.SoulValue;
            newSoul.GetComponent<Collectible>().type = CollectibleType.SOUL;
            newSoul.transform.position = foeSlained.transform.position;
        }

        void SpawnItem(GameObject foeSlained)
        {
            //It only spawn weapons at the moment. Needs to be changed to something more flexible in the future.
            int id = ItemDatabase.GetRandomWeaponID();
            newItem = itemsPool.GetObject();
            newItem.GetComponent<Collectible>().id = id;
            newItem.GetComponent<Collectible>().type = CollectibleType.WEAPON;
            newItem.transform.position = foeSlained.transform.position;
            newItem.transform.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>("Art/Icons/" + ItemDatabase.GetWeaponSlug(id));
        }

        //Removes a soul from the screen when the player collects it
        public void CollectSoul(GameObject soulCollected)
        {
            soulsPool.DisableObject(soulCollected);
            rsMng.hud.UpdateCharacterStats();
        }

        //Removes an item from the screen when the player collects it
        public void CollectItem(GameObject itemCollected)
        {
            Debug.Log("COLLECTING " + itemCollected.name);
            itemsPool.DisableObject(itemCollected);
        }

    }
}




