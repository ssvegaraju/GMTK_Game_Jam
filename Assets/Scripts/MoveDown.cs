using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -.05f, 0);
    }

    public void SetAnimatorTrigger(string trigger) {
        anim.SetTrigger(trigger);
    }
}
