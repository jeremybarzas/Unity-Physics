using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class SpringBehaviour : MonoBehaviour
    {
        // fields
        [SerializeField]
        Spring spring;

        // methods
        void Start()
        {
            spring = new Spring();
            var go0 = GameObject.CreatePrimitive(0);
            var go1 = GameObject.CreatePrimitive(0);
            var pb0 = go0.AddComponent<ParticleBehaviour>();
            var pb1 = go1.AddComponent<ParticleBehaviour>();

            pb0.Particle = spring.p1;
            pb1.Particle = spring.p2;
        }

        void Update()
        {
            spring.Update(Time.deltaTime);
        }
    }
}
