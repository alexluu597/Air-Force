using UnityEngine;
using UnityEngine.UI;

public class HudScore : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    private void Update()
    {
        GetComponent<Text>().text = "Score: " + gameState.score;
    }
}
