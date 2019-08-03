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

    private CircleCollider2D col;
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
        objectToSnap.OnRespawn += Unsnap;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!snapped && collision.gameObject.CompareTag("Player")) {
            Snap();
        }
    }

    private void Snap() {
        if (snapped)
            return;
        snapped = true;
        if (OnSnap != null) {
            OnSnap.Invoke();
        }
        Debug.Log("Snapped");
        StartCoroutine(WhileSnapping());
    }

    private IEnumerator WhileSnapping() {
        float startTime = Time.time;
        while (snapped || Time.time - startTime < 0.5f) {
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
    }

    private void SpawnParticles() {
        Destroy(Instantiate(particles, objectToSnap.transform.position, objectToSnap.transform.rotation), 1.5f);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + offset, detectionRadius);
    }
}
