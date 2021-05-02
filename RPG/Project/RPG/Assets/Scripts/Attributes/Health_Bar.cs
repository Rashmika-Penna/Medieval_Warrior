using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class Health_Bar : MonoBehaviour
    {
        [SerializeField] Health health_component = null;
        [SerializeField] RectTransform foreground = null;
        [SerializeField] Canvas health_bar_canvas = null;

        private void Update()
        {
            if(Mathf.Approximately(health_component.Get_Health_Fraction(), 0) || Mathf.Approximately(health_component.Get_Health_Fraction(), 1))
            {
                health_bar_canvas.enabled = false;
                return;
            }

            health_bar_canvas.enabled = true;
            foreground.localScale = new Vector3(health_component.Get_Health_Fraction(), 1, 1);
        }
    }
}