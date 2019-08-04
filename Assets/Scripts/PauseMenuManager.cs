using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseCanvas;
    public MenuButton[] buttons;

    private int currentIndex = 0;

    private float inputDelay = 0.3f;
    private float lastInputTime;

    public bool isPaused {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start() {
        buttons[0].OnSelectedButton();
        pauseCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            if (!isPaused) {
                Time.timeScale = 0;
                pauseCanvas.SetActive(true);
                isPaused = true;
            } else {
                Unpause();
            }
        }
        if (buttons == null || !isPaused)
            return;
        float input = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(input) > 0.1f && Time.realtimeSinceStartup - lastInputTime > inputDelay) {
            lastInputTime = Time.realtimeSinceStartup;
            int newIndex = (input < 0) ? currentIndex + 1 : currentIndex - 1;
            if (newIndex >= buttons.Length) {
                newIndex = 0;
            }
            if (newIndex < 0) {
                newIndex = buttons.Length - 1;
            }
            ChangeSelection(buttons[currentIndex], buttons[newIndex]);
            currentIndex = newIndex;
        }
        if (Input.GetButtonDown("Submit")) {
            AudioManager.instance.Play("ui_select");
            buttons[currentIndex].OnPressedButton();
        }
    }

    private void ChangeSelection(MenuButton old, MenuButton current) {
        AudioManager.instance.Play("ui_change");
        old.OnUnselectedButton();
        current.OnSelectedButton();
    }

    public void Unpause() {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void BackToMainMenu() {
        Time.timeScale = 1;
        StartCoroutine(LoadScene("MainMenu", 0.5f));
    }

    private IEnumerator LoadScene(string sceneName, float delay = 1) {
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
