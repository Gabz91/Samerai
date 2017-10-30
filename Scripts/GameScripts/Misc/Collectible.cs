using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class Collectible : MonoBehaviour
    {
        Item item;
        Transform collectiblesPool;
        public int value;
        int Value { get { return value; } }
    }
}
