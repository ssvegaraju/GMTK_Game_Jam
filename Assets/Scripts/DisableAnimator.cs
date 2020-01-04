using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DisableAnimator : MonoBehaviour
{
    public float delay = 3.1f;

    private Animator anim;
    private CameraFollow cam;
    private PlayerMovement player;

    private void Start() {
        anim = GetComponent<Animator>();
        cam = GetComponent<CameraFollow>();
        player = FindObjectOfType<PlayerMovement>();
        player.enabled = false;
        StartCoroutine(Disable());
    }
    
    IEnumerator Disable() {
        yield return new WaitForSeconds(3);
        anim.enabled = false;
        cam.enabled = true;
        player.enabled = true;
    }

    public void EndGameCutscene() {
        cam.enabled = false;
        player.enabled = false;
        anim.enabled = true;
        anim.SetTrigger("BossDefeated");
        StartCoroutine(ReEnableMovement());
    }

    IEnumerator ReEnableMovement() {
        yield return new WaitForSeconds(3);
        anim.enabled = false;
        cam.enabled = true;
        player.enabled = true;
    }
}
