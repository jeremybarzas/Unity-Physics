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
        public Particle(float m, Vector3 p)
        {
            mass = m;
            position = p;
            velocity = new Vector3(0, 0, -2f);
            acceleration = Vector3.zero;
            force = Vector3.zero;
            isKinematic = false;
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
        public SpringDamper(Particle part1, Particle part2, float tightness, float dampingFactor)
        {
            p1 = part1;
            p2 = part2;
            ks = tightness;
            kd = dampingFactor;
            lo = Vector3.Distance(p1.position, p2.position);
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
        public float density = 1;
        public float drag = 1;

        // methods
        public AeroTriangle(Particle p1, Particle p2, Particle p3, float _density, float _drag)
        {
            r1 = p1;
            r2 = p2;
            r3 = p3;
            density = _density;
            drag = _drag;
        }

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

            Vector3 aeroForce = faero / 3.0f;

            r1.Add_Force(aeroForce);
            r2.Add_Force(aeroForce);
            r3.Add_Force(aeroForce);
        }
    }

    [System.Serializable]
    public class ClothSystem
    {
        // fields
        public List<Particle> particles;
        public List<SpringDamper> springs;
        public List<SpringDamper> bendingSprings;
        public List<AeroTriangle> triangles;
        public Vector3 gravity;

        // methods
        public ClothSystem(int width, int length, float padding, float t, float d, float adens, float adrag)
        {
            particles = new List<Particle>();
            springs = new List<SpringDamper>();
            bendingSprings = new List<SpringDamper>();
            triangles = new List<AeroTriangle>();
            gravity = new Vector3(0, -9.81f, 0);

            // create particles with position
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    var pos = new Vector3(j, -i, 0);
                    pos *= padding;
                    Particle p = new Particle(1, pos);
                    particles.Add(p);
                }
            }

            // make spring dampers horizontal and vertical
            for (int i = 0; i < (length * width); i++)
            {
                if (i % (width) != width - 1)
                {
                    SpringDamper sdRight = new SpringDamper(particles[i], particles[i + 1], t, d);
                    springs.Add(sdRight);
                }
                if (i < (particles.Count - length))
                {
                    SpringDamper sdDown = new SpringDamper(particles[i], particles[i + length], t, d);
                    springs.Add(sdDown);
                }
            }

            // make spring dampers diagonal
            for (int i = 0; i <= ((width * length) - width) - 1; i++)
            {
                if (i % (width) != width - 1)
                {
                    SpringDamper diag = new SpringDamper(particles[i], particles[i + (width + 1)], t, d);
                    springs.Add(diag);
                }

                if (i % (width) > 0)
                {
                    SpringDamper diag = new SpringDamper(particles[i], particles[i + (width - 1)], t, d);
                    springs.Add(diag);
                }
            }

            // make triangles
            for (int i = 0; i < ((width * length) - width) - 1; i++)
            {
                if (i % (width) != width - 1)
                {
                    // t left, t right, b right
                    AeroTriangle tri = new AeroTriangle(particles[i], particles[i + 1], particles[i + (width + 1)], adens, adrag);
                    triangles.Add(tri);

                    // t left, b left, b right
                    tri = new AeroTriangle(particles[i], particles[i + width], particles[i + (width + 1)], adens, adrag);
                    triangles.Add(tri);
                }
            }

            // make bending springs
            for (int i = 0; i < (length * width) - 1; i++)
            {
                if (i % (width) <= width - ( width / 2))
                {
                    SpringDamper sdRight = new SpringDamper(particles[i], particles[i + 2], t, d);
                    bendingSprings.Add(sdRight);
                }
                if (i < (particles.Count - (length * 2)))
                {
                    SpringDamper sdDown = new SpringDamper(particles[i], particles[i + (length * 2)], t, d);
                    bendingSprings.Add(sdDown);
                }
            }

            /* ============ TESTING ============ */

            //// pin 4 corners in place
            //particles[0].Set_Kinematic(true);
            //particles[width - 1].Set_Kinematic(true);
            //particles[(width * length) - width].Set_Kinematic(true);
            //particles[(width * length) - 1].Set_Kinematic(true);

            // pin top row in place
            for (int i = 0; i < width; i++)
            {
                particles[i].Set_Kinematic(true);
            }

            //particles[0].Set_Kinematic(true);            
            //particles[width - 1].Set_Kinematic(true);

            //// pin side left in place
            //for (int i = 0; i < (width * length) - (width - 1);)
            //{
            //    particles[i].Set_Kinematic(true);
            //    i += width;
            //}

            /* ============ DEBUG INFO ============ */

            //foreach (SpringDamper s in springs)
            //{
            //    var currIndex = springs.IndexOf(s).ToString();
            //    var p1 = particles.IndexOf(s.p1).ToString();
            //    var p2 = particles.IndexOf(s.p2).ToString();
            //    string spring = "spring " + currIndex + ": p1 = " + p1 + ",  p2 = " + p2 + "\n";
            //    Debug.Log(spring);
            //}

            //foreach (SpringDamper s in bendingSprings)
            //{
            //    string currIndex = bendingSprings.IndexOf(s).ToString();
            //    var p1 = particles.IndexOf(s.p1).ToString();
            //    var p2 = particles.IndexOf(s.p2).ToString();
            //    string bspring = "bendingspring " + currIndex + ": p1 = " + p1 + ",  p2 = " + p2 + "\n";
            //    Debug.Log(bspring);
            //}

            foreach (AeroTriangle tri in triangles)
            {
                var currIndex = triangles.IndexOf(tri).ToString();
                var p1 = particles.IndexOf(tri.r1).ToString();
                var p2 = particles.IndexOf(tri.r2).ToString();
                var p3 = particles.IndexOf(tri.r3).ToString();
                string triangle = "triangle " + currIndex + ": p1 = " + p1 + ",  p2 = " + p2 + ",  p3 = " + p3 + "\n";
                Debug.Log(triangle);
            }
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

            foreach (SpringDamper bs in bendingSprings)
            {
                bs.Calculate_Force();
                bs.DrawLine();
            }

            foreach (AeroTriangle t in triangles)
            {
                t.Calculate_Force();
            }

            // Euler integration of movement
            foreach (Particle p in particles)
            {
                p.Update_Data(Time.deltaTime);                
            }
        }
    }
}
