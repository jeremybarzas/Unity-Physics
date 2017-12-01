using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class ClothBehaviour : MonoBehaviour
    {  
        // fields
        public List<ParticleBehaviour> particles;
        public List<SpringDamper> springs;
        public List<AeroTriangle> triangles;
        public Vector3 gravity = new Vector3(0, -9.81f, 0);

        // methods        

        // Unity methods
        void Update()
        {
            // calculate forces
            foreach (ParticleBehaviour p in particles)
            {
                p.Particle.Add_Force(gravity);
            }

            foreach (SpringDamper s in springs)
            {
                s.Calculate_Force();
            }

            foreach (AeroTriangle t in triangles)
            {
                t.Calculate_Force();
            }

            // Euler integration of movement
            foreach (ParticleBehaviour p in particles)
            {
                p.Update_Particle();
            }
        }
    }
}
