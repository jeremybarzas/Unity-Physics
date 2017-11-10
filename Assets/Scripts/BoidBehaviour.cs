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
            Randomize_Boid(b);
            //b.Initialize(5, 5, Vector3.up, Vector3.up);
            agent = b;
        }

        public void Randomize_Boid(Boid b)
        {            
            var x1 = Random.RandomRange(-1, 1);
            var y1 = Random.RandomRange(-1, 1);
            var z1 = Random.RandomRange(-1, 1);
            var mag = Random.RandomRange(1, 5);
            var velo = new Vector3(x1, y1, z1);
            velo = velo * mag;
            
            var x2 = Random.RandomRange(-10, 10);
            var y2 = Random.RandomRange(-10, 10);
            var z2 = Random.RandomRange(-10, 10);
            var pos = new Vector3(x2, y2, z2);            

            b.Initialize(10, 1, velo, pos);
        }

        // Unity methods
        public void LateUpdate()
        {
            transform.position = agent.Update_Agent(Time.deltaTime);
        }
    }
}
