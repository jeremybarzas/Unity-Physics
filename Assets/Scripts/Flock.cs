using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    [CreateAssetMenu(menuName = "Scriptables/Flock")]
    public class Flock : ScriptableObject
    {
        // fields
        List<Boid> boid_list;
        FloatVariable max_speed;
        FloatVariable max_force;
        FloatVariable seek_scale;
        FloatVariable alignment_scale;
        FloatVariable cohesion_scale;
        FloatVariable dispersion_scale;
        FloatVariable dispersion_distance;
        FloatVariable neighbor_distance;

        // properties
        public List<Boid> Boid_List
        {
            get { return boid_list; }            
        }        
        public float Max_Speed
        {
            get { return max_speed.value; }
            set { max_speed.value = value; }
        }
        public float Max_Force
        {
            get { return max_force.value; }
            set { max_force.value = value; }
        }
        public float Seek_Scale
        {
            get { return seek_scale.value; }
            set { seek_scale.value = value; }
        }
        public float Alignment_Scale
        {
            get { return alignment_scale.value; }
            set { alignment_scale.value = value; }
        }
        public float Cohesion_Scale
        {
            get { return cohesion_scale.value; }
            set { cohesion_scale.value = value; }
        }
        public float Dispersion_Scale
        {
            get { return dispersion_scale.value; }
            set { dispersion_scale.value = value; }
        }
        public float Dispersion_Distance
        {
            get { return dispersion_distance.value; }
            set { dispersion_distance.value = value; }
        }
        public float Neighbor_Distance
        {
            get { return neighbor_distance.value; }
            set { neighbor_distance.value = value; }
        }
        public Vector3 Seek_Target
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

                force = Vector3.ClampMagnitude(force, max_force.value);
                b.Add_Force(force);
            }
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
                    if ((boid.Position - b.Position).magnitude < neighbor_distance.value)
                    {
                        percievedCenter += b.Position;
                    }
                }
            }

            percievedCenter = percievedCenter / (boid_list.Count - 1);

            force = (percievedCenter - boid.Position);
            force = Vector3.ClampMagnitude(force, max_force.value);
            return force;
        }

        public Vector3 Dispersion(Boid boid)
        {
            var force = Vector3.zero;

            foreach (var b in boid_list)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < neighbor_distance.value)
                    {
                        if ((b.Position - boid.Position).magnitude < dispersion_distance.value)
                        {
                            force = force - (b.Position - boid.Position);
                        }
                    }
                }
            }

            force = Vector3.ClampMagnitude(force, max_force.value);
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
                    if ((boid.Position - b.Position).magnitude < neighbor_distance.value)
                    {
                        percievedVelocity += b.Velocity;
                    }
                }
            }

            percievedVelocity = percievedVelocity / (boid_list.Count - 1);

            force = (boid.Velocity - percievedVelocity);
            force = Vector3.ClampMagnitude(force, max_force.value);
            return force;
        }

        public Vector3 Seek(Boid boid)
        {
            var force = Vector3.zero;

            force = (Seek_Target - boid.Position);
            force = Vector3.ClampMagnitude(force, max_force.value);
            return force;
        }
    }
}
