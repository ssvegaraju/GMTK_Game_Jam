﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    private Vector3 pos1;
    private Vector3 pos2;

    private Vector3 startVector;

    private Vector3 originalPos;
    public float distance = 1;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        startVector = transform.position;
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos1 = new Vector3(transform.position.x, -distance + startVector.y, transform.position.z);
        pos2 = new Vector3(transform.position.x, distance + startVector.y, transform.position.z);
        transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }

    public void setOrigin(Vector3 vect) {
        startVector = vect;
    }

    public void setOriginal() {
        startVector = originalPos;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * distance);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.up * distance);
    }
}
