using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class Weapon : Item
    {
        public Weapon(string name, string slug, string description, int power, int defense) : base (name, slug, description, power, defense)
        {
            this.name = name;
            this.description = description;
            this.slug = slug;
            this.power = power;
            this.defense = defense;
        }
    }
}