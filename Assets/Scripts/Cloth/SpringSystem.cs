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

        public Particle(float m, Vector3 p) : base()
        {
            mass = m;
            position = p;
        }

        public void Add_Force(Vector3 f)
        {
            if (isKinematic)
                return;
            force += f;
        }

        public Vector3 Update_Data(float deltatime)
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
            lo = 2;
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
            e = e / l;

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

        public void DrawLine()
        {
            Debug.DrawLine(p1.position, p2.position);
        }
    }

    [System.Serializable]
    public class AeroTriangle
    {
        // fields
        public Particle r1;
        public Particle r2;
        public Particle r3;
        public float density;
        public float drag;

        // methods
        public void Calculate_Force()
        {
            Vector3 n = Vector3.Cross((r2.position - r1.position), (r3.position - r1.position));

            float nmag = Vector3.Cross((r2.position - r1.position), (r3.position - r1.position)).magnitude;

            Vector3 n_normal = n / nmag;

            float ao = 0.5f * Vector3.Cross((r2.position - r1.position), (r3.position - r1.position)).magnitude;

            Vector3 vsurface = (r1.velocity + r2.velocity + r3.velocity) / 3.0f;

            Vector3 v = vsurface * density;

            float a = ao * Vector3.Dot(v, n_normal) / v.magnitude;

            Vector3 faero = -0.5f * density * (v.magnitude * v.magnitude) * drag * a * n_normal;

            faero = faero / 3.0f;

            r1.Add_Force(faero);
            r2.Add_Force(faero);
            r3.Add_Force(faero);
        }
    }

    [System.Serializable]
    public class ClothSystem
    {
        // fields
        public List<Particle> particles;
        public List<SpringDamper> springs;
        public List<AeroTriangle> triangles;
        public Vector3 gravity;

        // methods
        public ClothSystem()
        {
            particles = new List<Particle>();
            springs = new List<SpringDamper>();
            triangles = new List<AeroTriangle>();
            gravity = new Vector3(0, -9.81f, 0);
        }

        public ClothSystem(List<Particle> p, List<SpringDamper> s, List<AeroTriangle> t, Vector3 g)
        {
            particles = p;
            springs = s;
            triangles = t;
            gravity = g;
        }

        public void Create_Cloth(int width, int length, float padding)
        {
            // create particles
            int pCount = width * length;
            while (particles.Count < pCount)
            {
                Vector3 pos = Vector3.zero;
                Particle p = new Particle(1, pos);
                p.Set_Kinematic(true);
                particles.Add(p);
            }

            // set particle position
            int vertIndex = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var pos = new Vector3(j, -i, 0);
                    pos *= padding;
                    particles[vertIndex].position = pos; ;
                    vertIndex++;
                }
            }

            // make 4 corners static
            particles[0].Set_Kinematic(true);
            particles[width].Set_Kinematic(true);
            particles[length].Set_Kinematic(true);
            particles[particles.Count - 1].Set_Kinematic(true);
            int n = width;
            int counter = 0;
            for (int i = 0; i < particles.Count; i++)
            {                
                if (counter < n-1)
                {
                    var sd = new SpringDamper(particles[i], particles[i + 1], 5, 5);
                    springs.Add(sd);
                    counter++;
                }
                else
                    counter = 0;


            }

            //// vertical
            //for (int i = 0, j = 0; j < length - 1;)
            //{
            //    if (i == length - 1)
            //    {
            //        j += length;
            //        SpringDamper sd = new SpringDamper(particles[j], particles[j + 1], 5, 5);
            //        springs.Add(sd);
            //        j++;
            //        i = 0;
            //    }
            //    else
            //    {
            //        SpringDamper sd = new SpringDamper(particles[i], particles[i + 1], 5, 5);
            //        springs.Add(sd);
            //        i++;
            //    }
            //}
        }

        public void Update_Data()
        {
            // calculate forces
            foreach (Particle p in particles)
            {
                p.Add_Force(gravity);
            }

            foreach (SpringDamper s in springs)
            {
                s.Calculate_Force();
                s.DrawLine();
            }

            //foreach (AeroTriangle t in triangles)
            //{
            //    t.Calculate_Force();
            //}

            // Euler integration of movement
            foreach (Particle p in particles)
            {
                p.Update_Data(Time.deltaTime);
            }
        }
    }
}
