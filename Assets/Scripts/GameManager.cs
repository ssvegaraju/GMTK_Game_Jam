using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Start() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private Vector3 playerSpawnPosition;
    private PlayerMovement player;

    private static string current;

    private bool firstTime = true;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log(current + ", " + scene.name);
        player = FindObjectOfType<PlayerMovement>();
        if (player == null)
            return;
        firstTime = current != scene.name;
        if (firstTime) {
            playerSpawnPosition = player.transform.position;
            current = scene.name;
            firstTime = false;
            return;
        }
        player.transform.position = playerSpawnPosition;
    }

    IEnumerator OnSceneChange(Scene scene) {
        Debug.Log(current + ", " + scene.name);
        player = FindObjectOfType<PlayerMovement>();
        while (player == null) {
            player = FindObjectOfType<PlayerMovement>();
            yield return null;
        }
        firstTime = current != scene.name;
        if (firstTime) {
            playerSpawnPosition = player.transform.position;
            current = scene.name;
            firstTime = false;
        } else 
            player.transform.position = playerSpawnPosition;
    }

    public void UpdateSpawnPosition(Vector3 pos) {
        playerSpawnPosition = pos;
    }
}
