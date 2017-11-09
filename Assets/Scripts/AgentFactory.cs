using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class AgentFactory : MonoBehaviour
    {
        // fields
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
                var go = new GameObject();
                var skeleton = go.AddComponent<BoidBehaviour>();

                var agent = ScriptableObject.CreateInstance<Agent>();
                skeleton.SetBoid(agent);

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
