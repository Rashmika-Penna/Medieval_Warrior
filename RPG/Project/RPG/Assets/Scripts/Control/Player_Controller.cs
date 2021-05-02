using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using System;
using RPG.Movement; 
using RPG.Attributes;

namespace RPG.Control
{
    public class Player_Controller : MonoBehaviour
    {
        [System.Serializable]
        struct Cursor_Mapping
        {
            public Cursor_Type type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] Cursor_Mapping[] cursor_mappings = null;
        [SerializeField] float max_proj_distance = 1f;
        [SerializeField] float raycast_radius = 1f;

        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if(Interact_With_UI()) { return; }

            if(health.Is_Dead())
            {
                Set_Cursor(Cursor_Type.Dead);
                StartCoroutine(Game_Over());
                return;
            }

            if(Interact_With_Component()) { return; }

            if(Move()) { return; }

            Set_Cursor(Cursor_Type.None);
        }

        private bool Interact_With_UI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                Set_Cursor(Cursor_Type.UI);
                return true;
            }

            return false;
        }

        IEnumerator Game_Over()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Game_Over");
        }

        private bool Interact_With_Component()
        {
            RaycastHit[] hits = Sort_All_Raycasts();

            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                
                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.Handle_Raycast(this))
                    {
                        Set_Cursor(raycastable.Get_Cursor_Type());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] Sort_All_Raycasts()
        {
            RaycastHit[] hits = Physics.SphereCastAll(Get_Mouse_Ray(), raycast_radius);
            float[] distances = new float[hits.Length];

            for(int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }
 
        private bool Move()
        {            
            Vector3 target;
            bool has_hit = Raycast_NavMesh(out target);

            if(has_hit)
            {
                if(!GetComponent<Mover>().Can_Move_To(target))
                {
                    return false;
                }

                if(Input.GetMouseButton(0))
                {
                    this.GetComponent<Mover>().Move_Action(target, 1f);
                }

                Set_Cursor(Cursor_Type.Movement);

                return true;
            }
            return false;
        }

        private bool Raycast_NavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool has_hit = Physics.Raycast(Get_Mouse_Ray(), out hit);

            if(!has_hit) { return false; }

            NavMeshHit navmesh_hit;
            bool cast_to_navmesh = NavMesh.SamplePosition(hit.point, out navmesh_hit, max_proj_distance, NavMesh.AllAreas);
           
            if(!cast_to_navmesh) { return false; }

            target = navmesh_hit.position;            

            return true;
        }        

        private void Set_Cursor(Cursor_Type type)
        {
            Cursor_Mapping mapping = Get_Cursor_Mapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private Cursor_Mapping Get_Cursor_Mapping(Cursor_Type type)
        {
            foreach(Cursor_Mapping mapping in cursor_mappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }

            return cursor_mappings[0];
        }

        private static Ray Get_Mouse_Ray()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}