using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    [CreateAssetMenu(menuName = "Agent/Boid")]
    public class Boid : Agent, IMoveable
    {
        // fields        
        private List<Boid> neighbors = new List<Boid>();

        // properties
        public List<Boid> Neighbors
        {
            get { return neighbors; }
        }
            
        // interface methods        
        public bool Add_Force(float mag, Vector3 newForce)
        {
            // check for any change in velocity
            if (mag <= 0 || newForce.Equals(new Vector3(0, 0, 0)))
                return false;

            // apply force
            force += newForce * mag;
            return true;
        }

        public Vector3 Update_Agent(float deltaTime)
        {
            acceleration = force / mass;
            velocity += acceleration * deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, max_speed);
            position += velocity * deltaTime;
            force = Vector3.zero;
            return position;
        }

        // inherited methods
        public override void Initialize()
        {
            max_speed = 50;
            mass = 1;
            velocity = Random.onUnitSphere;
            position = Vector3.zero;
            acceleration = Vector3.zero;
            force = Vector3.zero;
        }

        public void Set_Neighbors(float size)
        {
            List<Boid> boidList = AgentFactory.Get_Boids();            

            // check for neighbors within set distance
            foreach (var b in boidList)
            {
                if (b != this)
                {
                    if ((b.Position - this.Position).magnitude < size)
                    {
                        if (!this.neighbors.Contains(b))
                        {
                            this.neighbors.Add(b);
                        }

                    }
                    else
                    {
                        if (this.neighbors.Contains(b))
                        {
                            this.neighbors.Remove(b);
                        }
                    }
                }
            }
        }
    }
}
