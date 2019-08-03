using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement pm;

    private Vector3 startpos;
    private void Awake() {
        anim = GetComponent<Animator>();
        pm = FindObjectOfType<PlayerMovement>();
        pm.OnRespawn += ResetOnRespawn;
        startpos = transform.position;
        enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -.07f, 0);
    }

    private void ResetOnRespawn() {
        enabled = false;
        transform.position = startpos;
        anim.SetBool("Opened", false);
    }

    public void SetAnimatorTrigger(string trigger) {
        CameraFollow.instance.ShakeScreen(0.1f, 0.1f);
        if (anim == null)
            anim = GetComponent<Animator>();
        anim.SetBool("Opened", true);
    }
}
