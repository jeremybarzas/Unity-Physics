using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class BoidBehaviour : AgentBehaviour
    {
        //fields
        public float speed;
        public float mass;
        public Vector3 position;
        public Vector3 velocity;        

        // properties
        public Boid Boid 
        {
            get { return (Boid)agent; }
            set { agent = Boid; }
        }

        // Unity methods
        public void Start()
        {
            Boid.Initialize(speed, mass, velocity, position);
        }

        public void LateUpdate()
        {
            //List<Vector3> forces = new List<Vector3>();
            //forces.Add(new Vector3(1, 1, 1));
            //forces.Add(new Vector3(1, 1, 1));
            //forces.Add(new Vector3(1, 1, 1));
            //forces.Add(new Vector3(1, 1, 1));
            //forces.Add(new Vector3(1, 1, 1));
            //forces.Add(new Vector3(1, 1, 1));

            //Boid.Apply_Forces(2, forces);

            transform.position = Boid.Update_Boid();
        }

        public void SetBoid(Boid b)
        { 
            
        }
    }
}
 