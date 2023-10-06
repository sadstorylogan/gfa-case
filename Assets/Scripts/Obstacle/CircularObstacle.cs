using UnityEngine;

namespace Obstacle
{
    public class CircularObstacle : MonoBehaviour
    {
        [SerializeField] private Transform centerPoint; // The center around which the obstacle will rotate
        [SerializeField] private float rotationSpeed = 30f; // Speed of rotation in degrees per second

        private void Update()
        {
            transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
