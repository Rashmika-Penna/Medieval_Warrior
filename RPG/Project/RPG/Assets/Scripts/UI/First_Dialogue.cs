using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;

public class First_Dialogue : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<Dialogue_Trigger_3>().Trigger_Dialogue();
            Destroy(gameObject, 2f);
        }
    }
}
