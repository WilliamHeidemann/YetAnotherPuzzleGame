using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Swamer : MonoBehaviour
{
    private Quaternion targetRotation;
    private Vector3 targetPoint = new (2f, 2f, 2f);
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float flyingSpeed;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        rotationSpeed = Random.Range(100f, 300f);
        flyingSpeed = Random.Range(0.01f, 0.03f);
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
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        var didHit = Physics.Raycast(ray, out var hit);
        if (!didHit) return;
        targetPoint = hit.point + Vector3.up * 2;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, targetPoint);
    }
}
