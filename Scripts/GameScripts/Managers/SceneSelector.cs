using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

namespace GameScripts
{
    public class SceneSelector
    {
        static SceneSelector instance;
        public static SceneSelector Instance
        {
            get
            {
                if (instance == null)
                {
					
                    Debug.Log("SceneSelector");
                    instance = new SceneSelector();
                    instance.Initialize();
                }
                return instance;
            }
        }

        ResourceManager rsMng;
        public static List<GameObject> gameObjects;
        GameObject mainScene, menuScene, loadScene;
        public GameObject currentScene;
        Transform gameSlotsMS, gameSlotsLS;

        Button[] gameSlotButtons;
        int selected;
        bool x_axisBeingUsed = false;
        bool y_axisBeingUsed = false;
        Button[] mainSceneButtons;


        public void Update()
        {
            //If current scene is menu scene
            #region
            if (currentScene == menuScene)
            {
                if (Input.GetAxisRaw("HorizontalPad") != 0)
                {
                    if (!x_axisBeingUsed)
                    {
                        AudioManager.Instance.PlaySound("select");
                        x_axisBeingUsed = true;
                        if (selected == 0)
                        {
                            gameSlotButtons[1].Select();
                            selected = 1;
                        }
                        else if (selected == 1)
                        {
                            gameSlotButtons[0].Select();
                            selected = 0;
                        }
                        else if (selected == 2)
                        {
                            gameSlotButtons[3].Select();
                            selected = 3;
                        }
                        else if (selected == 3)
                        {
                            gameSlotButtons[2].Select();
                            selected = 2;
                        }
                    }
                }
                if (Input.GetAxisRaw("VerticalPad") != 0)
                {
                    if (!y_axisBeingUsed)
                    {
                        AudioManager.Instance.PlaySound("select");
                        y_axisBeingUsed = true;
                        if (selected == 0)
                        {
                            gameSlotButtons[2].Select();
                            selected = 2;
                        }
                        else if (selected == 1)
                        {
                            gameSlotButtons[3].Select();
                            selected = 3;
                        }
                        else if (selected == 2)
                        {
                            gameSlotButtons[0].Select();
                            selected = 0;
                        }
                        else if (selected == 3)
                        {
                            gameSlotButtons[1].Select();
                            selected = 1;
                        }
                    }
                }
                if (Input.GetAxisRaw("HorizontalPad") == 0)
                    x_axisBeingUsed = false;
                if (Input.GetAxisRaw("VerticalPad") == 0)
                    y_axisBeingUsed = false;
                if (Application.platform == RuntimePlatform.PS4)
                {
                    if (Input.GetKeyDown(KeyCode.JoystickButton0))
                        gameSlotButtons[selected].onClick.Invoke();
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.JoystickButton1))
                        gameSlotButtons[selected].onClick.Invoke();
                }
            }
            #endregion 
            #region
            if (currentScene == mainScene)
            {
               
            }
            #endregion 
        }

        void Initialize()
        {
            rsMng = Game.rsMng;
            menuScene = GameObject.Find("MenuScene");
            SetScene("MenuScene");
            if (Application.platform == RuntimePlatform.PS4 || Input.GetJoystickNames() != null)
            {
                SetUpPS4Variables();
            }
        }

        public void SetScene(string activeScene)
        {
            switch (activeScene)
            {
                case "MenuScene":
                    ChangeActiveScene(menuScene);
                    SetUpMenuScene();
                    break;
                case "MainScene":
                    if (mainScene == null)
                    {
                        mainScene = GameObject.Instantiate(rsMng.mainScenePrefab);
                        mainScene.name = "MainScene";
                    }
                    ChangeActiveScene(mainScene);
                    break;
                case "LoadScene":
                    if (loadScene == null)
                    {
                        loadScene = GameObject.Instantiate(rsMng.loadScenePrefab);
                        loadScene.name = "LoadScene";
                    }
                    SetUpLoadScene();
                    ChangeActiveScene(loadScene);
                    break;
                default:
                    break;
            }
        }

        void SetUpMenuScene()
        {
            if (gameSlotsMS == null)
                gameSlotsMS = GameObject.Find("GameSlots").transform;

            

            if (gameSlotsMS.childCount != MemoryCard.MAX_NUMBER_OF_PROFILES)
            {
                Debug.LogError(
                    "Incorrect number of button labels. Must be exactly " +
                    MemoryCard.MAX_NUMBER_OF_PROFILES);
            }
            else
            {
                //Set up the game slot buttons
                GameObject newGameSlotObj;
                GameState[] savedGames = new GameState[MemoryCard.MAX_NUMBER_OF_PROFILES];
                MemoryCard.Instance.LoadSavedGames(savedGames);
                // For every possible profile number.
                for (int i = 0; i < MemoryCard.MAX_NUMBER_OF_PROFILES; i++)
                {
                    int index = gameSlotsMS.GetChild(i).GetSiblingIndex() + 1;
                    // If the profile file exists,
                    // Then set the label accordingly to the profile stats
                    if (savedGames[i] != null)
                    {
                        newGameSlotObj = GameObject.Instantiate(rsMng.savedGamePrefab, gameSlotsMS.GetChild(i));
                        newGameSlotObj.transform.Find("GameNumber").GetComponent<Text>().text = "Game " + (i+1);
                        Transform gameStats = newGameSlotObj.transform.Find("GameStats");
                        gameStats.Find("Level").GetComponent<Text>().text = "Level: " +savedGames[i].playerState.Level.ToString();
                        gameStats.Find("Time").GetComponent<Text>().text = "Time: " +savedGames[i].playTime;
                        gameStats.Find("Stage").GetComponent<Text>().text = "Stage: " +savedGames[i].stage;
                        gameSlotsMS.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { Game.Instance.LoadGame(index); });
                    }   
                    else
                    {
                        // Otherwise set the label to just say 'New Game" indicating it is an empty slot.
                        newGameSlotObj = GameObject.Instantiate(rsMng.newGamePrefab, gameSlotsMS.GetChild(i));
                        gameSlotsMS.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { Game.Instance.StartGame(index); });
                    }

                }
            }
        }

        void SetUpLoadScene()
        {
            if (gameSlotsLS == null)
                gameSlotsLS = GameObject.Find("GameSelectionScreen").transform.Find("GameSlots").transform;
            if (gameSlotsLS.childCount != MemoryCard.MAX_NUMBER_OF_PROFILES)
            {
                Debug.LogError(
                    "Incorrect number of button labels. Must be exactly " +
                    MemoryCard.MAX_NUMBER_OF_PROFILES);
            }
            else
            {
                //Set up the game slot buttons
                GameObject newGameSlotObj;
                GameState[] savedGames = new GameState[MemoryCard.MAX_NUMBER_OF_PROFILES];
                MemoryCard.Instance.LoadSavedGames(savedGames);
                // For every possible profile number.
                for (int i = 0; i < MemoryCard.MAX_NUMBER_OF_PROFILES; i++)
                {
                    int index = i + 1;
                    // If the profile file exists,
                    // Then set the label accordingly to the profile stats
                    if (savedGames[i] != null)
                    {
                        if (gameSlotsLS.GetChild(i).childCount == 0)
                        {
                            newGameSlotObj = GameObject.Instantiate(rsMng.savedGamePrefab, gameSlotsLS.GetChild(i));
                            newGameSlotObj.transform.Find("GameNumber").GetComponent<Text>().text = "Game " + (i + 1);
                            Transform gameStats = newGameSlotObj.transform.Find("GameStats");
                            gameStats.Find("Level").GetComponent<Text>().text = "Level: " + savedGames[i].playerState.Level.ToString();
                            gameStats.Find("Time").GetComponent<Text>().text = "Time: " + savedGames[i].playTime;
                            gameStats.Find("Stage").GetComponent<Text>().text = "Stage: " +savedGames[i].stage;
                            gameSlotsLS.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { Game.Instance.LoadGame(index); });
                        }
                        else
                        {
                            Transform gameStats = gameSlotsLS.GetChild(i).GetChild(0).transform.Find("GameStats");
                            gameStats.Find("Level").GetComponent<Text>().text = "Level: " + savedGames[i].playerState.Level.ToString();
                            gameStats.Find("Time").GetComponent<Text>().text = "Time: " + savedGames[i].playTime;
                            gameStats.Find("Stage").GetComponent<Text>().text = "Stage: " +savedGames[i].stage;
                        }
                    }
                    else
                    {
                        // Otherwise set the label to just say 'New Game" indicating it is an empty slot.
                        if (gameSlotsLS.GetChild(i).childCount == 0)
                        {
                            newGameSlotObj = GameObject.Instantiate(rsMng.newGamePrefab, gameSlotsLS.GetChild(i));
                            gameSlotsLS.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { Game.Instance.StartGame(index); });
                        }
                    }
                }
            }
        }

        //Load a new scene after the sound of the button has ended
        public void LoadSceneByName(string sceneName)
        {
            new WaitForSeconds(rsMng.select.length);
            SetScene(sceneName);
        }


        void ChangeActiveScene(GameObject newScene)
        {
            newScene.SetActive(true);
            if (currentScene != null)
                currentScene.SetActive(false);
            currentScene = newScene;
        }

        void SetUpPS4Variables()
        {
            gameSlotButtons = new Button[MemoryCard.MAX_NUMBER_OF_PROFILES];
            for (int i = 0; i < gameSlotsMS.childCount; i++)
                gameSlotButtons[i] = gameSlotsMS.GetChild(i).GetComponent<Button>();
            gameSlotButtons[0].Select();
            selected = 0;
        }
    }
}
