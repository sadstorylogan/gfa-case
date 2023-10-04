using System;
using Cinemachine;
using Enemy;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Movement
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float lookSpeed = 2f;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float minPitch = -40f;
        [SerializeField] private float maxPitch = 85f;
        [SerializeField] private float currentPitch = 0f;
        [SerializeField] private float gravity = -9.81f;

        private CharacterController characterController;
        private Vector2 currentLookDelta;
        private PlayerControls controls; // Reference to our custom input controls
        private Vector3 velocity; // To store vertical velocity for gravity
        private UIManager uiManager;



        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

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

        private void Start()
        {
            uiManager = FindObjectOfType<UIManager>();
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
            // Apply gravity
            if (characterController.isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // Small value to ensure the player stays grounded
            }
            else
            {
                velocity.y += gravity * Time.deltaTime;
            }

            // Combine horizontal and vertical movement
            var combinedMove = moveDirection + velocity;

            // Move the player using the CharacterController
            characterController.Move(combinedMove * Time.deltaTime);
        }

        private void HandleLook()
        {
            // Rotate the player based on mouse input
            transform.Rotate(Vector3.up * (currentLookDelta.x * lookSpeed));

            currentPitch -= currentLookDelta.y * lookSpeed;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);

            // Set the pitch of the Cinemachine camera
            virtualCamera.transform.localEulerAngles = new Vector3(currentPitch, 0f, 0f);
        }

        // This function will be called by the new input system
        public void OnLook(InputAction.CallbackContext context)
        {
            currentLookDelta = context.ReadValue<Vector2>();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("DeadlyObject"))
            {
                Die();
            }
        }

        private void Die()
        {
            // Disable player movement and other functionalities
            this.enabled = false;

            // Disable enemy movement and other functionalities
            var enemies = FindObjectsOfType<EnemyController>();
            foreach (var enemy in enemies)
            {
                enemy.enabled = false;
            }
            
            uiManager.ShowGameOverScreen();
        }
    }
}
