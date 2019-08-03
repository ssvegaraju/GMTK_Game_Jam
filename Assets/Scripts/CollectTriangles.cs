using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollectTriangles : MonoBehaviour
{
    private int collectedAmt;
    public float rotateSpeed = 2f;

    public void Increment() {
        collectedAmt++;
    }

    public void Decrement() {
        collectedAmt--;
    }

    public void TestAmt() {
        KeyFollow[] keys = FindObjectsOfType<KeyFollow>();
        foreach (KeyFollow k in keys) {
            k.enabled = false;
        }
        if (collectedAmt > 3) {
            StartCoroutine(UnlockDoor(keys.Select(x => x.transform).ToArray()));
        }
    }

    private IEnumerator UnlockDoor(Transform[] keys) {
        Transform parent = new GameObject("Empty Parent").transform;
        parent.parent = transform;
        parent.localPosition = Vector3.zero;
        parent.rotation = Quaternion.identity;
        foreach (Transform key in keys) {
            key.parent = parent.transform;
            while (Vector3.Distance(key.position, transform.position) > 1f) {
                parent.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);
                Vector3 dirToParent = (parent.position - key.position).normalized;
                key.Translate(dirToParent * Time.deltaTime * 8);
                yield return null;
            }
            Destroy(key.gameObject);
            parent.rotation = Quaternion.identity;
        }
        CameraFollow.instance.ShakeScreen(3f, .7f);
        gameObject.GetComponent<MoveDown>().enabled = true;
        gameObject.GetComponent<MoveDown>().SetAnimatorTrigger("Opened");
    }
}
