using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapToPosition : MonoBehaviour
{
    public float detectionRadius;
    public Vector3 offset;
    public PlayerMovement objectToSnap;
    public GameObject objectToSnapTo;
    private bool snapped = false;

    public UnityEvent OnSnap, OnUnsnap;

    private GameObject particles;
    private GameObject laser;
    public LayerMask bossLayer;

    private CircleCollider2D col;
    private bool finishedUnsnapping = true;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CircleCollider2D>();
        if (col == null)
            col = gameObject.AddComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = detectionRadius;
        if (objectToSnap == null)
            objectToSnap = FindObjectOfType<PlayerMovement>();
        OnSnap.AddListener(delegate () {
            objectToSnap.OnSnap();
        });
        OnUnsnap.AddListener(SpawnParticles);
        OnUnsnap.AddListener(delegate () {
            objectToSnap.OnUnsnap();
        });
        particles = Resources.Load("UnsnapParticle") as GameObject;
        laser = Resources.Load("Laser") as GameObject;
        objectToSnap.OnRespawn += PlayerRespawning;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!snapped && collision.gameObject.CompareTag("Player")) {
            Snap();
        }
    }

    public void SpawnLaser() {
        GameObject g = Instantiate(laser, objectToSnap.transform.position, objectToSnap.transform.rotation);
        Destroy(g, 1.5f);
        StartCoroutine(CheckCollision(g));
    }

    private IEnumerator CheckCollision(GameObject g) {
        while (g != null) {
            Debug.DrawRay(g.transform.position, g.transform.up * 100, Color.white);
            RaycastHit2D hit = Physics2D.Raycast(g.transform.position, g.transform.up, 100, bossLayer);
            if (hit) {
                hit.collider.GetComponent<Boss>().TakeDamage(1);
            }
            yield return null;
        }
    }

    private void Snap() {
        if (snapped || !finishedUnsnapping)
            return;
        snapped = true;
        if (OnSnap != null) {
            OnSnap.Invoke();
        }
        finishedUnsnapping = false;
        StartCoroutine(WhileSnapping());
    }

    private IEnumerator WhileSnapping() {
        while (snapped) {
            objectToSnap.transform.position = objectToSnapTo.transform.position;
            objectToSnap.transform.rotation = objectToSnapTo.transform.rotation;
            snapped = !Input.GetButtonDown("Jump");
            yield return null;
        }
        Unsnap();
    }

    private void Unsnap() {
        objectToSnap.transform.eulerAngles = objectToSnapTo.transform.eulerAngles + Vector3.forward * 60;
        snapped = false;
        if (OnUnsnap != null) {
            OnUnsnap.Invoke();
        }
        finishedUnsnapping = true;
    }

    private void SpawnParticles() {
        Destroy(Instantiate(particles, objectToSnap.transform.position, objectToSnap.transform.rotation), 1.5f);
    }

    public void PlayerRespawning() {
        objectToSnap.transform.rotation = Quaternion.identity;
        snapped = false;
    }

    public void OnDestroy() {
        objectToSnap.OnRespawn -= PlayerRespawning;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + offset, detectionRadius);
    }
}
