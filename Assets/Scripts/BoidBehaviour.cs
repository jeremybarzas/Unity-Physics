using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class BoidBehaviour : AgentBehaviour
    {
        // fields
        public IMoveable moveable;

        // Unity methods
        public void LateUpdate()
        {
            transform.position = moveable.Update_Agent(Time.deltaTime);
            transform.forward = agent.Velocity.normalized / Time.deltaTime;
        }

        // methods
        public void Set_Moveable(Boid b)
        {            
            agent = b;
            moveable = b;
        }
    }
}
