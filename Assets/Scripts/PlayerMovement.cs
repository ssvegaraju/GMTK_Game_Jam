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
    public float unsnapReleaseForce = 10;
    public float rotateSpeed = 4f;

    public float RotateSpeed = 30f;

    private float horizontal, vertical;
    private bool rotate = false;
    private bool onGround = false;
    
    private bool isDead = false;

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
        rotate = Input.GetButton("Jump");
        onGround = (SnapToPosition.snapped) ? false : onGround;
    }

    private void FixedUpdate()
    {
        if (!isDead) {
            if (onGround && Mathf.Abs(horizontal) < 0.1f) {
                rigid.velocity = Vector2.Lerp(rigid.velocity, Vector2.up * rigid.velocity.y, stopSpeed);
            } else {
                if (Mathf.Abs(rigid.velocity.x) < moveSpeed) 
                    rigid.velocity += Vector2.right * horizontal * acceleration * Time.fixedDeltaTime;
            }
            if (rotate)
                rigid.MoveRotation(transform.eulerAngles.z - rotateSpeed);
            if (!onGround) {
                rigid.velocity += Vector2.up * Physics2D.gravity.y * 2.5f * Time.fixedDeltaTime;
            }
            if (onGround && vertical > 0.1f) {
                Jump();
            }
        } else {
            rigid.MoveRotation(transform.eulerAngles.z - rotateSpeed);
        }
    }

    private void Jump() {
        rigid.velocity = Vector2.up * jumpForce;
        onGround = false;
        rigid.angularVelocity = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            onGround = true;
        }
        if (collision.gameObject.CompareTag("BAD")) {
            rigid.gravityScale = 0f;
            isDead = true;
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().ShakeScreen(4f, 1f);
            Debug.Log("Test");
            GameObject.Find("SceneManager").GetComponent<SceneTransitions>().SwitchScene();
        }
    }

    public void Unsnap(float dir) {
        transform.eulerAngles = Vector3.forward * dir;
        rigid.AddForce(transform.up.normalized * unsnapReleaseForce, ForceMode2D.Impulse);
    }
}
