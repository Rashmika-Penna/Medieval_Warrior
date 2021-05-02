using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float max_path_length = 40f;
        [SerializeField] float max_speed = 6f;

        NavMeshAgent nav_mesh_agent;
        Health health;

        private void Awake()
        {
            nav_mesh_agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            nav_mesh_agent.enabled = !health.Is_Dead();
            Animation();
        }

        public void Move_Action(Vector3 destination, float speed_fraction)
        {
            GetComponent<Action_Scheduler>().Start_Action(this);
            Move_To(destination, speed_fraction);
        }

        public bool Can_Move_To(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool has_path = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);

            if (!has_path) { return false; }

            if (path.status != NavMeshPathStatus.PathComplete) { return false; }

            if (Get_Path_Length(path) > max_path_length) { return false; }

            return true;
        }

        public void Move_To(Vector3 destination, float speed_fraction)
        {
            nav_mesh_agent.destination = destination;
            nav_mesh_agent.speed = max_speed * Mathf.Clamp01(speed_fraction);
            nav_mesh_agent.isStopped = false;
        }

        private void Animation()
        {
            Vector3 velocity = nav_mesh_agent.velocity;
            Vector3 local_velocity = transform.InverseTransformDirection(velocity);
            float speed = local_velocity.z;
            GetComponent<Animator>().SetFloat("Forward_Speed", speed);
        }

        private float Get_Path_Length(NavMeshPath path)
        {
            float total = 0;

            if (path.corners.Length < 2) { return total; }

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }

        public void Cancel()
        {
            nav_mesh_agent.isStopped = true;
        }

        // Using Struct 
        [System.Serializable]
        struct Mover_Save_Data
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            // struct
            Mover_Save_Data data = new Mover_Save_Data();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;

            //// Dictionary
            //Dictionary<string, object> data = new Dictionary<string, object>();
            //data["position"] = new SerializableVector3(transform.position);
            //data["rotation"] = new SerializableVector3(transform.eulerAngles);
            //return data;
        }

        public void RestoreState(object state)
        {
            // Struct
            Mover_Save_Data data = (Mover_Save_Data)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;

            //// Dictionary
            //Dictionary<string, object> data = (Dictionary<string, object>)state;
            //GetComponent<NavMeshAgent>().enabled = false;
            //transform.position = ((SerializableVector3)data["position"]).ToVector();
            //transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            //GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}
