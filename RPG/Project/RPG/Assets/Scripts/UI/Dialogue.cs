using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    [CreateAssetMenu(fileName = "New Message", menuName = "Message")]
    public class Dialogue : ScriptableObject
    {
        [TextArea(3,10)]
        public string[] dialogues;
    }
}