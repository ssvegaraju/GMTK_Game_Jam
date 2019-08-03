using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D rigid;
    private PlayerMovement pm;
    private Camera cam;

    public static CameraFollow instance;
	public Vector3 startTransform;

    public Vector3 offset;
    [Range(0f, 1f)]
    public float followSmoothing;

    public Vector2 minMaxCameraZoom = new Vector2(5, 10);

    private bool initialized = false;

    void Awake(){
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        startTransform = transform.position;
        pm = FindObjectOfType<PlayerMovement>();
        if (target == null) {
            target = pm.transform;
        } 
        rigid = target.GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
        initialized = true;
	}

    public void ShakeScreen(float t, float strength){
        startTransform = transform.position;
        StartCoroutine(ScreenShake(t, strength));
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, followSmoothing);
        startTransform = transform.position;
        if (initialized) {
            cam.orthographicSize = Mathf.MoveTowards(minMaxCameraZoom.x, minMaxCameraZoom.y, 
                Time.fixedDeltaTime * rigid.velocity.sqrMagnitude / (pm.moveSpeed));
        }
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
