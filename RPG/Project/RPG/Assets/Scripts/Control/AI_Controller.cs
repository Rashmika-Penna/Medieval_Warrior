using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;

namespace RPG.Control
{
    public class AI_Controller : MonoBehaviour
    {
        [SerializeField] float chase_distance = 3f;
        [SerializeField] float suspicion_time = 3f;
        [SerializeField] float anger_cool_down = 5f;
        [SerializeField] Patrol_Path patrol_path = null;
        [SerializeField] float waypoint_tolerance = 1f;
        [Range(0, 1)] [SerializeField] float patrol_speed_fraction = 0.2f;
        [SerializeField] float shout_out_distance = 5f;
        float waypoint_dwell_time;

        Fight fighter;
        Health health;
        Mover mover;
        GameObject player;
        LazyValue<Vector3> guard_position;

        float time_since_player_was_seen = Mathf.Infinity;
        float time_since_arrived_at_waypoint = Mathf.Infinity;
        float time_since_angered = Mathf.Infinity;
        int current_waypoint_index = 0;

        private void Awake()
        {
            fighter = GetComponent<Fight>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guard_position = new LazyValue<Vector3>(Get_Guard_Position);
        }

        private Vector3 Get_Guard_Position()
        {
            return transform.position;
        }

        private void Start()
        {
            guard_position.ForceInit();
        }

        private void Update()
        {
            if (health.Is_Dead()) { return; }

            if (Is_Angered() && fighter.Can_Attack(player))
            {
                Attack_Behaviour();
            }
            else if (time_since_player_was_seen < suspicion_time)
            {
                // Suspicion
                Suspicion_Behaviour();
            }
            else
            {
                Patrol();
            }

            Update_Timers();
        }

        public void Aggrevate()
        {
            time_since_angered = 0;
        }

        private void Update_Timers()
        {
            time_since_player_was_seen += Time.deltaTime;
            time_since_arrived_at_waypoint += Time.deltaTime;
            time_since_angered += Time.deltaTime;
        }

        private void Patrol()
        {
            Vector3 next_position = guard_position.value;


            if (patrol_path != null)
            {
                if (At_Waypoint())
                {
                    time_since_arrived_at_waypoint = 0;
                    waypoint_dwell_time = Random.Range(1f, 5f);
                    Cycle_Waypoint();
                }

                next_position = Get_Current_Waypoint();
            }

            if (time_since_arrived_at_waypoint > waypoint_dwell_time)
            {
                mover.Move_Action(next_position, patrol_speed_fraction);
            }
        }

        private bool At_Waypoint()
        {
            float distance_to_waypoint = Vector3.Distance(transform.position, Get_Current_Waypoint());
            return distance_to_waypoint < waypoint_tolerance;
        }

        private void Cycle_Waypoint()
        {
            current_waypoint_index = patrol_path.Get_Next_Waypoint(current_waypoint_index);
        }

        private Vector3 Get_Current_Waypoint()
        {
            return patrol_path.Get_Waypoint(current_waypoint_index);
        }

        private void Suspicion_Behaviour()
        {
            GetComponent<Action_Scheduler>().Cancel_Current_Action();
        }

        private void Attack_Behaviour()
        {
            time_since_player_was_seen = 0;
            fighter.Attack(player);

            Anger_Nearby_Enemies();
        }

        private void Anger_Nearby_Enemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shout_out_distance, Vector3.up, 0);

            foreach(RaycastHit hit in hits)
            {
                AI_Controller ai = hit.collider.GetComponent<AI_Controller>();
                
                if(ai == null) { continue; }

                ai.Aggrevate();
            }
        }

        private bool Is_Angered()
        {
            float distance_to_player = Vector3.Distance(player.transform.position, transform.position);
            return (distance_to_player <= chase_distance) || (time_since_angered < anger_cool_down);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chase_distance);
        }
    }
}