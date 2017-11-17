using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facehead
{
    public class AgentFactory : MonoBehaviour
    {
        // fields
        public GameObject boidPrefab;
        public int Count = 100;
        private static List<Agent> agentList = new List<Agent>();
        public List<GameObject> boidList = new List<GameObject>();

        public FloatVariable Boid_Count;
        public Button Respawn_Boids;
                
        public List<Agent> agents
        {
            get { return agentList; }
        }

        // Unity methods
        private void Awake()
        {
            Respawn_Boids.onClick.AddListener(Respawn_Flock);            
        }

        private void Start()
        {
            Boid_Count.value = Count;
            Spawn_Flock();
        }

        private void Update()
        {
            Count = (int)Boid_Count.value;
        }

        // methods      
        public void Spawn_Flock()
        {
            for (int i = 0; i < Count; i++)
            {
                Spawn_Boid();
            }
        }

        public void Spawn_Boid()
        {
            var go = Instantiate(boidPrefab);
            var skeleton = go.AddComponent<BoidBehaviour>();
            var boid = ScriptableObject.CreateInstance<Boid>();

            go.hideFlags = HideFlags.HideInHierarchy;
            boidList.Add(go);
            boid.Initialize();
            skeleton.Set_Moveable(boid);
            agentList.Add(boid);
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

        public void Respawn_Flock()
        {   
            // destroy and clear all boid prefab instances
            for (int i = boidList.Count - 1; i >= 0; i--)
            {
                Destroy(boidList[i]);
                boidList.RemoveAt(i);      
            }            

            // removes all agent scriptable objects
            for (int i = agentList.Count - 1; i >= 0; i--)
            {
                agentList.RemoveAt(i);
            }
            
            // recreates the flock based on new count
            Spawn_Flock();            
        }
    }
}
