using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public Animator transistionAnim;
    public string sceneName;
    public void SwitchScene() {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene() {
        transistionAnim.SetTrigger("end");
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(sceneName);
    }
}
