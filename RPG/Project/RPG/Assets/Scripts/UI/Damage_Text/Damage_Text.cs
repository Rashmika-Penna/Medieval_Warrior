using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.Damage_Text
{
    public class Damage_Text : MonoBehaviour
    {
        [SerializeField] Text damage_text = null;

        public void Set_Value(float amount)
        {
            damage_text.text = String.Format("{0:0}", amount);
        }
    }
}
