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
        public bool isKinematic;

        // methods
        public Particle()
        {
            mass = 1;
            position = Vector3.zero;
            velocity = Vector3.zero;
            acceleration = Vector3.zero;
            force = Vector3.zero;
            isKinematic = false;
        }

        public Particle(float m, Vector3 p, Vector3 v, bool k)
        {
            mass = m;
            position = p;
            velocity = v;
            acceleration = Vector3.zero;
            force = Vector3.zero;
            isKinematic = k;
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

        public void Set_Kinematic(bool toggle)
        {   
            isKinematic = toggle;
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
        public float lo; // rest length displacment

        // methods
        public SpringDamper()
        {
            p1 = new Particle();
            p2 = new Particle();
            ks = 1;
            kd = 1;
        }

        public SpringDamper(Particle part1, Particle part2, float tightness, float dampingFactor)
        {
            p1 = part1;
            p2 = part2;
            ks = tightness;
            kd = dampingFactor;
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
    }

    public class AeroTriangle
    {
        // fields
        public Particle r1;
        public Particle r2;
        public Particle r3;
        float density;
        float drag;

        // methods
        public void Calculate_Force()
        {
            var n = Vector3.Cross((r2.position - r1.position), (r3.position - r1.position))
                    / Vector3.Cross((r2.position - r1.position), (r3.position - r1.position)).magnitude;
           
            var ao = 0.5f * Vector3.Cross((r2.position - r1.position), (r3.position - r1.position)).normalized;

            var vsurface = (r1.velocity + r2.velocity + r3.velocity) / 3;

            var v = vsurface * -density;

            var a = ao * Vector3.Dot(v, n) / v.magnitude;

            var an = Vector3.Cross(a,n).normalized;

            var faero = -0.5f * density * (v.magnitude * v.magnitude) * drag * an;

            faero /= 3;

            r1.Add_Force(faero);
            r2.Add_Force(faero);
            r3.Add_Force(faero);
        }
    }
}
