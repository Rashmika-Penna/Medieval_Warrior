using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stat
{
    public class XP_Display : MonoBehaviour
    {
        Experience xp;

        private void Awake()
        {
            xp = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", xp.Get_XP_Points());
        }
    }
}