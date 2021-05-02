using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class Dialogue_Trigger_1 : MonoBehaviour
    {
        public Dialogue dialogue;

        public void Trigger_Dialogue()
        {
            FindObjectOfType<Dialogue_Manager>().Start_Dialogue(dialogue);
        }
    }
}