using UnityEngine;
using UnityEngine.UI;

public class Destroy_Text : MonoBehaviour
{
    [SerializeField] GameObject text_to_destroy = null;    

    public void Destroy_Damage_Text()
    {
        Destroy(text_to_destroy);
    }    
}
