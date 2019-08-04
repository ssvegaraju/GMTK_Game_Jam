using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftAndRight : MonoBehaviour
{
    private Vector3 pos1;
    private Vector3 pos2;

    private Vector3 startVector;
    public float distance = 1;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        startVector = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos1 = new Vector3(-distance + startVector.x, transform.position.y, transform.position.z);
        pos2 = new Vector3(distance + startVector.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * distance);
        Gizmos.DrawLine(transform.position, transform.position - Vector3.right * distance);
    }
}
