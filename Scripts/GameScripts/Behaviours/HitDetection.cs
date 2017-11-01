using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScripts
{
    public class HitDetection : MonoBehaviour
    {
        PlayerCharacter player;
        NPC foe;
        Game game;

        void Start()
        {
            game = Game.Instance;
            player = game.player;
        }

        private void OnTriggerEnter(Collider other)
        {
            //The gameobject the script is attached to
            if (tag.Equals("Foe"))
            {
                if (other.gameObject.tag.Equals("PlayerWeapon"))
                {
                    foe = GetComponent<EnemyController>().foe;
                    foe.InflictDamage(player.Attack);

                    //Push the foe on hit
                    transform.position = Vector3.MoveTowards(transform.position, other.transform.position, -10 * Time.deltaTime);

                    if (!foe.IsAlive)
                        game.KillFoe(gameObject);
                }
                else if (other.gameObject.name.Equals("Fireball"))
                {
                    foe = GetComponent<EnemyController>().foe;
                    foe.InflictDamage(player.Attack);

                    if (!foe.IsAlive)
                        game.KillFoe(gameObject);
                }
            }
            else if (tag.Equals("Player"))
            {
                switch (other.tag)
                {
                    case "Collectible":
                        player.GainSoul(other.GetComponent<Collectible>().value);
                        game.CollectSoul(other.gameObject);
                        break;
                    case "EnemyWeapon":
                        game.HitThePlayer(other.transform.GetComponentInParent<EnemyController>().foe.Attack);
                        break;
                    default:
                        break;
                }
            }
            else if (tag.Equals("Projectile"))
            {
                switch (other.tag)
                {
                    case "Player":
                        gameObject.SetActive(false);
                        game.HitThePlayer(1);
                        break;
                    case "Environment":
                        StartCoroutine("KunaiImpact");
                        break;
                    case "Floor":
                        StartCoroutine("KunaiImpact");
                        break;
                    default:
                        break;
                }
            }
        }

        IEnumerator KunaiImpact()
        {
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Animator>().SetFloat("Speed", 0);
            yield return new WaitForSeconds(3);
            gameObject.SetActive(false);
        }
    }
}
