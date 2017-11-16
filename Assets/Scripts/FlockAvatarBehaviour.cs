using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class FlockAvatarBehaviour : MonoBehaviour
    {
        public List<Boid> boids;

        // Unity methods
        private void LateUpdate()
        {
            boids = GetComponentInParent<FlockBehaviour>().boidList;
            transform.forward = Get_Forward();
        }

        // methods
        public Vector3 Get_Forward()
        {
            var forward = Vector3.zero;
             
            foreach (var b in boids)
            {
                forward += b.Velocity;
            }

            forward = forward / boids.Count;
            return forward.normalized;
        }
    }
}
