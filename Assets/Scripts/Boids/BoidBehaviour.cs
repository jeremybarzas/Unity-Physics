using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class BoidBehaviour : AgentBehaviour
    {
        // fields
        public Boid boid;

        // Unity methods
        public void LateUpdate()
        {
            transform.position = boid.Update_Agent(Time.deltaTime);
            transform.forward = agent.Velocity.normalized / Time.deltaTime;
        }

        // methods
        public void Set_Boid(Boid b)
        {            
            agent = b;
            boid = b;
        }
    }
}
