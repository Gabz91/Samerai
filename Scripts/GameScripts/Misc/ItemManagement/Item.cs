using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    [System.Serializable]
    public class Item
    {
        [SerializeField]
        protected string name;
        public string Name { get { return name; } }
        protected string description;
        public string Description { get { return description; } }
        protected string slug;
        public string Slug { get { return slug; } }
        protected int power;
        public int Power { get { return power; } }
        protected int defense;
        public int Defense { get { return defense; } }

        public ItemType type;
        [SerializeField]
        public int id;

        public Item(string name, string slug, string description, int power, int defense)
        {
            this.name = name;
            this.description = description;
            this.slug = slug;
            this.power = power;
            this.power = power;
        }
    }

    public enum ItemType
    {
        ITEM,
        WEAPON,
    }
}