using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    [CreateAssetMenu(menuName = "Scriptables/Flock")]
    public class Flock : ScriptableObject
    {
        // fields
        [SerializeField]
        List<Boid> boid_list;
        [SerializeField]
        float max_speed;
        [SerializeField]
        float max_force;
        [SerializeField]
        float seek_scale;
        [SerializeField]
        float alignment_scale;
        [SerializeField]
        float cohesion_scale;
        [SerializeField]
        float dispersion_scale;
        [SerializeField]
        float dispersion_distance;
        [SerializeField]
        float neighbor_distance;

        // properties
        public List<Boid> Boid_List
        {
            get { return boid_list; }            
        }        
        public float Max_Speed
        {
            get { return max_speed; }            
        }
        public float Max_Force
        {
            get { return max_force; }            
        }
        public float Seek_Scale
        {
            get { return seek_scale; }
        }
        public float Alignment_Scale
        {
            get { return alignment_scale; }
        }
        public float Cohesion_Scale
        {
            get { return cohesion_scale; }
        }
        public float Dispersion_Scale
        {
            get { return dispersion_scale; }
        }
        public float Dispersion_Distance
        {
            get { return dispersion_distance; }
        }
        public float Neighbor_Distance
        {
            get { return neighbor_distance; }
        }
        public Vector3 Seek_Target
        {
            get;
            set;
        }
        public Vector3 Flock_Center
        {
            get;
            set;
        }
        public Vector3 Flock_Forward
        {
            get;
            set;
        }

        // methods
        public void Update_Flock()
        {
            foreach (Boid b in boid_list)
            {
                var force = Vector3.zero;

                force += Cohesion(b) * Cohesion_Scale;
                force += Dispersion(b) * Dispersion_Scale;
                force += Alignment(b) * Alignment_Scale;
                force += Seek(b) * Seek_Scale;

                force = Vector3.ClampMagnitude(force, max_force);
                b.Add_Force(force);

                Flock_Center += b.Position;
                Flock_Forward += b.Velocity;
            }

            Flock_Center /= boid_list.Count;
            Flock_Forward /= boid_list.Count;
        }

        public void Add_Boid(Boid b)
        {
            boid_list.Add(b);
        }

        public void Remove_Boid(Boid b)
        {
            boid_list.Remove(b);
        }

        public Vector3 Cohesion(Boid boid)
        {
            var force = Vector3.zero;
            var percievedCenter = Vector3.zero;

            foreach (var b in boid_list)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < neighbor_distance)
                    {
                        percievedCenter += b.Position;
                    }
                }
            }

            percievedCenter = percievedCenter / (boid_list.Count - 1);

            force = (percievedCenter - boid.Position);
            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }

        public Vector3 Dispersion(Boid boid)
        {
            var force = Vector3.zero;

            foreach (var b in boid_list)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < neighbor_distance)
                    {
                        if ((b.Position - boid.Position).magnitude < dispersion_distance)
                        {
                            force = force - (b.Position - boid.Position);
                        }
                    }
                }
            }

            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }

        public Vector3 Alignment(Boid boid)
        {
            var force = Vector3.zero;
            var percievedVelocity = Vector3.zero;

            foreach (var b in boid_list)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < neighbor_distance)
                    {
                        percievedVelocity += b.Velocity;
                    }
                }
            }

            percievedVelocity = percievedVelocity / (boid_list.Count - 1);

            force = (boid.Velocity - percievedVelocity);
            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }

        public Vector3 Seek(Boid boid)
        {
            var force = Vector3.zero;

            force = (Seek_Target - boid.Position);
            force = Vector3.ClampMagnitude(force, max_force);
            return force;
        }
    }
}
