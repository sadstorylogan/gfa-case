using System;
using Player.Movement;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float rotationSpeed = 10f;

        private CharacterController characterController;
        private Transform playerTransform;
        

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            if (PlayerController.instance != null)
            {
                playerTransform = PlayerController.instance.transform;
            }
        }

        private void Update()
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }

        private void RotateTowardsPlayer()
        {
            var directionToPlayer = playerTransform.position - transform.position;
            // Ensure the enemy doesn't tilt upwards/downwards
            directionToPlayer.y = 0; 

            // Calculate the rotation required to look at the player
            var targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        private void MoveTowardsPlayer()
        {
            var direction = (playerTransform.position - transform.position).normalized;
            var movement = direction * (speed * Time.deltaTime);

            // Move the enemy using the CharacterController
            characterController.Move(movement);
        }
    }
}
