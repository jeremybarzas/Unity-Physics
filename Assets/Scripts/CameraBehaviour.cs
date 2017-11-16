using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class CameraBehaviour : MonoBehaviour
    {
        // fields    
        public float offsetDistance;
        public float scrollSpeed;
        public float rotateSpeed;

        private Camera camera;
        private Vector3 offsetVector;

        // Unity methods
        private void Start()
        {
            camera = GetComponentInChildren<Camera>();
            offsetVector = new Vector3(0, 0, -offsetDistance);
            camera.transform.localPosition = offsetVector;
        }

        private void LateUpdate()
        {
            Check_Input();
        }

        // methods
        private void Check_Input()
        {
            /*========== Mouse Wheel Zoom ==========*/
            // get mouse wheel input
            var mouseWheelDelta = (Input.GetAxisRaw("Mouse ScrollWheel") * scrollSpeed * 10);

            // check for change in mouse wheel
            if (mouseWheelDelta != 0)
            {
                // accomodate for facing camera toward parent transform
                mouseWheelDelta *= -1;

                // update offset distance based on mouse wheel delta
                offsetDistance += mouseWheelDelta;

                // update offset vector with new distance
                offsetVector.z = -offsetDistance;

                // hard assign camera local position to new offset vector
                camera.transform.localPosition = offsetVector;
            }

            /*========== click and drag Rotate ==========*/
            if (Input.GetKey("left alt"))
            {
                if (Input.GetMouseButton(0))
                {
                    var mouseXDelta = Input.GetAxis("Mouse X") * rotateSpeed;                    
                    var newRotation = new Vector3(0, mouseXDelta, 0);
                    transform.Rotate(newRotation);
                }
            }
        }
    }
}
