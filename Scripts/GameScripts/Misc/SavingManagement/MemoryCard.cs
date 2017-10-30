using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

namespace GameScripts
{
    /// <summary>
    /// This class is responsible for loading/saving data.
    /// </summary>
    public class MemoryCard
    {
        static MemoryCard instance;
        public static MemoryCard Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("MemoryCard");
                    instance = new MemoryCard();
                }
                return instance;
            }
        }

        //public PlayerPrefsHandler prefs { get; private set; }

        /// <summary>
        /// The currently loaded Save Data.
        /// </summary>
        public GameState CurrentGameState { get; private set; }

        /// <summary>
        /// Use this to prevent reloading the data when a new scene loads.
        /// </summary>
        public bool isDataLoaded = false;

        /// <summary>
        /// Store the currently loaded profile number here.
        /// </summary>
        public int currentlyLoadedProfileNumber { get; private set; }

        /// <summary>
        /// The maximum number of profiles we'll allow our users to have.
        /// </summary>
        public const int MAX_NUMBER_OF_PROFILES = 4;

        /// <summary>
        /// Loads the save data for a specific profile number. 
        /// This will eventually be called from a button.
        /// </summary>
        /// <param name="profileNumber">(Optional) the profile number to load, 
        /// omit to automatically load the first profile found.</param>
        public void LoadGameState(int profileNumber = 0)
        {
            // Automatically load the first available profile.
            if (profileNumber <= 0)
            {
                // We iterate through the possible profile numbers in case one with a lower number
                // no longer exists.
                for (int i = 1; i <= MAX_NUMBER_OF_PROFILES; i++)
                {
                    if (File.Exists(GetGameStateFilePath(i)))
                    {
                        // Once the file is found, load it from the calculated file name.
                        CurrentGameState = GameState.ReadFromFile(GetGameStateFilePath(i));
                        // And set the current profile number for later use when we save.
                        currentlyLoadedProfileNumber = i;
                        break;
                    }
                }
            }
            else
            {
                // If the profileNumber parameter is supplied then we'll look to see if that exists.
                if (File.Exists(GetGameStateFilePath(profileNumber)))
                {
                    // If the file exists then load the SaveData from the calculated file name.
                    CurrentGameState = GameState.ReadFromFile(GetGameStateFilePath(profileNumber));
                    isDataLoaded = true;
                }
                else
                {
                    // Otherwise just return a new
                    CurrentGameState = new GameState();
                    isDataLoaded = false;
                    Debug.Log("DATANOTLOADED");
                }

                // And set the current profile number for later use when we save.
                currentlyLoadedProfileNumber = profileNumber;
            }
        }

        /// <summary>
        /// Loads all the saved games and stores them locally.
        /// </summary>
        public void LoadSavedGames(GameState[] savedGames)
        {
            //Iterates through the possible profile numbers.
            for (int i = 1; i <= MAX_NUMBER_OF_PROFILES; i++)
            {
                //If a saved game exists, it loads and stores it within the saved games.
                if (File.Exists(GetGameStateFilePath(i)) && savedGames[i-1] == null)
                {
                    savedGames[i-1] = GameState.ReadFromFile(GetGameStateFilePath(i));
                }
            }
        }

        /// <summary>
        /// The base name of our save data files.
        /// </summary>
        private const string GAME_STATE_FILE_NAME_BASE = "savedata";
        /// <summary>
        /// The extension of our save data files.
        /// </summary>
        private const string GAME_STATE_FILE_EXTENSION = ".txt";

        /// <summary>
        /// The directory our save data files will be stored in. 
        /// This is done through a getter because we're calling to a non-constant member (Application.dataPath)
        /// to construct this.
        /// </summary>
		private string GAME_STATE_DIRECTORY { get {
                if (Application.platform == RuntimePlatform.Android)
                {
                    return "/storage/emulated/0/Android/data/com.EasyGames.Samerai3D/Saves/";
                }
                else
                {
                    return Application.dataPath + "/Saves/";
                }
			} }

        /// <summary>
        /// The full path and file name for our SaveData file.
        /// ex: 'c:\projectdirectory\assets\saves\savedata1.txt'
        /// </summary>
        /// <param name="profileNumber">The number profile to load (must be greater than 0).</param>
        public string GetGameStateFilePath(int profileNumber)
        {
            // If the profile number is less than 1 then throw an exception.
            if (profileNumber < 1)
                throw new System.ArgumentException("profileNumber must be greater than 1. Was: " + profileNumber);

            // Ensure that the directory exists.
            if (!Directory.Exists(GAME_STATE_DIRECTORY))
                Directory.CreateDirectory(GAME_STATE_DIRECTORY);

            // Construct the string representation of the directory + file name.
            return GAME_STATE_DIRECTORY + GAME_STATE_FILE_NAME_BASE + profileNumber.ToString() + GAME_STATE_FILE_EXTENSION;
        }

        /// <summary>
        /// Writes the save data to file.
        /// </summary>
        public void WriteGameState()
        {
            // If for some accidental reason we forgot to assign a profile number,
            // then check to see if there is any unused profile number (i.e. a file doesn't exist for it). 
            if (currentlyLoadedProfileNumber <= 0)
            {
                for (int i = 1; i <= MAX_NUMBER_OF_PROFILES; i++)
                {
                    if (!File.Exists(GetGameStateFilePath(i)))
                    {
                        currentlyLoadedProfileNumber = i;
                        break;
                    }
                }
            }

            // If we couldn't find an empty profile then throw an exception because something went very wrong.
            if (currentlyLoadedProfileNumber <= 0)
            {
                throw new System.Exception("Cannot WriteSaveData. No available profiles and currentlyLoadedProfile = 0");
            }
            else
            {
                // Otherwise save the SaveData to file.

                // If the save data doesn't exist yet, 
                // then create a new default save data.
                if (CurrentGameState == null)
                {
                    CurrentGameState = new GameState();
                }

                // Finally save it to th file using the constructed path + file name
                CurrentGameState.WriteToFile(GetGameStateFilePath(currentlyLoadedProfileNumber));
            }
        }
    }
}