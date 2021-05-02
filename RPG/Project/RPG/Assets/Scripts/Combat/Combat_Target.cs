using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]

    public class Combat_Target : MonoBehaviour, IRaycastable
    {
        public Cursor_Type Get_Cursor_Type()
        {
            return Cursor_Type.Combat;
        }

        public bool Handle_Raycast(Player_Controller call_controller)
        {
            if (!call_controller.GetComponent<Fight>().Can_Attack(gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                call_controller.GetComponent<Fight>().Attack(gameObject);
            }

            return true;
        }
    }
}