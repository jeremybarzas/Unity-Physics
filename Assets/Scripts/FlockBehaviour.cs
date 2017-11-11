using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class FlockBehaviour : MonoBehaviour
    {
        // fields
        public List<Boid> boidList = new List<Boid>();
        public float max_force = 2;
        public float cohesionScale = 1;
        public float dispersionScale = 1;        
        public float alignmentScale = 1;
        public float padding_distance = 2;
        public float neighbor_distance = 10;

        // methods
        public Vector3 Cohesion(Boid boid)
        {
            var force = Vector3.zero;
            var percievedCenter = Vector3.zero;

            foreach (var b in boid.neighbors)
            {
                if (b != boid)
                {
                    percievedCenter += b.Position;
                }                
            }           

            percievedCenter = percievedCenter / (boidList.Count - 1);

            force = (percievedCenter - boid.Position);
            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }

        public Vector3 Dispersion(Boid boid)
        {
            Vector3 force = Vector3.zero;

            foreach (var b in boid.neighbors)
            {
                if (b != boid)
                {
                    if ((b.Position - boid.Position).magnitude < padding_distance)
                    {
                        force = force - (b.Position - boid.Position);
                    }
                }
            }

            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }

        public Vector3 Alignment(Boid boid)
        {
            Vector3 force = Vector3.zero;
            var percievedVelocity = Vector3.zero;

            foreach (var b in boid.neighbors)
            {
                if (b != boid)
                {
                    percievedVelocity += b.Velocity;
                }
            }

            percievedVelocity = percievedVelocity / (boidList.Count - 1);

            force = (boid.Velocity - percievedVelocity);
            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }

        // Unity methods
        private void Start()
        {
            boidList = AgentFactory.Get_Boids();
        }

        private void Update()
        {
            foreach (var boid in boidList)
            {
                foreach (var b in boidList)
                {
                    if (b != boid)
                    {
                        if ((b.Position - boid.Position).magnitude < neighbor_distance)
                        {
                            if (!boid.neighbors.Contains(b))
                            {
                                boid.neighbors.Add(b);
                            }
                            
                        }
                        else
                        {
                            if (boid.neighbors.Contains(b))
                            {
                                boid.neighbors.Remove(b);
                            }
                        }
                    }
                }
            }

            foreach (var b in boidList)
            {
                var v1 = Cohesion(b);
                var v2 = Dispersion(b);
                var v3 = Alignment(b);
                
                b.Add_Force(cohesionScale, v1);
                b.Add_Force(dispersionScale, v2);
                b.Add_Force(alignmentScale, v3);                
            }            
        }
    }
}
