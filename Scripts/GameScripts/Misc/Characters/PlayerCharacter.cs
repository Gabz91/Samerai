﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    [System.Serializable]
    public class PlayerCharacter : Character {

        //public Attack[] attackSlots;
        //Stats fields
        [SerializeField]
        int level;
        public int Level { get { return level; } }
        [SerializeField]
        int expNeeded;
        public int ExpNeeded { get { return expNeeded; } }
        [SerializeField]
        int exp;
        public int Exp { get { return exp; } }

        public PlayerCharacter()
        {
            level = 1;
            exp = 0;
            expNeeded = (int)(1.25 * (Level + 1 ^ 3));

            health = 10;
            currentHealth = 10;
            attack = 1;
            defense = 1;
        }

        public void GainExp(int expGained)
        {
            this.exp += expGained;
            if (Exp >= expNeeded)
                LevelUp();
        }

        void LevelUp()
        {
            level++;
            expNeeded = (int)(1.25 * (Mathf.Pow(Level, 3)));
            attack++;
            health += 5;
        }

        public override void InflictDamage(int damage)
        {
            damage = damage - defense;
            if (damage <= 0)
                damage = 1;
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Debug.LogError("You were defeated");
                IsAlive = false;
                Time.timeScale = 0;
            }
        }

        public void GainSoul(int soulValue)
        {
            if (CurrentHealth < Health)
                currentHealth += soulValue;
            if (CurrentHealth > Health)
                currentHealth = Health;
        }

        public void AddEquipStats(Item item)
        {
            attack += ((Weapon)item).Power;
            defense += ((Weapon)item).Defense;
        }

        public void RemoveEquipStats(Item item)
        {
            attack -= ((Weapon)item).Power;
            defense -= ((Weapon)item).Defense;
        }
    }
}
