using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class FlockBehaviour : MonoBehaviour
    {
        // fields        
        public float max_speed = 75;
        public float max_force = 20;
        public float alignmentScale = 5;
        private float cohesionScale = 20;
        private float dispersionScale = 40;
        private float dispersion_distance = 2;
        private float neighbor_distance = 10;
        public List<Boid> boidList = new List<Boid>();

        // properties
        public float Max_Speed
        {
            get { return Max_Speed; }
            set { Max_Speed = value; }
        }
        public float Max_Force
        {
            get
            {
                return max_force;
            }

            set
            {
                max_force = value;
            }
        }
        public float AlignmentScale
        {
            get
            {
                return alignmentScale;
            }

            set
            {
                alignmentScale = value;
            }
        }
        public float CohesionScale
        {
            get
            {
                return cohesionScale;
            }

            set
            {
                cohesionScale = value;
            }
        }
        public float DispersionScale
        {
            get
            {
                return dispersionScale;
            }

            set
            {
                dispersionScale = value;
            }
        }
        public float Dispersion_Distance
        {
            get
            {
                return dispersion_distance;
            }

            set
            {
                dispersion_distance = value;
            }
        }
        public float Neighbor_Distance
        {
            get
            {
                return neighbor_distance;
            }

            set
            {
                neighbor_distance = value;
            }
        }

        // methods
        public Vector3 Get_Center()
        {
            var center = Vector3.zero;

            foreach (var b in boidList)
            {
                center += b.Position;
            }

            center = center / boidList.Count;
            return center;
        }

        public Vector3 Cohesion(Boid boid)
        {
            var force = Vector3.zero;
            var percievedCenter = Vector3.zero;

            foreach (var b in boidList)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < Neighbor_Distance)
                    {
                        percievedCenter += b.Position;
                    }
                }
            }
            
            percievedCenter = percievedCenter / (boidList.Count - 1);

            force = (percievedCenter - boid.Position);
            force = Vector3.ClampMagnitude(force, Max_Force);
            return force;
        }

        public Vector3 Dispersion(Boid boid)
        {
            Vector3 force = Vector3.zero;

            foreach (var b in boidList)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < Neighbor_Distance)
                    {
                        if ((b.Position - boid.Position).magnitude < Dispersion_Distance)
                        {
                            force = force - (b.Position - boid.Position);
                        }
                    }
                }
            }

            force = Vector3.ClampMagnitude(force, Max_Force);
            return force;
        }

        public Vector3 Alignment(Boid boid)
        {
            Vector3 force = Vector3.zero;
            var percievedVelocity = Vector3.zero;

            foreach (var b in boidList)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < Neighbor_Distance)
                    {
                        percievedVelocity += b.Velocity;
                    }
                }
            }

            percievedVelocity = percievedVelocity / (boidList.Count - 1);

            force = (boid.Velocity - percievedVelocity);
            force = Vector3.ClampMagnitude(force, Max_Force);
            return force;
        }

        // Unity methods
        private void Start()
        {
            boidList = AgentFactory.Get_Boids();
        }

        private void Update()
        {
            foreach (var b in boidList)
            {                
                b.Max_Speed = Max_Speed;

                var v1 = Cohesion(b);
                var v2 = Dispersion(b);
                var v3 = Alignment(b);
                
                b.Add_Force(CohesionScale, v1);
                b.Add_Force(DispersionScale, v2);
                b.Add_Force(AlignmentScale, v3);                
            }

            transform.position = Get_Center();
        }
    }
}
