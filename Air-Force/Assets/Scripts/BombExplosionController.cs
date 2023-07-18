using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionController : MonoBehaviour
{
    GameState gameState;
    CircleCollider2D collider;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        collider = GetComponent<CircleCollider2D>();
    }

    ParticleSystem shockWave;
    ParticleSystem fireBall;
    ParticleSystem smoke;

    public float MaxExplosionTimerMillis;
    protected float explosionTimerMillis;

    private void Start()
    {
        shockWave = this.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        fireBall = this.gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        smoke = this.gameObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        if (gameState.upgradeLevel[3] == 3)
        {
            collider.radius = 5;
            shockWave.startSize = 12;

            float fireBallConstant = Random.Range(7.6f, 8.6f);
            float smokeConstant = Random.Range(9.07f, 10.09f);

            fireBall.startSize = fireBallConstant;
            smoke.startSize = smokeConstant;
        }
    }

    void Update()
    {
        explosionTimerMillis += Time.deltaTime;
        if(explosionTimerMillis >= MaxExplosionTimerMillis)
        {
            this.gameObject.SetActive(false);
        }
    }
}
