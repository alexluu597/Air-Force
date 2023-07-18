using UnityEngine;
using UnityEngine.UI;

public class HudEnemiesDefeated : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    private void Update()
    {
        GetComponent<Text>().text = "Enemies Defeated: " + gameState.enemiesDefeated;
    }
}
