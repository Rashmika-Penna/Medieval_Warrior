﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.UI;

namespace RPG.Cinematics
{
    public class Cinematic_Trigger : MonoBehaviour
    {
        bool already_triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if(!already_triggered && other.gameObject.tag == "Player")
            {
                already_triggered = true;
                GetComponent<PlayableDirector>().Play();
                FindObjectOfType<Dialogue_Trigger_4>().Trigger_Dialogue();
            }
        }
    }
}