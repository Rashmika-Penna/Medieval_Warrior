using System.Collections; 
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Control;

namespace RPG.Scene_Management
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int scene_to_load = 0;
        [SerializeField] Transform spawn_point = null;
        [SerializeField] Destination_Identifier destination;
        [SerializeField] float fade_out_time = 1f;
        [SerializeField] float fade_in_time = 1f;
        [SerializeField] float fade_wait_time = 0.5f;

        enum Destination_Identifier
        {
            A, B, C, D, E
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if(scene_to_load < 0)
            {
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            Saving_Wrapper wrapper = FindObjectOfType<Saving_Wrapper>();

            // Removing Player control
            Player_Controller player_controller = GameObject.FindWithTag("Player").GetComponent<Player_Controller>();
            player_controller.enabled = false;

            yield return fader.Fade_Out(fade_out_time);

            // Save level            
            wrapper.Save_Game();

            yield return SceneManager.LoadSceneAsync(scene_to_load);

            // Finding Player in new scene
            // And disabling player control
            Player_Controller new_player_controller = GameObject.FindWithTag("Player").GetComponent<Player_Controller>();
            new_player_controller.enabled = false;

            // Load level
            wrapper.Load_Game();

            Portal other_portal = Get_Other_Portal();
            Update_Player(other_portal);

            wrapper.Save_Game(); 

            yield return new WaitForSeconds(fade_wait_time);
            fader.Fade_In(fade_in_time);

            // Restoring Player control
            new_player_controller.enabled = true;

            Destroy(gameObject);
        }

        private void Update_Player(Portal other_portal)
        {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.GetComponent<NavMeshAgent>().Warp(other_portal.spawn_point.position);
            player.transform.rotation = other_portal.spawn_point.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal Get_Other_Portal()
        {
            foreach(Portal portal in FindObjectsOfType<Portal>())
            {
                if(portal == this) { continue; }
                if(portal.destination != destination) { continue; }

                return portal;
            }

            return null;
        }
    }
}