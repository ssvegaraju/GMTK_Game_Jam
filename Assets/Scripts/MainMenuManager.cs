﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public MenuButton[] buttons;

    private int currentIndex = 0;

    private float inputDelay = 0.3f;
    private float lastInputTime;

    // Start is called before the first frame update
    void Start() {
        if (!AudioManager.instance.IsPlaying("Chill")) {
            AudioManager.instance.StopAllSounds();
            AudioManager.instance.Play("Chill");
        }
        buttons[0].OnSelectedButton();
    }

    // Update is called once per frame
    void Update() {
        if (buttons == null)
            return;
        float input = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(input) > 0.1f && Time.time - lastInputTime > inputDelay) {
            lastInputTime = Time.time;
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

    public void StartNewGame() {
        StartCoroutine(LoadScene("Level 1"));
    }

    public void BackToMainMenu() {
        StartCoroutine(LoadScene("MainMenu"));
    }

    public void LoadNewScene(string sceneName) {
        StartCoroutine(LoadScene(sceneName, 0.5f));
    }

    private IEnumerator LoadScene(string sceneName, float delay = 1) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
