using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class Dialogue_Manager : MonoBehaviour
    {
        public GameObject dialogue_box;
        public Text message_text;
        private Queue<string> dialogues;

        private void Start()
        {
            dialogues = new Queue<string>();
        }

        public void Start_Dialogue(Dialogue dialogue)
        {
            dialogue_box.SetActive(true);
            dialogues.Clear();

            foreach(string message in dialogue.dialogues)
            {
                dialogues.Enqueue(message);
            }

            Display_Next_Dialogue();
        }

        public void Display_Next_Dialogue()
        {            
            if(dialogues.Count == 0)
            {
                End_Message();
                return;
            }

            string message = dialogues.Dequeue();
            StopAllCoroutines();
            StartCoroutine(Type(message));
        }

        IEnumerator Type(string message)
        {
            message_text.text = "";

            foreach(char letter in message.ToCharArray())
            {
                message_text.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void End_Message()
        {
            dialogue_box.SetActive(false);
        }
    }
}