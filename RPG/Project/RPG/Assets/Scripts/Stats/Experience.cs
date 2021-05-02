using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stat
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float exp_points = 0;

        public event Action On_XP_Gained;

        public void Gain_XP(float exp)
        {
            exp_points += exp;
            On_XP_Gained();
        }

        public float Get_XP_Points()
        {
            return exp_points;
        }

        public object CaptureState()
        {
            return exp_points;
        }

        public void RestoreState(object state)
        {
            exp_points = (float)state;
        }
    }
}