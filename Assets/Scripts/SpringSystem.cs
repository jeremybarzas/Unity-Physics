using System.Collections;
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

        //public bool isKinematic;

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
            acceleration = Vector3.zero;
            force = Vector3.zero;
        }

        public void Add_Force(Vector3 f)
        {
            force += f;
        }

        public Vector3 Update(float deltatime)
        {
            //if (isKinematic)
                //return position;

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

        public float ks; // constant tightness
        public float kd; // damping coefficient
        public float lo; // rest position displacment
        
        // methods
        public SpringDamper()
        {
            var p1_start = new Vector3(-5, 0, 0);
            var p2_start = new Vector3(5, 0, 0);

            p1 = new Particle(1, p1_start * 2, Vector3.right);
            p2 = new Particle(1, p2_start * 2, Vector3.left);           
        }

        public void Calculate_Force()
        {
            // calculate unit length vector
            var e = p2.position - p1.position;
            var l = e.magnitude;
            e = e.normalized / l;

            // calculate 1D vectors
            var v1 = Vector3.Dot(e, p1.velocity);
            var v2 = Vector3.Dot(e, p2.velocity);

            // convert 1D to 3D vector
            var fsd = -ks * (lo - l) - kd * (v1 - v2);
            var f1 = fsd * e;
            var f2 = -f1;

            p1.Add_Force(f1);
            p2.Add_Force(f2);
        }

        public void Update()
        {
            p1.Update(Time.deltaTime);
            p1.Update(Time.deltaTime);
        }
    }
}
