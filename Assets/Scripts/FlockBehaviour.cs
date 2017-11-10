using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class FlockBehaviour : MonoBehaviour
    {
        // fields
        public List<Boid> boidList = new List<Boid>();
        public float cohesionScale = 1;
        public float dispersionScale = 1;        
        public float alignmentScale = 1;
        public float padding_distance = 2;
        public float max_force = 2;

        // methods
        public Vector3 Cohesion(Boid boid)
        {
            var force = Vector3.zero;            
            var positionSum = Vector3.zero;
            var percievedCenter = Vector3.zero;

            foreach (var b in boidList)
            {
                positionSum += b.Position;
            }           

            percievedCenter = positionSum / (boidList.Count - 1);

            force = percievedCenter - boid.Position;
            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }

        public Vector3 Dispersion(Boid boid)
        {
            Vector3 force = Vector3.zero;

            foreach (var b in boidList)
            {
                if ((boid.Position - b.Position).magnitude < padding_distance)
                {
                    force = force - (boid.Position - b.Position);
                }
            }

            force = Vector3.ClampMagnitude(force, -max_force);
            return force;
        }

        public Vector3 Alignment(Boid boid)
        {
            Vector3 force = Vector3.zero;
            var velocitySum = Vector3.zero;
            var percievedVelocity = Vector3.zero;

            foreach (var b in boidList)
            {
                velocitySum += b.Velocity;
            }

            percievedVelocity = velocitySum / (boidList.Count - 1);

            percievedVelocity = percievedVelocity - boid.Velocity;
            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }

        // Unity methods
        private void Start()
        {
            foreach (var agent in AgentFactory.Agents)
            {                
                boidList.Add((Boid)agent);
            }
        }

        private void Update()
        {
            var avgPos = Vector3.zero;
            foreach (var b in boidList)
            {
                var v1 = Cohesion(b);
                var v2 = Dispersion(b);
                var v3 = Alignment(b);
                
                b.Add_Force(cohesionScale, v1);
                b.Add_Force(dispersionScale, v2);
                b.Add_Force(alignmentScale, v3);

                avgPos += b.Position;
            }

            avgPos /= boidList.Count;

            transform.position = avgPos;
        }
    }
}
