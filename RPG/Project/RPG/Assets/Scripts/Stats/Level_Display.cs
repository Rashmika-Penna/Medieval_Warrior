using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stat
{
    public class Level_Display : MonoBehaviour
    {
        Base_Stats base_stats;

        private void Awake()
        {
            base_stats = GameObject.FindWithTag("Player").GetComponent<Base_Stats>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", base_stats.Get_Level());
        }
    }
}