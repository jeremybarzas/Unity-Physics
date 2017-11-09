using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class BoidBehaviour : AgentBehaviour
    {
        // methods
        public void SetBoid(Agent b)
        {
            agent = (Boid)b;
            ((Boid)agent).Initialize(5, 1, Vector3.up, Vector3.zero);
        }

        // Unity methods
        public void LateUpdate()
        {            
            transform.position = agent.Update_Agent(Time.deltaTime);
        }        
    }
}
 