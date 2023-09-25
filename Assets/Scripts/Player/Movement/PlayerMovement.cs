using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5.0f;
        [SerializeField] private float jumpForce = 5.0f;
        [SerializeField] private float gravity = 9.81f;

        private CharacterController characterController;
        private Vector3 velocity;
        private bool isGrounded = true;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            isGrounded = characterController.isGrounded;

            if (!isGrounded)
            {
                velocity.y -= gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = -2f; // Reset vertical velocity when grounded
            }
            
            // Auto-Run Forward
            var moveDirection = transform.forward * moveSpeed;

            // Apply jump
            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }

            // Apply the final movement
            characterController.Move((moveDirection + velocity) * Time.deltaTime);
            
        }
    }
}
