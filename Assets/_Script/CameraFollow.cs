using UnityEngine;

namespace Assets._Script
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float smoothing = 5f;

        private Vector3 offset;

        void Start()
        {
            offset = transform.position - target.position; //vitri cua cam - cho vi tri target
        }

        void FixedUpdate()
        {
            Vector3 targetCamPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }

    }
}
