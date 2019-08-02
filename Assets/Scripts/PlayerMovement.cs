using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    [Range(1, 15)]
    public float maxSpeed = 8f;
    [Range(0f, 1f)]
    public float stopSpeed = 0.5f;
    public float jumpForce;

    private float horizontal, vertical;

    private Rigidbody2D rigid;
    private Collider2D col;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    private void Update() {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(horizontal) < 0.1f) {
            rigid.velocity = Vector2.Lerp(rigid.velocity, Vector2.up * rigid.velocity.y, stopSpeed);
        } else {
            rigid.velocity += Vector2.right * horizontal * moveSpeed * Time.fixedDeltaTime;
        }

    }
}
