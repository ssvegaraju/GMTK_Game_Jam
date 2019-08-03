﻿using System.Collections;
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
    private float vertical;


    public UnityEvent OnSnap, OnUnsnap;

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
        OnUnsnap.AddListener(delegate () {
            objectToSnap.OnUnsnap();
        });
    }

    private void Update() {
        vertical = Input.GetAxis("Vertical");
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
            snapped = vertical < 0.1f;
            yield return null;
        }
        Unsnap();
    }

    private void Unsnap() {
        float dir = objectToSnapTo.transform.eulerAngles.z + 60;
        objectToSnap.transform.eulerAngles = Vector3.forward * dir;
        objectToSnap.transform.Translate(objectToSnap.transform.up.normalized * (detectionRadius + 0.3f));
        snapped = false;
        if (OnUnsnap != null) {
            OnUnsnap.Invoke();
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + offset, detectionRadius);
    }
}
