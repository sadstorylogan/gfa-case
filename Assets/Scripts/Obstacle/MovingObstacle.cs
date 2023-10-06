using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obstacle
{
    public class MovingObstacle : MonoBehaviour
    {
        public Vector3 startPoint;
        public Vector3 endPoint;
        public float speed = 1.0f;

        private bool movingToEnd = true;

        private void Update()
        {
            if (movingToEnd)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);
                if (transform.position == endPoint)
                    movingToEnd = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPoint, speed * Time.deltaTime);
                if (transform.position == startPoint)
                    movingToEnd = true;
            }
        }
    }
}
