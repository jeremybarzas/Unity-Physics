using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class ParticleBehaviour : MonoBehaviour
    {
        // fields        
        [SerializeField]
        Particle particle;

        // properties
        public Particle Particle
        {
            get
            {
                return particle;
            }

            set
            {
                particle = value;
            }
        }       

        // methods
        public void LateUpdate()
        {
            transform.position = particle.position;
        }
    }
}
