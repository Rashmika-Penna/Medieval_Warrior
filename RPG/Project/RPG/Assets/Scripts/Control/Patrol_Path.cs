using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class Patrol_Path : MonoBehaviour
    {
        const float waypoint_gizmo_radius = 0.3f;
              
        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                int j = Get_Next_Waypoint(i);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(Get_Waypoint(i), waypoint_gizmo_radius);
                Gizmos.DrawLine(Get_Waypoint(i), Get_Waypoint(j));
            }
        }

        public int Get_Next_Waypoint(int i)
        {
            if(i+1 == transform.childCount)
            {
                return 0;
            }

            return i + 1;
        }

        public Vector3 Get_Waypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}