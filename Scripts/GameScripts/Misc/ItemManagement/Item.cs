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
        protected int value;
        public int Value { get { return value; } }

        public ItemType type;
        [SerializeField]
        public int id;

        public Item(string name, string slug, string description, int value)
        {
            this.name = name;
            this.description = description;
            this.slug = slug;
            this.value = value;
        }
    }

    public enum ItemType
    {
        ITEM,
        WEAPON,
    }
}