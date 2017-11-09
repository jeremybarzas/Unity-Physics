using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{ 

    public abstract class Agent : ScriptableObject
    {
        // variables
        [SerializeField]
        protected float speed;
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

        // methods       
        protected abstract bool Add_Force(float magnitude, Vector3 newForce);
        protected abstract Vector3 Update_Agent(float deltaTime);
    }
}
