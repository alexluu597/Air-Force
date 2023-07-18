using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class BlackHoleController : MonoBehaviour
{
    GameState gameState;

    [SerializeField] public float GRAVITY_PULL = .78f;
    public static float m_GravityRadius = 1f;
    public float MaxExplosionTimerMillis;
    protected float explosionTimerMillis;
    void Awake()
    {
        m_GravityRadius = GetComponent<CircleCollider2D>().radius;
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    private void Start()
    {
        if(gameState.upgradeLevel[8] >= 2)
        {
            this.transform.localScale = new Vector3(3, 3, 3);
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
    /// <summary>
    /// Attract objects towards an area when they come within the bounds of a collider.
    /// This function is on the physics timer so it won't necessarily run every frame.
    /// </summary>
    /// <param name="other">Any object within reach of gravity's collider</param>
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.attachedRigidbody && other.CompareTag("EnemyPlanes")
            || other.attachedRigidbody && other.CompareTag("EnemyJets")
            || other.attachedRigidbody && other.CompareTag("EnemyBlimps")
            || other.attachedRigidbody && other.CompareTag("BlimpRocket")
            || other.attachedRigidbody && other.CompareTag("EnemyKamikazes")
            || other.attachedRigidbody && other.CompareTag("EnemyUfos"))
        {
            float gravityIntensity = Vector3.Distance(transform.position, other.transform.position) / m_GravityRadius;
            other.attachedRigidbody.AddForce((transform.position - other.transform.position) * gravityIntensity * other.attachedRigidbody.mass * GRAVITY_PULL * Time.smoothDeltaTime);
            Debug.DrawRay(other.transform.position, transform.position - other.transform.position);
        }
    }
}
