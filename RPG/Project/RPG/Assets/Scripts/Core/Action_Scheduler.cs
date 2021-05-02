using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Action_Scheduler : MonoBehaviour
    {
        IAction current_action;

        public void Start_Action(IAction action)
        {
            if(current_action == action) { return; }

            if(current_action != null)
            {
                current_action.Cancel();
            }

            current_action = action;
        }

        public void Cancel_Current_Action()
        {
            Start_Action(null);
        }
    }
}