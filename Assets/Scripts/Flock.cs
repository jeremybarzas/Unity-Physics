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
        private FlockStats flock_config;
        [SerializeField]
        private FlockStats flock_stats;
        [SerializeField]
        private List<Boid> boid_list;

        // properties
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
        public void Initialize()
        {
            flock_stats = Instantiate(flock_config);
            boid_list = new List<Boid>();
            Seek_Target = Vector3.zero;
            Flock_Center = Vector3.zero;
            Flock_Forward = Vector3.zero;
        }

        public void Update_Flock()
        {
            foreach (Boid b in boid_list)
            {
                var force = Vector3.zero;

                force += Cohesion(b) * flock_stats.cohesion_scale;
                force += Dispersion(b) * flock_stats.dispersion_scale;
                force += Alignment(b) * flock_stats.alignment_scale;
                force += Seek(b) * flock_stats.seek_scale;

                force = Vector3.ClampMagnitude(force, flock_stats.max_force);
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
            Flock_Center = Vector3.zero;

            var force = Vector3.zero;
            var percievedCenter = Vector3.zero;
            
            foreach (var b in boid_list)
            {
                Flock_Center += b.Position;

                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < flock_stats.neighbor_distance)
                    {                        
                        percievedCenter += b.Position;
                    }
                }
            }

            Flock_Center /= boid_list.Count;

            percievedCenter = percievedCenter / (boid_list.Count - 1);
            force = (percievedCenter - boid.Position);            
            return force;
        }

        public Vector3 Dispersion(Boid boid)
        {
            var force = Vector3.zero;

            foreach (var b in boid_list)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < flock_stats.neighbor_distance)
                    {
                        if ((b.Position - boid.Position).magnitude < flock_stats.dispersion_distance)
                        {
                            force = force - (b.Position - boid.Position);
                        }
                    }
                }
            }
            return force;
        }

        public Vector3 Alignment(Boid boid)
        {
            Flock_Forward = Vector3.zero;

            var force = Vector3.zero;
            var percievedVelocity = Vector3.zero;

            foreach (var b in boid_list)
            {
                Flock_Forward += b.Velocity;

                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < flock_stats.neighbor_distance)
                    {
                        percievedVelocity += b.Velocity;
                    }
                }
            }

            Flock_Forward /= boid_list.Count;

            percievedVelocity = percievedVelocity / (boid_list.Count - 1);
            force = (boid.Velocity - percievedVelocity);
            return force;
        }

        public Vector3 Seek(Boid boid)
        {
            var force = Vector3.zero;

            force = (Seek_Target - boid.Position);            
            return force;
        }

        public void DebugStats()
        {
            if (Input.GetKeyDown("space"))
            {
                if (flock_stats.GetInstanceID() == flock_config.GetInstanceID())
                    Debug.Log("InstanceID Indentical");
                else
                    Debug.Log("InstanceID Different");

                Debug.Log("flock_config speed: " + flock_config.max_speed);
                Debug.Log("flock_stats speed: " + flock_stats.max_speed);

                Debug.Log("flock_stats.speed -= 1f;");
                flock_stats.max_speed -= 1f;

                Debug.Log("flock_config speed: " + flock_config.max_speed);
                Debug.Log("flock_stats speed: " + flock_stats.max_speed);
            }
        }            
    }
}
