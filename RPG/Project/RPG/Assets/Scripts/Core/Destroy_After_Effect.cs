using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Destroy_After_Effect : MonoBehaviour
    {
        [SerializeField] GameObject target_destroy = null;

        private void Update()
        {
            if(!GetComponent<ParticleSystem>().IsAlive())
            {
                if(target_destroy != null)
                {
                    Destroy(target_destroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}