using UnityEngine.Events;
using UnityEngine;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent on_hit;

         public void On_Hit()
        {
            on_hit.Invoke();
        }
    }
}