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
        // Duration of the slide
        [SerializeField] private float slideDuration = 1f; 
        // Height of the character during the slide
        [SerializeField] private float slideHeight = 0.5f; 

        // To check if the player is currently sliding
        private bool isSliding = false;
        // To store the original height of the character
        private float originalHeight; 
        // To track the duration of the slide
        private float slideTimer = 0f; 


        private CharacterController characterController;
        private Vector2 currentLookDelta;
        // Reference to our custom input controls
        private PlayerControls controls; 
        // To store vertical velocity for gravity
        private Vector3 velocity;
        
        // The height of the jump
        public float jumpHeight = 2.0f;
        // To check if the player is currently jumping
        private bool isJumping = false; 
        // To store the vertical component of our movement
        private float verticalSpeed = 0; 
        // Speed of transitioning from standing to sliding and vice versa
        public float slideTransitionSpeed = 5f; 
        
        private UIManager uiManager;
        private Animator animator;

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
            animator = GetComponent<Animator>();

            // Initialize the controls
            controls = new PlayerControls();

            // Bind the look event
            controls.Player.Look.performed += ctx => currentLookDelta = ctx.ReadValue<Vector2>();
            controls.Player.Look.canceled += ctx => currentLookDelta = Vector2.zero;
            controls.Player.Jump.performed += ctx => OnJump(); 
            controls.Player.Jump.canceled += ctx => isJumping = false;
            controls.Player.Slide.performed += ctx => StartSlide();
            controls.Player.Slide.canceled += ctx => StopSlide();
        }
        
        private void StartSlide()
        {
            if (!isSliding && characterController.isGrounded)
            {
                isSliding = true;
                originalHeight = characterController.height;
                characterController.height = slideHeight;
                slideTimer = 0f; // Reset the slide timer
            }
        }

        private void StopSlide()
        {
            if (isSliding)
            {
                isSliding = false;
                characterController.height = originalHeight;
            }
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
            HandleJump();
            HandleSlide();
        }
        
        private void HandleSlide()
        {
            if (isSliding)
            {
                slideTimer += Time.deltaTime;
                if (slideTimer >= slideDuration)
                {
                    StopSlide();
                }
            }
        }

        private void HandleJump()
        {
            if (characterController.isGrounded && isJumping)
            {
                verticalSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity);
                // Reset the jump 
                isJumping = false; 
            }

            // Apply gravity
            verticalSpeed += gravity * Time.deltaTime;

            // Create a vector3 for vertical movement
            var verticalMove = new Vector3(0, verticalSpeed, 0) * Time.deltaTime;

            // Apply vertical movement
            characterController.Move(verticalMove);
        }
        
        public void OnJump()
        {
            if (characterController.isGrounded)
            {
                isJumping = true;
            }
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
            else if (hit.gameObject.CompareTag("Coin"))
            {
                Win();
                // Remove the object from the scene
                Destroy(hit.gameObject); 
            }
        }

        private void Win()
        {
            // Disable player movement and other functionalities
            this.enabled = false;

            // Show the win screen
            if (uiManager != null)
            {
                uiManager.ShowWinScreen();
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
