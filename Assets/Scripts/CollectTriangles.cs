using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTriangles : MonoBehaviour
{
    private int collectedAmt;

    public void Increment() {
        collectedAmt++;
        Debug.Log(collectedAmt);
        if (collectedAmt > 3) {
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().ShakeScreen(3f, .7f);
            Object.Destroy(this.gameObject);
        }
    }
}
