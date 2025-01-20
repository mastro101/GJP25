using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;

    InputAction moveAction;
    Vector2 inputDirection;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
    }

    private void Update()
    {
        inputDirection = moveAction.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
    }
}