using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{ 
    public abstract class Agent : ScriptableObject
    {
        // variables
        [SerializeField]
        protected float max_speed;
        [SerializeField]
        protected float mass;
        [SerializeField]
        protected Vector3 velocity;
        [SerializeField]
        protected Vector3 position;
        [SerializeField]
        protected Vector3 acceleration;
        [SerializeField]
        protected Vector3 force;

        // properties
        public float Max_Speed
        {
            get { return max_speed; }
            set { max_speed = value; }
        }
        public float Mass
        {
            get { return mass; }
        }
        public Vector3 Position
        {
            get { return position; }
        }
        public Vector3 Velocity
        {
            get { return velocity; }
        }
        public Vector3 Acceleration
        {
            get { return acceleration; }
        }
        public Vector3 Force
        {
            get { return force; }
        }

        // methods
        public abstract void Initialize(float mass, float maxSpeed);
    }
}
