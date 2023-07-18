using UnityEngine;
using UnityEngine.UI;

public class HudHighScore : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    private void Update()
    {
        GetComponent<Text>().text = "High Score: " + gameState.highScore;
    }
}
