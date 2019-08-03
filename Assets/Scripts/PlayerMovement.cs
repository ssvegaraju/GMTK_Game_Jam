using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Range(1, 15)]
    public float moveSpeed = 8f;
    public float jumpForce = 8f;
    public float unsnapReleaseForce = 10;
    public float rotateSpeed = 4f;
    public float lowJumpMultiplier = 1.5f, fallMultiplier = 2.5f;

    public event System.Action OnRespawn;

    private float horizontal;
    private bool vertical;
    private bool rotate = false;
    private bool onGround = false;

    private bool isSnapped = false;
    private bool isDead = false;

    private Rigidbody2D rigid;
    public Collider2D col;
    private Animator anim;

    private Vector3 moveDirection;

    private Vector3 respawnPos;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        respawnPos = transform.position;
    }

    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetButton("Jump");
        rotate = Input.GetButton("Rotate");
        if (!isDead) {
            if (!isSnapped) {
                moveDirection = Vector3.right * horizontal * moveSpeed;
                if (rotate)
                    rigid.MoveRotation(transform.eulerAngles.z - rotateSpeed);
                if (onGround && vertical) {
                    Jump();
                } else {
                    moveDirection.y = rigid.velocity.y;
                }
                rigid.velocity = moveDirection;
                if (rigid.velocity.normalized.y < 0)
                    rigid.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
                else if (rigid.velocity.normalized.y > 0 && !vertical)
                    rigid.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
                anim.SetFloat("velocity", rigid.velocity.sqrMagnitude);
            }
        } else {
            rigid.MoveRotation(transform.eulerAngles.z - rotateSpeed);
        }
    }

    public void OnSnap() {
        isSnapped = true;
        onGround = true;
        anim.SetBool("onGround", onGround);
        anim.SetBool("Snap", true);
    }

    public void OnUnsnap() {
        anim.SetBool("Snap", false);
        anim.SetTrigger("Unsnap");
        rigid.velocity = transform.up * unsnapReleaseForce;
        Invoke("UnsnapComplete", 0.2f);
    }

    public void Respawn() {
        if (OnRespawn != null)
            OnRespawn();
        isDead = false;
        rigid.gravityScale = 1f;
        isSnapped = false;
        transform.position = respawnPos;
    }

    private void UnsnapComplete() {
        anim.ResetTrigger("Unsnap");
        isSnapped = false;
    }

    private void Jump() {
        moveDirection.y = jumpForce;
        onGround = false;
        anim.SetBool("onGround", onGround);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            if (transform.position.y > collision.transform.position.y) {
                onGround = true;
                anim.SetBool("onGround", onGround);
            }
        }
        if (collision.gameObject.CompareTag("BAD")) {
            rigid.gravityScale = 0f;
            isDead = true;
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().ShakeScreen(4f, 1f);
            GameObject.Find("SceneManager").GetComponent<SceneTransitions>().Respawn();
        }
        if (collision.gameObject.CompareTag("Key")) {
            GameObject.Find("KeyDoor").GetComponent<CollectTriangles>().Increment();
            Object.Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Respawn")) {
            respawnPos = collision.gameObject.transform.position;
            collision.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
}
