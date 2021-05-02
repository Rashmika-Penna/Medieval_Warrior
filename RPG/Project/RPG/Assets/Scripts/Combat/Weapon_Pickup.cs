using System.Collections;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Weapon_Pickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] Weapon_Config weapon = null;
        [SerializeField] float restore_health = 0;
        [SerializeField] float respawn_time = 2f;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                Pickup(other.gameObject);                
            }
        }

        public void Pickup(GameObject subject)
        {
            if(weapon != null)
            {
                subject.GetComponent<Fight>().Equip_Weapon(weapon);
            }

            if(restore_health > 0)
            {
                subject.GetComponent<Health>().Heal(restore_health);
            }

            StartCoroutine(Respawn_Pickups(respawn_time));
        }

        private IEnumerator Respawn_Pickups(float time)
        {
            Show_Pickup(false);
            yield return new WaitForSeconds(time);
            Show_Pickup(true);
        }

        private void Show_Pickup(bool should_show)
        {
            GetComponent<Collider>().enabled = should_show;
            
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(should_show);
            }
        }

        public bool Handle_Raycast(Player_Controller call_controller)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Pickup(call_controller.gameObject);
            }

            return true;
        }

        public Cursor_Type Get_Cursor_Type()
        {
            return Cursor_Type.Pickup;
        }
    }
}