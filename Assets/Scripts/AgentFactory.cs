using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facehead
{
    public class AgentFactory : MonoBehaviour
    {
        // fields
        public GameObject boidPrefab;
        public int Count;
        private static List<Agent> agentList = new List<Agent>();

        public Slider Boid_Count;
        public Button Respawn_Boids;

        // Unity methods
        private void Awake()
        {            
            
        }

        private void Start()
        {
            Boid_Count.value = Count;
            Respawn_Boids.onClick.AddListener(Respawn_Flock);
            Spawn_Flock();
        }

        private void Update()
        {
            //Count = (int)Boid_Count.value;
        }

        // methods      
        public void Spawn_Flock()
        {
            for (int i = 0; i < Boid_Count.value; i++)
            {
                Spawn_Boid();
            }
        }

        public void Spawn_Boid()
        {
            var go = Instantiate(boidPrefab);
            go.hideFlags = HideFlags.HideInHierarchy;
            var skeleton = go.AddComponent<BoidBehaviour>();
            var boid = ScriptableObject.CreateInstance<Boid>();

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

        public static void Respawn_Flock()
        {            
            var boids = FindObjectsOfType<BoidBehaviour>();
            foreach (var b in boids)
            {
                Destroy(b);
            }
            //boids = FindObjectsOfType<Boid>();
        }
    }
}
