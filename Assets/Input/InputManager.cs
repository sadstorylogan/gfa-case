using System;
using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        private PlayerInput playerInput;
        private PlayerInput.PlayerActions playerActions;

        private void Awake()
        {
            playerInput = new PlayerInput();
            playerActions = playerInput.Player;
        }

        private void OnEnable()
        {
            playerActions.Enable();
        }

        private void OnDisable()
        {
            playerActions.Disable();
        }
    }
}
