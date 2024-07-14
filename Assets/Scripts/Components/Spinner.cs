using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spinner : MonoBehaviour
{
    private void Start()
    {
        transform.Rotate(Vector3.back, Random.Range(0f, 360f));
    }

    private void Update()
    {
        transform.Rotate(Vector3.back, Time.deltaTime * 5f);
    }
}
