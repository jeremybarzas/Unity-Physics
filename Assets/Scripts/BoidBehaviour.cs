using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class BoidBehaviour : AgentBehaviour
    {
        // methods
        public void SetBoid(Boid b)
        {
            b.Initialize(5, 5, Vector3.up, Vector3.up);
            agent = b;
        }

        // Unity methods
        public void LateUpdate()
        {
            transform.position = agent.Update_Agent(Time.deltaTime);
        }
    }
}
