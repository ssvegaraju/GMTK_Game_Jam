﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    private Vector3 pos1;
    private Vector3 pos2;
    public float distance = 1;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pos1 = new Vector3(transform.position.x, -distance, transform.position.z);
        pos2 = new Vector3(transform.position.x, distance, transform.position.z);
        transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
}
