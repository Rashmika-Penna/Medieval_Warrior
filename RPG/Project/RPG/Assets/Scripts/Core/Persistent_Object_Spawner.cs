using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Persistent_Object_Spawner : MonoBehaviour
    {
        [SerializeField] GameObject persistent_obj_prefab;
        static bool has_spawned = false;

        private void Awake()
        {
            if(has_spawned) { return; }

            Spawn_Persistent_Objs();

            has_spawned = true;
        }

        private void Spawn_Persistent_Objs()
        {
            GameObject persistent_obj = Instantiate(persistent_obj_prefab);
            DontDestroyOnLoad(persistent_obj);
        }
    }
}