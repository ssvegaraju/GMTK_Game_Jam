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
	[HideInInspector] public Vector3 startTransform;
    public float followSmoothing;

    public Vector2 minMaxCameraZoom = new Vector2(5, 10);
    public float verticalOffset;
    public float lookAheadX;
    public float smoothTimeX;
    public float verticalSmoothTime;
    public Vector2 deadZoneSize;

    DeadZone deadZone;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool isLookingAhead;

    private bool initialized = false;

    void Awake(){
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
	}

    private void Start() {
        startTransform = transform.position;
        pm = FindObjectOfType<PlayerMovement>();
        if (target == null) {
            target = pm.transform;
        }
        rigid = target.GetComponent<Rigidbody2D>();
        cam = GetComponent<Camera>();
        initialized = true;
        deadZone = new DeadZone(pm.col.bounds, deadZoneSize);
    }

    public void ShakeScreen(float t, float strength){
        startTransform = transform.position;
        StartCoroutine(ScreenShake(t, strength));
	}

    // Update is called once per frame
    void LateUpdate()
    {
        deadZone.Update(pm.col.bounds);

        Vector2 focusPosition = deadZone.center + Vector2.up * verticalOffset;

        if (deadZone.velocity.x != 0) {
            lookAheadDirX = Mathf.Sign(deadZone.velocity.x);
            if (Mathf.Sign(rigid.velocity.x) == Mathf.Sign(deadZone.velocity.x) && rigid.velocity.x != 0) {
                isLookingAhead = false;
                targetLookAheadX = lookAheadDirX * lookAheadX;
            } else {
                if (!isLookingAhead) {
                    isLookingAhead = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadX - currentLookAheadX) / 4f;
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, smoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
        //if (initialized) {
        //    cam.orthographicSize = Mathf.MoveTowards(minMaxCameraZoom.x, minMaxCameraZoom.y, 
        //        Time.fixedDeltaTime * rigid.velocity.sqrMagnitude / (pm.moveSpeed));
        //}
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

    void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(deadZone.center, deadZoneSize);
    }

    struct DeadZone
    {
        public Vector2 center;
        public Vector2 velocity;
        float left, right;
        float top, bottom;


        public DeadZone(Bounds targetBounds, Vector2 size) {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds) {
            float shiftX = 0;
            if (targetBounds.min.x < left) {
                shiftX = targetBounds.min.x - left;
            } else if (targetBounds.max.x > right) {
                shiftX = targetBounds.max.x - right;
            }
            left += shiftX;
            right += shiftX;

            float shiftY = 0;
            if (targetBounds.min.y < bottom) {
                shiftY = targetBounds.min.y - bottom;
            } else if (targetBounds.max.y > top) {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
