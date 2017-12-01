using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class ClothBehaviour : MonoBehaviour
    {
        // fields
        public ClothSimulation cloth;
        public List<ParticleBehaviour> particles;

        // Unity methods
        private void Awake()
        {
            cloth = new ClothSimulation();
            particles = new List<ParticleBehaviour>();
        }

        private void Update()
        {
            Update_Cloth_Object();
        }

        // methods
        private void Update_Cloth_Object()
        {
            cloth.Update_Data();

            foreach (ParticleBehaviour p in particles)
            {
                p.Update_Particle();
            }
        }
    }
}
