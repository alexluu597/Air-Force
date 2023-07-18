using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public Transform explosionPrefab;

    protected float maxExplosionTimerMillis;
    protected float explosionTimerMillis;

    private void Start()
    {
        if(gameState.upgradeLevel[3] < 2)
        {
            maxExplosionTimerMillis = 1;
        }
        if (gameState.upgradeLevel[3] >= 2)
        {
            maxExplosionTimerMillis = 2;
        }
    }

    private void Update()
    {
        explosionTimerMillis += Time.deltaTime;
        if(explosionTimerMillis >= maxExplosionTimerMillis)
        {
            Transform explosion = Instantiate(explosionPrefab);
            explosion.position = this.transform.position;
            this.gameObject.SetActive(false);
            explosionTimerMillis = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Enemy"))
        {
            explosionTimerMillis = 0;
        }
        if (collider.CompareTag("UpWall") || collider.CompareTag("DownWall") || collider.CompareTag("LeftWall") || collider.CompareTag("RightWall"))
        {
            this.gameObject.SetActive(false);
        }

    }
}
