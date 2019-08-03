using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public static CameraFollow instance;
	public Vector3 startTransform;

    public Vector3 offset;
    [Range(0f, 1f)]
    public float followSmoothing;

    public Vector2 minMaxCameraZoom;

    private bool stopFollowing = false;

    void Awake(){
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        startTransform = transform.position;
	}

    public void ShakeScreen(float t, float strength){
        stopFollowing = true;
        startTransform = transform.position;
        StartCoroutine(ScreenShake(t, strength));
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (!stopFollowing) {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, followSmoothing);
            startTransform = transform.position;
       // }
    }

    IEnumerator ScreenShake(float t, float strength){
		float z = transform.position.z;
		while (t > 0){
			t -= Time.deltaTime*10;
			
			transform.position = new Vector2(startTransform.x,startTransform.y) + Random.insideUnitCircle*strength/8;
			transform.position += new Vector3(0,0,z);
			yield return null;
		}
		transform.position = startTransform;
	}
}
