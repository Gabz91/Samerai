using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class Weapon : Item
    {
        int power;
        public int Power { get { return power; } }

        public Weapon(string name, string slug, string description, int power) : base (name, slug, description, power)
        {
            this.name = name;
            this.description = description;
            this.slug = slug;
            this.power = power;
        }

        public void ShowWeapon()
        {
            Debug.Log("Name: " + Name + " Description: " + Description + " Power: " + Power);
        }
    }
}