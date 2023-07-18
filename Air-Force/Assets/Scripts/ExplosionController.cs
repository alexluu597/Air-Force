using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public float MaxExplosionTimerMillis;
    protected float explosionTimerMillis;
    void Update()
    {
        explosionTimerMillis += Time.deltaTime;
        if (explosionTimerMillis >= MaxExplosionTimerMillis)
        {
            this.gameObject.SetActive(false);
        }
    }
}
