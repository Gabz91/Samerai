using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GenericScripts
{
    public class Utility : MonoSingleton<Utility>
    {

        public LayerMask whatIsGround;

        public void ToggleImage(Image sprite)
        {
            Color temp = sprite.color;
            if (temp.a != 0)
                temp.a = 0;
            else
                temp.a = 1;

            sprite.color = temp;
        }

        public bool IsGroundOnLeft(CharacterController characterCtrl)
        {
            Vector3 lineCastPosLeft = characterCtrl.bounds.center + new Vector3(0, 0, characterCtrl.bounds.extents.z);
            Debug.DrawLine(lineCastPosLeft, lineCastPosLeft + Vector3.down, Color.yellow);
            bool isGroundLeft = Physics.Linecast(lineCastPosLeft, lineCastPosLeft + Vector3.down * 2, whatIsGround);

            return isGroundLeft;
        }

        public bool IsGroundOnRight(CharacterController characterCtrl)
        {
            Vector3 lineCastPosRight = characterCtrl.bounds.center - new Vector3(0, 0, characterCtrl.bounds.extents.z);
            Debug.DrawLine(lineCastPosRight, lineCastPosRight + Vector3.down, Color.red);
            bool isGroundRight = Physics.Linecast(lineCastPosRight, lineCastPosRight + Vector3.down * 2, whatIsGround);

            return isGroundRight;
        }

        public bool IsGroundAhead(CharacterController characterCtrl)
        {
            Vector3 lineCastPosAhead = characterCtrl.bounds.center + new Vector3(characterCtrl.bounds.extents.x, 0, 0);
            Debug.DrawLine(lineCastPosAhead, lineCastPosAhead + Vector3.down, Color.magenta);
            bool isGroundAhead = Physics.Linecast(lineCastPosAhead, lineCastPosAhead + Vector3.down * 2, whatIsGround);

            return isGroundAhead;
        }

        public bool IsGroundBehind(CharacterController characterCtrl)
        {
            Vector3 lineCastPosBehind = characterCtrl.bounds.center - new Vector3(characterCtrl.bounds.extents.x, 0, 0);
            Debug.DrawLine(lineCastPosBehind, lineCastPosBehind + Vector3.down, Color.black);
            bool isGroundBehind = Physics.Linecast(lineCastPosBehind, lineCastPosBehind + Vector3.down * 2, whatIsGround);

            return isGroundBehind;
        }

        public bool IsGround(CharacterController characterCtrl)
        {
            Vector3 lineCastPosBehind = characterCtrl.bounds.center;
            Debug.DrawLine(lineCastPosBehind, lineCastPosBehind + Vector3.down, Color.black);
            bool isGroundBehind = Physics.Linecast(lineCastPosBehind, lineCastPosBehind + Vector3.down * 2, whatIsGround);

            return isGroundBehind;
        }
    }
}
