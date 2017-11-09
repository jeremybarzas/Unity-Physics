using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    [CreateAssetMenu(menuName = "Agent/Boid")]
    public class Boid : Agent
    {
        // properties        
        public float Speed
        {
            get { return speed; }
            set { speed = Speed; }
        }
        public float Mass
        {
            get { return mass; }
            set { mass = Mass; }
        }
        public Vector3 Position
        {
            get { return position; }
            set { position = Position; }
        }
        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = Velocity; }
        }
        public Vector3 Acceleration
        {
            get { return acceleration; }
            set { acceleration = Acceleration; }
        }       
        public Vector3 Force
        {
            get { return force; }
            set { force = Force; }
        }

        // methods
        public void Initialize(float spd, float mas, Vector3 velo, Vector3 pos)
        {
            Speed = spd;
            Mass = mas;
            Velocity = velo;
            Position = pos;
        }

        // inherited methods
        public override bool Add_Force(float mag, Vector3 newForce)
        {
            // check for any change in velocity
            if (mag <= 0 || newForce.Equals(new Vector3(0,0,0)))
                return false;
            
            // apply force
            Force += newForce * mag;
            return true;
        }

        public override Vector3 Update_Agent(float deltaTime)
        {
            Acceleration = Force * 1 / Mass;
            Velocity += Acceleration * deltaTime;
            Velocity = Vector3.ClampMagnitude(Velocity, Speed);
            Position += Velocity * deltaTime;
            return Position;
        }     
    }
}
