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

        // methods
        public Vector3 Cohesion(Boid boid)
        {
            Vector3 force = Vector3.zero;            
            Vector3 positionSum = Vector3.zero;
            Vector3 percievedCenter = Vector3.zero;

            foreach (var b in boidList)
            {
                positionSum += b.Position;
            }           

            percievedCenter = positionSum / (boidList.Count - 1);

            force = percievedCenter - boid.Position;

            return force / force.magnitude;
        }

        public Vector3 Dispersion(Boid boid)
        {
            Vector3 force = Vector3.zero;


            return force / force.magnitude;
        }        

        public Vector3 Alignment(Boid boid)
        {
            Vector3 force = Vector3.zero;


            return force / force.magnitude;
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
            Vector3 v1, v2, v3;

            foreach (var b in boidList)
            {
                v1 = Cohesion(b);
                b.Add_Force(cMag, v1);

                v2 = Dispersion(b);
                b.Add_Force(dMag, v2);

                v3 = Alignment(b);
                b.Add_Force(aMag, v3);
            }
        }
    }
}
