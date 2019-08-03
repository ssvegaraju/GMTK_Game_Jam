using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    private Vector3 playerPos;

    private Vector3 originalPos;

    private bool reset = false;

    void Start()
    {
        playerPos = GameObject.Find("Player").transform.position + offset;
        originalPos = transform.position;
    }

    void FixedUpdate()
    {
        playerPos = GameObject.Find("Player").transform.position + offset;
        gameObject.GetComponent<MoveUpAndDown>().setOrigin(playerPos);
        transform.position = Vector3.Slerp(transform.position, playerPos, 2f * Time.deltaTime);
    }

    public void resetPos() {
        transform.position = originalPos;
    }
}
