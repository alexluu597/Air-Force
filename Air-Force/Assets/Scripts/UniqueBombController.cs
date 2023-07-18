using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueBombController : MonoBehaviour
{
    public Transform explosionPrefab;

    public float maxExplosionTimerMillis;
    protected float explosionTimerMillis;
    private void Update()
    {
        explosionTimerMillis += Time.deltaTime;
        if (explosionTimerMillis >= maxExplosionTimerMillis)
        {
            Transform explosion = Instantiate(explosionPrefab);
            this.gameObject.SetActive(false);
            explosionTimerMillis = 0;
            explosion.position = this.transform.position;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            explosionTimerMillis = 0;
        }
        if (collider.CompareTag("UpWall") || collider.CompareTag("DownWall") || collider.CompareTag("LeftWall") || collider.CompareTag("RightWall"))
        {
            this.gameObject.SetActive(false);
        }

    }
}
