using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Facehead
{
    public class ClothCamera : MonoBehaviour
    {
        // fields
        private Camera camera;
        public Vector3 offsetVector;
        public float offsetDistance;
        public float scrollSpeed;
        public float rotateSpeed;

        // Unity methods
        private void Start()
        {
            ClothBehaviour cloth = GetComponentInParent<ClothBehaviour>();
            offsetVector = new Vector3((cloth.width * .25f) * cloth.width, -(cloth.length * .25f) * cloth.length, -offsetDistance);
            transform.localPosition = offsetVector;
            camera = GetComponentInChildren<Camera>();
        }

        private void LateUpdate()
        {
            Check_Input();

            ScreenToWorld();
        }

        // methods
        private void Check_Input()
        {
            /*========== Mouse Wheel Zoom ==========*/
            // get mouse wheel input
            var mouseWheelDelta = ((Input.GetAxisRaw("Mouse ScrollWheel") * scrollSpeed * 100) * Time .deltaTime);

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

            offsetVector = new Vector3(offsetVector.x, offsetVector.y, -offsetDistance);
            camera.transform.localPosition = offsetVector;
        }

        public void ScreenToWorld()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mouseX = Input.GetAxis("Horizontal");
                var mouseY = Input.GetAxis("Vertical");

                var point = camera.ScreenToWorldPoint(new Vector3(mouseX, mouseY, camera.nearClipPlane));                
                point.Normalize();

                Ray ray = new Ray(camera.transform.position, point);
                var rayPoint = ray.GetPoint(100);

                Debug.Log("Camera Pos: " + camera.transform.position);
                Debug.Log("RayTarget: " + rayPoint);
            }            
        }
    }
}
