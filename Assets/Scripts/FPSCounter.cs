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
        private float avgFps = 0;

        // Unity methods
        private void Start()
        {
            StartCoroutine(FPS());
        }

        private void Update()
        {
            string s1 = "FPS: " + fps;
            string s2 = "Avg FPS: " + avgFps;
            string s3 = "Boid Count: " + AgentFactory.Get_Boids().Count;
            string updatedText = s1 + "\n" + s2 + "\n" + s3;
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
