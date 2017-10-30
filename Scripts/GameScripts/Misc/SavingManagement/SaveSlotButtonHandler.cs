using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

namespace GameScripts
{
    /// <summary>
    /// Attach this to any game object. I like to attach it to the canvas containing the buttons that will
    /// load profiles. That way it's in a 'logical' place and easy to find.
    /// </summary>
    public class SaveSlotButtonHandler
    {

        // Load the saved data of the selected profile number if there is any.
        // Set up the selected scene. The parameter can be 1,2,3 or 4.
        /// <summary>
        /// Load data from the current Game State,
        /// </summary>
        /// <param name="profileNumber"></param>
        public static void LoadGameState(int profileNumber)
        {
            // Load the save data file
            MemoryCard.Instance.LoadGameState(profileNumber);
        }

        // This should be assigned to each button via the inspector.
        // The parameter in the inspector's on click event will be 1,2, or 3
        /// <summary>
        /// Called from the OnClick methods for buttons.
        /// </summary>
        public static void SaveGame()
        {
            MemoryCard.Instance.WriteGameState();
            AudioManager.Instance.PlaySound("life");
        }
    }
}