using UnityEngine;
using Random = UnityEngine.Random;

namespace Components
{
    public class LightBehaviour : MonoBehaviour
    {
        private Quaternion targetRotation;
        private Vector3 targetPoint = new (1f, 2f, 1f);
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float flyingSpeed;
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
            rotationSpeed = Random.Range(100f, 300f);
            flyingSpeed = Random.Range(0.02f, 0.03f);
        }

        void Update()
        {
            // Always goes forward
            // Target is where the mouse hits the level
            // Tries to rotate towards the mouse hit
            var direction = targetPoint - transform.position;
            targetRotation = Quaternion.LookRotation(direction);

            UpdateTarget();
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * flyingSpeed;
        }

        private void UpdateTarget()
        {
            var position = Vector3.zero;
            if (Input.mousePresent)
            {
                position = Input.mousePosition;
            } 
            else if (Input.touchCount > 0)
            {
                position = Input.GetTouch(0).position;
            }

            if (Vector3.zero == position ||
                float.IsNegativeInfinity(position.x) || 
                float.IsNegativeInfinity(position.y) || 
                float.IsPositiveInfinity(position.x) || 
                float.IsPositiveInfinity(position.y)) return;
        
            var ray = mainCamera.ScreenPointToRay(position);
            var didHit = Physics.Raycast(ray, out var hit);
            if (!didHit) return;
            targetPoint = hit.point;// + Vector3.up * 2;
            targetPoint.y = .8f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, targetPoint);
        }
    }
}
