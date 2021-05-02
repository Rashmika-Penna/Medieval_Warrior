using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Camera_Follow : MonoBehaviour
    {
        [SerializeField] Transform player = null;

        private void LateUpdate()
        {
            this.transform.position = player.position;
        }
    }
}
