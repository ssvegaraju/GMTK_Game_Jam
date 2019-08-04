using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DisableAnimator : MonoBehaviour
{
    public float delay = 3.1f;

    private Animator anim;
    private CameraFollow cam;

    private void Start() {
        anim = GetComponent<Animator>();
        cam = GetComponent<CameraFollow>();
        StartCoroutine(Disable());
    }
    
    IEnumerator Disable() {
        yield return new WaitForSeconds(3);
        anim.enabled = false;
        cam.enabled = true;
    }
}
