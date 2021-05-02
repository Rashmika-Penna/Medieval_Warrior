using System;
using UnityEngine;
using UnityEngine.UI;
using RPG.Attributes;

namespace RPG.Combat
{
    public class Enemy_Health_Display : MonoBehaviour
    {
        Fight fighter;

        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fight>();
        }

        private void Update()
        {
            if(fighter.Get_Target() == null)
            {
                GetComponent<Text>().text = "***";
                return;
            }

            Health health = fighter.Get_Target();
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.Get_Health_Points(), health.Get_Max_Health_Points());
        }
    }
}