using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class BoidBehaviour : AgentBehaviour
    {
        public Boid Boid 
        {
            get { return (Boid)agent; }
            set { agent = Boid; }
        }

        public void LateUpdate()
        {
            transform.position = Boid.Update_Boid(Time.deltaTime);
        }

        public void Init(float s, float m, Vector3 v, Vector3 p)
        {
            Boid.Init(s, m, v, p);
        }

        public bool Apply_Forces(float mag, List<Vector3> forces)
        {
            Vector3 forceSummation = new Vector3(0,0,0);

            foreach (Vector3 force in forces)
            {
                forceSummation += force;
            }

            return Boid.Add_Force_Boid(2, forceSummation);
        }
    }
}
