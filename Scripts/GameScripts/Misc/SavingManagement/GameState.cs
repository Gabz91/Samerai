using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameScripts
{
    /// <summary>
    /// Responsible for:
    /// - Maintaining the stats for a player and their progress
    /// - Writing this data to a file.
    /// - Reading this data from a file.
    /// </summary>
    public class GameState
    {

        #region Defaults
        private const int DEFAULT_STAGE = 1;
        private const int DEFAULT_ENM_KILLED = 0;
        private const int DEFAULT_ENM_TOKILL = 2;
        #endregion

        // We initialize all of the stats to be default values.
        //public int coins = DEFAULT_COINS;
        //public int health = DEFAULT_HEALTH;
        //public int lives = DEFAULT_LIVES;
        //public string lastLevel = DEFAULT_LEVEL;

        public Inventory inventoryState;
        public Equipment equipmentState;
        public PlayerCharacter playerState;
        public float playTime;
        public int stage = DEFAULT_STAGE;
        public int enemiesKilled = DEFAULT_ENM_KILLED;
        public int enemiesToKill = DEFAULT_ENM_TOKILL;

        const bool DEBUG_ON = true;

        /// <summary>
        /// Writes the instance of this class to the specified file in JSON format.
        /// </summary>
        /// <param name="filePath">The file name and full path to write to.</param>
        public void WriteToFile(string filePath)
        {
            
            SaveGameState();
            // Convert the instance ('this') of this class to a JSON string with "pretty print" (nice indenting).
            string json = JsonUtility.ToJson(this, true);

            // Write that JSON string to the specified file.
            File.WriteAllText(filePath, json);

            //ResourceManager.Instance.SetInventory();

            // Tell us what we just wrote if DEBUG_ON is on.
            if (DEBUG_ON)
                Debug.LogFormat("WriteToFile({0}) -- data:\n{1}", filePath, json);
        }

        /// <summary>
        /// Returns a new GameState object read from the data in the specified file.
        /// </summary>
        /// <param name="filePath">The file to attempt to read from.</param>
        public static GameState ReadFromFile(string filePath)
        {
            // If the file doesn't exist then just return the default object.
            if (!File.Exists(filePath))
            {
                Debug.LogErrorFormat("ReadFromFile({0}) -- file not found, returning new object", filePath);
                return new GameState();
            }
            else
            {
                // If the file does exist then read the entire file to a string.
                string contents = File.ReadAllText(filePath);

                // If debug is on then tell us the file we read and its contents.
                if (DEBUG_ON)
                    Debug.LogFormat("ReadFromFile({0})\ncontents:\n{1}", filePath, contents);

                // If it happens that the file is somehow empty then tell us and return a new GameState object.
                if (string.IsNullOrEmpty(contents))
                {
                    Debug.LogErrorFormat("File: '{0}' is empty. Returning default SaveData");
                    return new GameState();
                }

                // Otherwise we can just use JsonUtility to convert the string to a new GameState object.
                return JsonUtility.FromJson<GameState>(contents);
            }
        }

        /// <summary>
        /// Saves the current state of the game in this GameState object.
        /// </summary>
        public void SaveGameState()
        {
            Debug.Log("Game Saved!");
            //Save player and resources state
            ResourceManager currentRsMng = ResourceManager.Instance;
            inventoryState = currentRsMng.inventory;
            equipmentState = currentRsMng.equipment;
            playerState = currentRsMng.player;

            //Save state of the stage
            stage = Game.stage;
            enemiesKilled = Game.enemiesKilled;
            enemiesToKill = Game.enemiesToKill;

            playTime += Time.time;
        }

        /// <summary>
        /// This is used to check if the GameState object is the same as the default.
        /// i.e. it hasn't been written to yet.
        /// </summary>
        //public bool IsDefault()
        //{
        //    return (
        //        coins == DEFAULT_COINS &&
        //        health == DEFAULT_HEALTH &&
        //        lives == DEFAULT_LIVES &&
        //        lastLevel == DEFAULT_LEVEL &&
        //        powerUps.Count == 0);
        //}

        /// <summary>
        /// A friendly string representation of this object.
        /// </summary>
        //public override string ToString()
        //{
        //    string[] powerUpsStrings = new string[powerUps.Count];
        //    for (int i = 0; i < powerUps.Count; i++)
        //    {
        //        powerUpsStrings[i] = powerUps[i].ToString();
        //    }

        //    return string.Format(
        //        "coins: {0}\nhealth: {1}\nlives: {2}\npowerUps: {3}\nlastLevel: {4}",
        //        coins,
        //        health,
        //        lives,
        //        "[" + string.Join(",", powerUpsStrings) + "]",
        //        lastLevel
        //        );
        //}
    }
}
