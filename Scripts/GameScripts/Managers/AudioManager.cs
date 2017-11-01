using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameScripts
{
    //Handles the audio in the game.
    public class AudioManager
    {
        static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    Debug.Log("AudioManager not set");
                }
                return instance;
            }
        }

        ResourceManager rsMng;
        static AudioSource audioSrc;
        GameObject gameObject;

        // Use this for initialization
        private AudioManager(GameObject amGameObject)
        {
            gameObject = amGameObject;
            audioSrc = gameObject.GetComponent<AudioSource>(); //TODO find this in the ResourceManager
        }

        public static AudioManager SetInstance(GameObject amGameObject)
        {
            if (instance == null)
            {
                instance = new AudioManager(amGameObject);
            }
            return instance;
        }

        public void PlaySound(string clipName)
        {
            if (rsMng == null)
            {
                rsMng = ResourceManager.Instance; //TODO This might create issues
            }

            switch (clipName)
            {
                case "select":
                    audioSrc.PlayOneShot(rsMng.select);
                    break;
                case "jump":
                    audioSrc.PlayOneShot(rsMng.jump);
                    break;
                case "attack":
                    audioSrc.PlayOneShot(rsMng.attack);
                    break;
                case "life":
                    audioSrc.PlayOneShot(rsMng.life);
                    break;
                default:
                    break;
            }
        }
    }
}