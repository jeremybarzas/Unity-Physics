using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Facehead
{
    public class SliderBehaviour : MonoBehaviour
    {
        private Slider sliderComponent;
        private Text textComponent;
        public FloatVariable floatVariable;

        // Unity methods
        void Start()
        {
            sliderComponent = GetComponent<Slider>();
            textComponent = GetComponentInChildren<Text>();

            sliderComponent.value = floatVariable.value;
        }

        void Update()
        {
            floatVariable.value = sliderComponent.value;
            Set_Text();
        }

        // methods
        private void Set_Text()
        {
            textComponent.text = textComponent.name + ": " + floatVariable.value.ToString();
        }
    }
}
