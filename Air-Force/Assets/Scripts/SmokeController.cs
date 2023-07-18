using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    GameState gameState;

    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public float MaxExplosionTimerMillis;
    protected float explosionTimerMillis;
    private void Start()
    {
        if(gameState.upgradeLevel[6] == 3)
        {
            MaxExplosionTimerMillis = 8;
        }
    }

    void Update()
    {
        explosionTimerMillis += Time.deltaTime;
        if (explosionTimerMillis >= MaxExplosionTimerMillis)
        {
            this.gameObject.SetActive(false);
        }
    }
}
