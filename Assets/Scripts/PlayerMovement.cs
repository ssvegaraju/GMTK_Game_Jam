using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float acceleration;
    [Range(1, 15)]
    public float moveSpeed = 8f;
    [Range(0f, 1f)]
    public float stopSpeed = 0.5f;
    public float jumpForce;
    public float rotateSpeed = 4f;

    private float horizontal, vertical;
    private bool onGround = false;

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
            if (Mathf.Abs(rigid.velocity.x) < moveSpeed) 
                rigid.velocity += Vector2.right * horizontal * acceleration * Time.fixedDeltaTime;
        }
        if (Mathf.Abs(horizontal) > 0.1f && onGround)
            rigid.MoveRotation(transform.eulerAngles.z - rotateSpeed * Mathf.Sign(horizontal));
        if (!onGround) {
            rigid.velocity += Vector2.up * Physics2D.gravity.y * 2.5f * Time.fixedDeltaTime;
        }
        if (onGround && vertical > 0.1f) {
            Jump();
        }
    }

    private void Jump() {
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        onGround = false;
        rigid.angularVelocity = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            onGround = true;
        }
    }
}
