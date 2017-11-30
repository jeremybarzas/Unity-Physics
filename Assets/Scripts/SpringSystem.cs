﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    [System.Serializable]
    public class Particle
    {
        // fields
        public float mass;
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 acceleration;
        public Vector3 force;
        public bool isKinematic;

        // methods
        public Particle()
        {
            mass = 1;
            position = Vector3.zero;
            velocity = Vector3.zero;
            acceleration = Vector3.zero;
            force = Vector3.zero;
        }

        public Particle(float m, Vector3 p, Vector3 v)
        {
            mass = m;
            position = p;
            velocity = v;
        }

        public void Add_Force(Vector3 f)
        {
            force += f;
        }

        public Vector3 Update(float deltatime)
        {
            if (isKinematic)
                return position;

            acceleration = force / mass;
            velocity += acceleration * deltatime;

            position += velocity * deltatime;
            force = Vector3.zero;
            return position;
        }
    }

    [System.Serializable]
    public class SpringDamper
    {
        // fields        
        public Particle p1;        
        public Particle p2;

        public float k = 1;
        public Vector3 p1_start;
        public Vector3 p2_start;
        
        // methods
        public SpringDamper()
        {
            p1 = new Particle(1, p1_start * 2, Vector3.right);
            p2 = new Particle(1, p2_start * 2, Vector3.zero);

            p1_start = new Vector3(5, 0, 0);
            p2_start = new Vector3(-5, 0, 0);
        }

        public void Calculate_Force()
        {

        }

        public void Update()
        {            
            var p1_currentPos = p1.position;
            var p2_currentPos = p2.position;

            var p1_x = p1_currentPos - p1_start;
            var p2_x = p2_currentPos - p2_start;

            var p1_force = -k * p1_x;
            var p2_force = -k * p2_x;

            p1.Add_Force(p1_force);
            p2.Add_Force(p2_force);
        }
    }
}
