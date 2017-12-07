using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    [CreateAssetMenu(menuName = "Scriptables/FlockStats")]
    public class FlockStats : ScriptableObject
    {
        // fields        
        [SerializeField]
        public float max_speed;
        [SerializeField]
        public float max_force;
        [SerializeField]
        public float neighbor_distance;
        [SerializeField]
        public float alignment_scale;
        [SerializeField]
        public float cohesion_scale;
        [SerializeField]
        public float dispersion_scale;
        [SerializeField]
        public float dispersion_distance;
        [SerializeField]
        public float seek_scale;
    }
}
