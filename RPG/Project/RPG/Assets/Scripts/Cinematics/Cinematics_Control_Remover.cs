using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    public class Cinematics_Control_Remover : MonoBehaviour
    {
        GameObject player;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += Disable_Control;
            GetComponent<PlayableDirector>().stopped += Enable_Control;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= Disable_Control;
            GetComponent<PlayableDirector>().stopped -= Enable_Control;
        }

        private void Disable_Control(PlayableDirector pd)
        {
            player.GetComponent<Action_Scheduler>().Cancel_Current_Action();
            player.GetComponent<Player_Controller>().enabled = false;
        }

        private void Enable_Control(PlayableDirector pd)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<Player_Controller>().enabled = true;
        }
    }
}