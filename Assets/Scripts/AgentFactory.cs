using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class AgentFactory : MonoBehaviour
    {
        // fields
        public GameObject boidPrefab;
        public int Count;
        private static List<Agent> agentList = new List<Agent>();

        // properties
        public static List<Agent> Agents
        {
            get { return agentList; }
        }

        // methods      
        public void Create()
        {
            for (int i = 0; i < Count; i++)
            {                
                var go = Instantiate(boidPrefab);
                go.hideFlags = HideFlags.HideInHierarchy;
                var skeleton = go.AddComponent<BoidBehaviour>();
                var boid = ScriptableObject.CreateInstance<Boid>();
                
                boid.Initialize();
                skeleton.Set_Moveable(boid);
                agentList.Add(boid);
            }
        }

        public static List<Boid> Get_Boids()
        {
            var boids = new List<Boid>();

            foreach (Boid b in agentList)
            {
                boids.Add(b);
            }

            return boids;
        }

        // Unity methods
        private void Awake()
        {
            Create();
        }
    }
}
