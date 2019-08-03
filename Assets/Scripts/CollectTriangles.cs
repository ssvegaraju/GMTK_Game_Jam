using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectTriangles : MonoBehaviour
{
    private int collectedAmt;

    public void Increment() {
        collectedAmt++;
        Debug.Log(collectedAmt);
        
    }

    public void Decrement() {
        collectedAmt--;
    }

    public void TestAmt() {
        if (collectedAmt > 3) {
            GameObject.Find("Main Camera").GetComponent<CameraFollow>().ShakeScreen(3f, .7f);
            Object.Destroy(GameObject.Find("Key"));
            Object.Destroy(GameObject.Find("Key 2"));
            Object.Destroy(GameObject.Find("Key 3"));
            Object.Destroy(GameObject.Find("Key 4"));
            gameObject.GetComponent<MoveDown>().enabled = true;
            gameObject.GetComponent<MoveDown>().SetAnimatorTrigger("Opened");
            
        }
    }
}
