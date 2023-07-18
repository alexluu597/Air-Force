using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public static bool gameIsPaused;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }
    void PauseGame()
    {
        if (gameIsPaused)
        {
            gameState.message = "PAUSE";
            Time.timeScale = 0f;
        }
        else
        {
            gameState.message = "";
            Time.timeScale = 1f;
        }
    }
}
