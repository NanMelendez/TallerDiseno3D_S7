using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private InputActionReference moveAction;
    [Min(0.0f)] public float speed = 1.0f;

    private Vector2 moveDir = Vector2.zero;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    private void Update()
    {
        moveDir = moveAction ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
    }

    private void FixedUpdate()
    {
        Vector3 finalDir = (transform.forward * moveDir.y) + (transform.right * moveDir.x);

        controller.Move(speed * Time.deltaTime * finalDir);
    }
}
