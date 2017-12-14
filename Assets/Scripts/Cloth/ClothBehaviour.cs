using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class ClothBehaviour : MonoBehaviour
    {
        // fields
        public int width = 10;
        public int length = 10;
        public float padding = 10;
        public float tightness = 1;
        public float dampingFactor = 1;

        public ClothSystem cloth;

        // Unity methods
        private void Awake()
        {
            cloth = new ClothSystem(width, length, padding, tightness, dampingFactor);            
        }

        private void Start()
        {            
            int counter = 0;
            foreach (Particle p in cloth.particles)
            {                
                var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                go.name = string.Format("Particle::{0}", counter);
                var pb = go.AddComponent<ParticleBehaviour>();
                pb.Particle = p;
                counter++;
            }
        }

        private void FixedUpdate()
        {
            cloth.Update_Data();
        }        
    }
}
