using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.UI;

public class Second_Dialogue : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<Dialogue_Trigger_4>().Trigger_Dialogue();
            Destroy(gameObject, 2f);
        }
    }
}
