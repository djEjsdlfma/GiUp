/*using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitlePlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float Speed;

    private void Update()
    {
        _rb.linearVelocity = SetMovement() * Speed;
    }

    private Vector3 SetMovement()
    {
        Vector2 input = Vector2.zero;

        input.x += Keyboard.current.aKey.isPressed ? -1f : 0;
        input.y += Keyboard.current.wKey.isPressed ? 1f : 0;
        input.x += Keyboard.current.dKey.isPressed ? 1f : 0;
        input.y += Keyboard.current.sKey.isPressed ? -1f : 0;

        return Quaternion.Euler(0, -30f, 0) * new Vector3(input.x, 0f, input.y);
    }
}*/