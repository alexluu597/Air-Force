using System;
using UnityEngine;
using UnityEngine.UI;

public class HudHp : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    private void Update()
    {
        GetComponent<Text>().text = "Hp: " + Math.Round(gameState.playerHp, 2);
    }
}
