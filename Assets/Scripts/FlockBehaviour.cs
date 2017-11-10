using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class FlockBehaviour : MonoBehaviour
    {
        // fields
        public List<Boid> boidList = new List<Boid>();
        public float dMag = 1;
        public float cMag = 1;
        public float aMag = 1;
        public float max_force = 5;

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

            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }        

        public Vector3 Alignment(Boid boid)
        {
            Vector3 force = Vector3.zero;

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
