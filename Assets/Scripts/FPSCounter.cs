using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facehead
{
    public class FPSCounter : MonoBehaviour
    {
        // fields
        private float fps = 0;

        // Unity methods
        private void Start()
        {
            StartCoroutine(FPS());
        }

        private void Update()
        {
            string s1 = "FPS: " + fps;            
            string s2 = "Boid Count: " + AgentFactory.Get_Boids().Count;
            string updatedText = s1 + "\n" + s2;
            GetComponent<Text>().text = updatedText;
        }

        private IEnumerator FPS()
        {
            while (true)
            {
                fps = 1 / Time.deltaTime;
                yield return new WaitForSeconds(1);
            }
        }
    }
}
