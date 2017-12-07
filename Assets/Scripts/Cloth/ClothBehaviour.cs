using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class ClothBehaviour : MonoBehaviour
    {
        // fields
        public int width = 4;
        public int length = 4;
        public float padding = 10;
        public ClothSystem cloth;
        public float ks, kd;

        // Unity methods
        private void Awake()
        {
            cloth = new ClothSystem();
            cloth.Create_Cloth(width, length, padding);
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

        private void Update()
        {
            cloth.Update_Data();
        }        
    }
}
