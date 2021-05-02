using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;
using System;

namespace RPG.Control
{
    public class Message_Post_1 : MonoBehaviour, IRaycastable
    {        
        //public GameObject continue_button;
        //public Text text_display;
        //public string[] dialogues;
        //public float typing_speed;
        //private int index;

        //private void Update()
        //{
        //    if (text_display.text == dialogues[index])
        //    {
        //        continue_button.SetActive(true);
        //    }
        //}

        //IEnumerator Start()
        //{
        //    if(Input.GetMouseButtonDown(0))
        //    {
        //        foreach (char letter in dialogues[index].ToCharArray())
        //        {
        //            text_display.text += letter;
        //            yield return new WaitForSeconds(typing_speed);
        //        }
        //    }            
        //}

        //public void Next_Dialogue()
        //{
        //    continue_button.SetActive(false);

        //    if (index < dialogues.Length - 1)
        //    {
        //        index++;
        //        text_display.text = "";
        //        StartCoroutine(Start());
        //    }
        //    else
        //    {
        //        text_display.text = "";
        //        continue_button.SetActive(false);
        //    }
        //}

        public bool Handle_Raycast(Player_Controller call_controller)
        {
            if (Input.GetMouseButtonDown(0))
            {
                FindObjectOfType<Dialogue_Trigger_2>().Trigger_Dialogue();
            }

            return true;
        }

        public Cursor_Type Get_Cursor_Type()
        {
            return Cursor_Type.Message;
        }
    }
}