using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScripts
{
    public class HUD
    {
        GameObject characterStatsPanel;
        GameObject stagePanel;
        PlayerCharacter player;
        // Use this for initialization
        public HUD(GameObject csPanel, GameObject sPanel)
        {
            characterStatsPanel = csPanel;
            stagePanel = sPanel;
        }

        public void UpdateCharacterStats()
        {
            if (characterStatsPanel == null)
                characterStatsPanel = GameObject.Find("CharacterStatus");
            if (player == null)
                player = ResourceManager.Instance.player;
            characterStatsPanel.transform.Find("Level").GetComponent<Text>().text = "Level " + player.Level;
            characterStatsPanel.transform.Find("Attack").GetComponent<Text>().text = "Attack " + player.Attack;
            characterStatsPanel.transform.Find("Defense").GetComponent<Text>().text = "Defense " + player.Defense;
            characterStatsPanel.transform.Find("HealthText").GetComponent<Text>().text = "Health " + player.CurrentHealth + " I " + player.Health;
            characterStatsPanel.transform.Find("ExpText").GetComponent<Text>().text = "Exp " + player.Exp + " I " +player.ExpNeeded;
            characterStatsPanel.transform.Find("Health").GetComponent<Image>().fillAmount = (float)player.CurrentHealth / player.Health;
            characterStatsPanel.transform.Find("Exp").GetComponent<Image>().fillAmount = (float)player.Exp / player.ExpNeeded;
        }

        public void UpdateStageStats()
        {
            if (stagePanel == null)
                stagePanel = GameObject.Find("StagePanel");
            stagePanel.transform.Find("EnemiesKilled").GetComponent<Text>().text = "Enemies killed: " + Game.enemiesKilled + " I " + Game.enemiesToKill;
            stagePanel.transform.Find("StageNumber").GetComponent<Text>().text = "Stage " + Game.stage;
        }

        public void UpdatePlayer()
        {
            player = ResourceManager.Instance.player;
        }
    }
}