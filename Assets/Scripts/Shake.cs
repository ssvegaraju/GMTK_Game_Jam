using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shake : MonoBehaviour {

	public static Shake instance;
	public Vector3 startTransform;

    void Awake(){
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        startTransform = transform.position;
	}
	public void ShakeScreen(float t, float strength){
        StartCoroutine(ScreenShake(t, strength));
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
