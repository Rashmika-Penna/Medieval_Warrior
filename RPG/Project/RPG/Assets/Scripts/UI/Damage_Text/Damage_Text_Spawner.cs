using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

namespace RPG.UI.Damage_Text
{
    public class Damage_Text_Spawner : MonoBehaviour
    {
        [SerializeField] Damage_Text damage_text = null;
        
        public void Spawn(float damage)
        {
            Damage_Text instance = Instantiate(damage_text, transform);
            instance.Set_Value(damage);
        }
    }
}
