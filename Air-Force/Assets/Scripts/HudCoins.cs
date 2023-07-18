using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudCoins : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    private void Update()
    {
        GetComponent<Text>().text = "Coins: " + gameState.coins;
    }
}
