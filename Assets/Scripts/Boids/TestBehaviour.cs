using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class TestBehaviour : MonoBehaviour
    {
        // fields
        public Flock flock;

        // Unity methods
        private void Start()
        {
            flock.Initialize();   
        }

        private void Update()
        {
            flock.Update_Flock();
            flock.DebugStats();
        }

        private void LateUpdate()
        {
            transform.position = flock.Flock_Center;
        }
    }
}
