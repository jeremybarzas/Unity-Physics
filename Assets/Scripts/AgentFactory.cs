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
        private static List<Agent> agents = new List<Agent>();

        // properties
        public static List<Agent> Agents
        {
            get { return agents; }
        }

        // methods      
        public void Create()
        {
            for (int i = 0; i < Count; i++)
            {
                var go = Instantiate(boidPrefab);
                var skeleton = go.AddComponent<BoidBehaviour>();

                var agent = ScriptableObject.CreateInstance<Boid>();                
                //skeleton.Set_Boid(agent);
                skeleton.Randomize_Boid(agent);
                agents.Add(agent);
            }
        }        

        // Unity methods
        private void Awake()
        {
            Create();
        }
    }
}
