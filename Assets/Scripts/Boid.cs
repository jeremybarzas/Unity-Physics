using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    [CreateAssetMenu(menuName = "Scriptables/Boid")]
    public class Boid : Agent
    {
        // inherited methods
        public override void Initialize(float m, float maxSpeed)
        {
            max_speed = maxSpeed;
            mass = m;
            velocity = Random.onUnitSphere;
            position = Vector3.zero;
            acceleration = Vector3.zero;
            force = Vector3.zero;
        }

        // methods        
        public bool Add_Force(float mag, Vector3 newForce)
        {
            // check for any change in velocity
            if (mag <= 0 || newForce.Equals(new Vector3(0, 0, 0)))
                return false;

            // apply force
            force += newForce * mag;
            return true;
        }

        public bool Add_Force(Vector3 newForce)
        {
            // check for any change in velocity
            if (newForce.Equals(new Vector3(0, 0, 0)))
                return false;

            // apply force
            force += newForce;
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
    }
}
