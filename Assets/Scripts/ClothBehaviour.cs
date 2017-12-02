using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class ClothBehaviour : MonoBehaviour
    {
        // fields
        public ClothSimulation cloth;
        public float ks, kd;

        // Unity methods
        private void Awake()
        {
            cloth = new ClothSimulation();
        }

        private void Start()
        {
            for (int i = 0; i < 25; ++i)
            {
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                var pb = go.AddComponent<ParticleBehaviour>();
                var pos = new Vector3(i % 5 * i, i % 5 * i, 0);
                cloth.particles.Add(new Particle(1, pos, Vector3.up, false));
                pb.Particle = cloth.particles[i];

            }

            for (int i = 0; i < 24; ++i)
            {                
                cloth.springs.Add(new SpringDamper(cloth.particles[i], cloth.particles[i + 1], 5, 5));
            }

            ////dotriangle
            //for (int i = 0; i < 25 * 3; i += 3)
            //{
            //    var bot = i * 5;
            //    var bot_right = bot + 1;
            //    var tri = new AeroTriangle()
            //    {
            //        r1 = cloth.particles[i],
            //        r2 = cloth.particles[bot],
            //        r3 = cloth.particles[bot_right],
            //        density = 2,
            //        drag = 2,
            //    };


            //    cloth.triangles.Add(tri);

            //}

            for (int i = 0; i < 5; i++)
                cloth.particles[i].Set_Kinematic(true);
        }
        private void Update()
        {
            cloth.Update_Data();
        }
    }
}
