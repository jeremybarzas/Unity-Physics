using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class FlockAvatarBehaviour : MonoBehaviour
    {
        // fields
        public FlockBehaviour parent;

        // Unity methods
        private void Start()
        {
            parent = GetComponentInParent<FlockBehaviour>();
        }

        private void LateUpdate()
        {            
            transform.forward = parent.flockForward.normalized;
        }
    }
}
