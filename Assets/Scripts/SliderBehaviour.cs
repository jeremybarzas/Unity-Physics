using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facehead
{
    public class SliderBehaviour : MonoBehaviour
    {
        public Slider sliderComponent;
        public Text textComponent;

        // Unity methods
        void Start()
        {
            sliderComponent = GetComponent<Slider>();
            textComponent = GetComponentInChildren<Text>();
            Set_Text();
        }

        void Update()
        {
            Set_Text();
        }

        // methods
        private void Set_Text()
        {
            textComponent.text = textComponent.name + ": " + ((int)sliderComponent.value).ToString();
        }
    }
}
