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

    public void Respawn() {
        StartCoroutine(LoadRespawn());
    }

    IEnumerator LoadScene() {
        transistionAnim.SetTrigger("end");
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadRespawn() {
        transistionAnim.SetTrigger("end");
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
