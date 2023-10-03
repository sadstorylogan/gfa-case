using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float lookSpeed = 2f;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float minPitch = -40f;
        [SerializeField] private float maxPitch = 85f;
        [SerializeField] private float currentPitch = 0f;

        private CharacterController characterController;
        private Vector2 currentLookDelta;
        private PlayerControls controls; // Reference to our custom input controls


        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            
            // Initialize the controls
            controls = new PlayerControls();

            // Bind the look event
            controls.Player.Look.performed += ctx => currentLookDelta = ctx.ReadValue<Vector2>();
            controls.Player.Look.canceled += ctx => currentLookDelta = Vector2.zero;
        }
        
        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void Update()
        {
            HandleMovement();
            HandleLook();
        }

        private void HandleMovement()
        {
            // Move the player forward automatically
            var moveDirection = transform.forward * moveSpeed;
            characterController.SimpleMove(moveDirection);
        }
        
        private void HandleLook()
        {
            // Rotate the player based on mouse input
            transform.Rotate(Vector3.up * (currentLookDelta.x * lookSpeed));

            currentPitch -= currentLookDelta.y * lookSpeed;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

            // Adjust the pitch of the camera based on mouse input
            // virtualCamera.transform.Rotate(Vector3.left * (currentLookDelta.y * lookSpeed));
            
            // Set the pitch of the Cinemachine camera
            virtualCamera.transform.localEulerAngles = new Vector3(currentPitch, 0f, 0f);
        }
        

        // This function will be called by the new input system
        public void OnLook(InputAction.CallbackContext context)
        {
            currentLookDelta = context.ReadValue<Vector2>();
        }
    }
}
