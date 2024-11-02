using UnityEngine;

namespace Starbend.Voxel
{

    public class CameraController : MonoBehaviour
    {
        public float moveSpeed = 10f; 
        public float lookSpeed = 100f;

        private float xRotation = 0f;

        void Update()
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
            float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

            transform.Translate(new Vector3(moveX, 0, moveZ));

            float mouseX = Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -180, 180); // Limit vertical rotation

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }

}
