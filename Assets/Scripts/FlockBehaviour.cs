using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class FlockBehaviour : MonoBehaviour
    {
        // fields
        public List<Boid> boidList = new List<Boid>();
        public float dMag;
        public float cMag;
        public float aMag;
        public float max_force;
        public float padding_distance;

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

            force = Vector3.ClampMagnitude(force, max_force);
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
            foreach (var b in boidList)
            {
                var v1 = Cohesion(b);
                var v2 = Dispersion(b);
                var v3 = Alignment(b);
                
                b.Add_Force(cMag, v1);
                b.Add_Force(dMag, v2);
                b.Add_Force(aMag, v3);
            }
        }
    }
}
