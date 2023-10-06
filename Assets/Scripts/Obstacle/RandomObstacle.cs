using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObstacle : MonoBehaviour
{
    [SerializeField] private Vector3 areaSize = new Vector3(5f, 0f, 5f); // Defines the size of the area within which the obstacle can move
    [SerializeField] private float moveSpeed = 1f; // Speed at which the obstacle moves to the random point

    private Vector3 targetPosition;

    private void Start()
    {
        PickRandomTarget();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            PickRandomTarget();
        }
    }

    private void PickRandomTarget()
    {
        var randomX = Random.Range(-areaSize.x / 2, areaSize.x / 2);
        var randomZ = Random.Range(-areaSize.z / 2, areaSize.z / 2);

        targetPosition = new Vector3(randomX, transform.position.y, randomZ) + transform.position;
    }
}
