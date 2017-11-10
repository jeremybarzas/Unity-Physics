using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class BoidBehaviour : AgentBehaviour
    {
        // methods
        public void Set_Boid(Boid b)
        {
            b.Initialize(5, 1, Vector3.up, Vector3.up);            
            agent = b;            
        }

        public void Randomize_Boid(Boid b)
        {
            var x1 = Random.RandomRange(-10, 10);
            var y1 = Random.RandomRange(-10, 10);
            var z1 = Random.RandomRange(-10, 10);
            var magnitude = Random.RandomRange(1, 5);
            var direction = new Vector3(x1, y1, z1);
            direction.Normalize();
            var velocity = direction * magnitude;
            
            var x2 = Random.RandomRange(-20, 20);
            var y2 = Random.RandomRange(30, 50);
            var z2 = Random.RandomRange(-20, 20);
            var position = new Vector3(x2, y2, z2);

            b.Initialize(10, 1, velocity, position);
            agent = b;            
        }

        // Unity methods
        public void LateUpdate()
        {
            transform.position = agent.Update_Agent(Time.deltaTime);
            //transform.Rotate(agent.velocity.normalized);
            transform.forward = agent.velocity.normalized;
        }
    }
}
