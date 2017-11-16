using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facehead
{
    public class FlockBehaviour : MonoBehaviour
    {
        // fields        
        public float max_speed = 75;
        public float max_force = 20;
        public float alignmentScale = 5;
        public float cohesionScale = 20;
        public float dispersionScale = 40;
        public float dispersion_distance = 2;
        public float neighbor_distance = 10;
        public List<Boid> boidList = new List<Boid>();

        public Slider Max_Speed;
        public Slider Max_Force;
        public Slider Alignment_Scale;
        public Slider Cohesion_Scale;
        public Slider Dispersion_Scale;
        public Slider Dispersion_Distance;
        public Slider Neighbor_Distance;        

        // Unity methods
        private void Start()
        {
            boidList = AgentFactory.Get_Boids();
            Max_Speed.value = max_speed;
            Max_Force.value = max_force;
            Alignment_Scale.value = alignmentScale;
            Cohesion_Scale.value = cohesionScale;
            Dispersion_Scale.value = dispersionScale;
            Dispersion_Distance.value = dispersion_distance;
            Neighbor_Distance.value = neighbor_distance;
        }

        private void Update()
        {
            foreach (var b in boidList)
            {                
                b.Max_Speed = Max_Speed.value;

                var v1 = Cohesion(b);
                var v2 = Dispersion(b);
                var v3 = Alignment(b);
                
                b.Add_Force(Cohesion_Scale.value, v1);
                b.Add_Force(Dispersion_Scale.value, v2);
                b.Add_Force(Alignment_Scale.value, v3);                
            }

            transform.position = Get_Center();
        }

        // methods
        public Vector3 Get_Center()
        {
            var center = Vector3.zero;

            foreach (var b in boidList)
            {
                center += b.Position;
            }

            center = center / boidList.Count;
            return center;
        }

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
    }
}
