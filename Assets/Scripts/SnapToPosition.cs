using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPosition : MonoBehaviour
{
    public GameObject objectToSnap;
    public GameObject objectToSnapTo;
    private bool snapped = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(objectToSnap.gameObject.transform.position, gameObject.transform.position);
        if (distance < 0.5f)
        {
            snapped = true;
            objectToSnap.transform.position = objectToSnapTo.transform.position;
            objectToSnap.transform.rotation = objectToSnapTo.transform.rotation;
        }

        if (snapped)
        {
            objectToSnap.transform.position = objectToSnapTo.transform.position;
            objectToSnap.transform.rotation = objectToSnapTo.transform.rotation;
        }
    }
}
