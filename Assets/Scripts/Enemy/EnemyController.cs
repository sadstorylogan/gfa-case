using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private float speed = 5f;

        private void Update()
        {
            var direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * (speed * Time.deltaTime);
        }
    }
}
