using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Scene_Management
{
    public class Saving_Wrapper : MonoBehaviour
    {
        [SerializeField] float fade_in_time = 0.2f;
        const string save_file = "save";

        private void Awake()
        {
            StartCoroutine(Load_Last_Scene());
        }

        private IEnumerator Load_Last_Scene()
        {            
            yield return GetComponent<SavingSystem>().LoadLastScene(save_file);
            Fader fader = FindObjectOfType<Fader>();
            fader.Immediate_Fade_Out();
            yield return fader.Fade_In(fade_in_time);
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Load_Game();

            }

            if(Input.GetKeyDown(KeyCode.S))
            {
                Save_Game();
            }

            if(Input.GetKeyDown(KeyCode.Delete))
            {
                Delete_Game();
            }
        }

        public void Save_Game()
        {
            GetComponent<SavingSystem>().Save(save_file);
        }

        public void Load_Game()
        {
            GetComponent<SavingSystem>().Load(save_file);
        }

        public void Delete_Game()
        {
            GetComponent<SavingSystem>().Delete(save_file);
        }
    }
}