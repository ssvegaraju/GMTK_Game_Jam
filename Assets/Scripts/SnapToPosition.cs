using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapToPosition : MonoBehaviour
{
    public PlayerMovement objectToSnap;
    public GameObject objectToSnapTo;
    public static bool snapped = false;
    private float vertical;

    public UnityEvent OnSnap, OnUnsnap;

    private float snapTime, releaseTime;
    // Start is called before the first frame update
    void Start()
    {
        if (objectToSnap == null)
            objectToSnap = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");

        float distance = Vector2.Distance(objectToSnap.transform.position, gameObject.transform.position);
        if (distance < 0.5f && !snapped && Time.time - releaseTime > 0.3f)
        {
            snapped = true;
            snapTime = Time.time;
            if (OnSnap != null) {
                OnSnap.Invoke();
            }
        }

        if (snapped)
        {
            if (Time.time - snapTime <= 0.3f || vertical < 0.1f)
            {
                objectToSnap.transform.position = objectToSnapTo.transform.position;
                objectToSnap.transform.rotation = objectToSnapTo.transform.rotation;
            }
            else 
            {
                float dir = objectToSnapTo.transform.eulerAngles.z + 60;
                objectToSnap.Unsnap(dir);
                releaseTime = Time.time;
                snapped = false;
                if (OnUnsnap != null) {
                    OnUnsnap.Invoke();
                }
            }
        }
    }
}
