using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.InputSystem;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        
        private CharacterController characterController;
        private float gravity = -9.81f;
        private Vector3 playerVelocity;
        
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var moveDirection = transform.forward * (moveSpeed * Time.deltaTime);
            characterController.Move(moveDirection);
        }
    }
}
