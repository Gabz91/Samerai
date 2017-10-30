using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class NPC : Character
    {
        int Level { get; set; }
        public int EXP { get; set; }
        public int SoulValue { get; set; }
        float speed;
        public float Speed { get { return speed; } }

        /// <summary>
        /// The class of the NPC. Skills and stats depends on this field.
        /// </summary>
        public NPCType type;

        /// <summary>
        /// The attack range of the unit.
        /// Determines the distance from which the unit is able to perform an attack.
        /// </summary>
        public float attackRange;

        public NPC(NPCType type,int stageLvl)
        {
            Level = stageLvl;
            this.type = type;
            attack = 1;
            health = 5 * Level;
            currentHealth = health;
            switch (type)
            {
                case NPCType.MELEE_UNIT:
                    speed = 0.8f + 0.1f * (Level - 1);
                    attackRange = 0.95f;
                    break;
                case NPCType.RANGED_UNIT:
                    speed = 0.5f + 0.1f * (Level - 1);
                    attackRange = 2.95f;
                    break;
                default:
                    break;
            }
            speed = 0.8f + 0.1f * (Level - 1);
            CalculaterReward();
        }
        
        public virtual void CalculaterReward()
        {
            EXP = (Level - 1) * 2 + 1;
            SoulValue = Level / 2 + 1;
        }        
    }

    public enum NPCType
    {
        MELEE_UNIT,
        RANGED_UNIT,
        MAGIC_UNIT
    }
}
