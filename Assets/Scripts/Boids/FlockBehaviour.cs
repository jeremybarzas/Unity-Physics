using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facehead
{
    public class FlockBehaviour : MonoBehaviour
    {
        // fields        
        public List<Boid> boidList = new List<Boid>();
        public float max_speed = 75;
        public float max_force = 20;
        public float seekScale = 1;
        public float alignmentScale = 5;
        public float cohesionScale = 20;
        public float dispersionScale = 40;
        public float dispersion_distance = 2;
        public float neighbor_distance = 10;

        public FloatVariable Max_Speed;
        public FloatVariable Max_Force;
        public FloatVariable Seek_Scale;
        public FloatVariable Alignment_Scale;
        public FloatVariable Cohesion_Scale;
        public FloatVariable Dispersion_Scale;
        public FloatVariable Dispersion_Distance;
        public FloatVariable Neighbor_Distance;

        public GameObject seekTarget;

        public Vector3 flockCenter = Vector3.zero;
        public Vector3 flockForward = Vector3.zero;

        // Unity methods
        private void Start()
        {
            boidList = AgentFactory.Get_Boids();

            // assign slider default values
            Max_Speed.value = max_speed;
            Max_Force.value = max_force;
            Seek_Scale.value = seekScale;
            Alignment_Scale.value = alignmentScale;
            Cohesion_Scale.value = cohesionScale;
            Dispersion_Scale.value = dispersionScale;
            Dispersion_Distance.value = dispersion_distance;
            Neighbor_Distance.value = neighbor_distance;
        }

        private void Update()
        {
            if (boidList != AgentFactory.Get_Boids())
            {
                boidList = AgentFactory.Get_Boids();
            }

            // add force to all boids
            foreach (var b in boidList)
            {
                b.Max_Speed = Max_Speed.value;

                var v1 = Cohesion(b);
                var v2 = Dispersion(b);
                var v3 = Alignment(b);
                var v4 = Seek(b);

                b.Add_Force(Cohesion_Scale.value, v1);
                b.Add_Force(Dispersion_Scale.value, v2);
                b.Add_Force(Alignment_Scale.value, v3);
                b.Add_Force(Seek_Scale.value, v4);

                flockCenter += b.Position;
                flockForward += b.Velocity;
            }

            flockCenter /= boidList.Count;
            flockForward /= boidList.Count;
        }

        private void LateUpdate()
        {
            transform.position = flockCenter;
        }

        // methods
        public Vector3 Cohesion(Boid boid)
        {
            var force = Vector3.zero;
            var percievedCenter = Vector3.zero;

            foreach (var b in boidList)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < Neighbor_Distance.value)
                    {
                        percievedCenter += b.Position;
                    }
                }
            }

            percievedCenter = percievedCenter / (boidList.Count - 1);

            force = (percievedCenter - boid.Position);
            force = Vector3.ClampMagnitude(force, Max_Force.value);
            return force;
        }

        public Vector3 Dispersion(Boid boid)
        {
            Vector3 force = Vector3.zero;

            foreach (var b in boidList)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < Neighbor_Distance.value)
                    {
                        if ((b.Position - boid.Position).magnitude < Dispersion_Distance.value)
                        {
                            force = force - (b.Position - boid.Position);
                        }
                    }
                }
            }

            force = Vector3.ClampMagnitude(force, Max_Force.value);
            return force;
        }

        public Vector3 Alignment(Boid boid)
        {
            Vector3 force = Vector3.zero;
            var percievedVelocity = Vector3.zero;

            foreach (var b in boidList)
            {
                if (b != boid)
                {
                    if ((boid.Position - b.Position).magnitude < Neighbor_Distance.value)
                    {
                        percievedVelocity += b.Velocity;
                    }
                }
            }

            percievedVelocity = percievedVelocity / (boidList.Count - 1);

            force = (boid.Velocity - percievedVelocity);
            force = Vector3.ClampMagnitude(force, Max_Force.value);
            return force;
        }

        public Vector3 Seek(Boid boid)
        {
            var force = Vector3.zero;
            var seektarget = seekTarget.transform.position;

            force = (seektarget - boid.Position);
            force = Vector3.ClampMagnitude(force, Max_Force.value);
            return force;
        }        
    }
}
