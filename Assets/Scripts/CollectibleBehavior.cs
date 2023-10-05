using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 50f; // Speed of rotation (degrees per second)
    [SerializeField] private float bounceHeight = 0.5f; // Maximum height of the bounce
    [SerializeField] private float bounceSpeed = 2f;    // Speed of the bounce

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        RotateObject();
        BounceObject();
    }

    private void RotateObject()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void BounceObject()
    {
        var newY = startPosition.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
