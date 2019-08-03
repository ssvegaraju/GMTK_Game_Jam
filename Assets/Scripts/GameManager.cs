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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    #endregion

    private Vector3 playerSpawnPosition;
    private PlayerMovement player;

    private string current;

    private bool firstTime = true;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        player = FindObjectOfType<PlayerMovement>();
        if (player == null)
            return;
        if (firstTime) {
            playerSpawnPosition = player.transform.position;
            current = scene.name;
            firstTime = false;
        }
        if (current.Equals(scene.name)) {
            player.transform.position = playerSpawnPosition;
        } else {
            playerSpawnPosition = player.transform.position;
        }
    }

    public void UpdateSpawnPosition(Vector3 pos) {
        playerSpawnPosition = pos;
        Debug.Log(playerSpawnPosition);
    }
}
