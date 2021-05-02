using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
