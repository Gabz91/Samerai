using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class Collectible : MonoBehaviour
    {
        public int id;
        public CollectibleType type;
        Transform collectiblesPool;
        public int value;
    }

    public enum CollectibleType
    {
        ITEM,
        WEAPON,
        SOUL
    }
}
