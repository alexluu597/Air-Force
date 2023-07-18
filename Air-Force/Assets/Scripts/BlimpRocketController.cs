using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlimpRocketController : MonoBehaviour
{
    GameState gameState;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public float maxHp;
    public float angleChangingSpeed;
    public float movementSpeed;
    public GameObject target;
    public Transform[] explosionPrefabs;

    protected const float MaxHomingTimerMillis = 4f;
    protected const float MaxDpsTimerMillis = 0.1f;
    protected const float MaxDamageTimerMillis = 0.1f;

    protected float homingTimerMillis;
    protected float homingHp;
    protected Rigidbody2D rigidBody;
    protected float dpsTimerMillis;
    protected float damageTimerMillis;
    protected Color damageColor;

    protected float bulletDamage, miniBulletDamage, bombDamage, bombExplosionDamage, missileDamage,
        flameDamage, smokeBombDamage, smokeDamage, boomerangDamage, blackHoleDamage;
    protected float slowed;

    protected bool hasTakenDamage;
    protected bool canTakeDamage;
    private void Start()
    {
        homingHp = maxHp;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        slowed = 1;
        movementSpeed *= slowed;
        if (gameState.upgradeLevel[2] == 0)
        {
            bulletDamage = 1;
        }
        else if (gameState.upgradeLevel[2] == 1)
        {
            bulletDamage = 1.4f;
        }
        else if (gameState.upgradeLevel[2] == 2)
        {
            bulletDamage = 2f;
        }
        else if (gameState.upgradeLevel[2] == 3)
        {
            bulletDamage = 3f;
        }

        if (gameState.upgradeLevel[3] == 0)
        {
            bombDamage = 1;
            bombExplosionDamage = 1;
        }
        else if (gameState.upgradeLevel[3] >= 1)
        {
            bombDamage = 2;
            bombExplosionDamage = 2;
        }

        if (gameState.upgradeLevel[4] == 0)
        {
            missileDamage = 1;
        }
        else if (gameState.upgradeLevel[4] == 1)
        {
            missileDamage = 1.2f;
        }
        else if (gameState.upgradeLevel[4] == 2)
        {
            missileDamage = 1.6f;
        }
        else if (gameState.upgradeLevel[4] == 3)
        {
            missileDamage = 2.2f;
        }

        if (gameState.upgradeLevel[5] == 0)
        {
            flameDamage = 1;
        }
        else if (gameState.upgradeLevel[5] >= 2)
        {
            flameDamage = 2;
        }

        if (gameState.upgradeLevel[6] == 0)
        {
            smokeBombDamage = 1;
            smokeDamage = 1;
        }
        else if (gameState.upgradeLevel[6] >= 2)
        {
            smokeBombDamage = 2;
            smokeDamage = 2;
        }

        if (gameState.upgradeLevel[7] == 0)
        {
            boomerangDamage = 1;
        }
        else if (gameState.upgradeLevel[7] >= 2)
        {
            boomerangDamage = 1.33f;
        }

        if(gameState.upgradeLevel[7] == 3)
        {
            boomerangDamage = 750;
        }

        if (gameState.upgradeLevel[8] == 0)
        {
            blackHoleDamage = 0;
        }
        else if (gameState.upgradeLevel[8] == 3)
        {
            blackHoleDamage = 5;
        }

        if (gameState.upgradeLevel[12] == 0)
        {
            miniBulletDamage = 1;
        }
        else if (gameState.upgradeLevel[12] == 1)
        {
            miniBulletDamage = 1.2f;
        }
        else if (gameState.upgradeLevel[12] == 2)
        {
            miniBulletDamage = 1.6f;
        }
        else if (gameState.upgradeLevel[12] == 3)
        {
            miniBulletDamage = 2.2f;
        }
    }

    private void Update()
    {
        homingTimerMillis += Time.deltaTime;
        if(homingTimerMillis >= MaxHomingTimerMillis)
        {
            this.gameObject.SetActive(false);
        }
        if (hasTakenDamage)
        {
            damageTimerMillis += Time.deltaTime;
            damageColor = new Color(255, 0, 0);
            spriteRenderer.color = damageColor;
            if (damageTimerMillis >= MaxDamageTimerMillis)
            {
                damageColor = new Color(255, 255, 255);
                spriteRenderer.color = damageColor;
                hasTakenDamage = false;
                damageTimerMillis = 0;
            }
        }
        if (!canTakeDamage)
        {
            dpsTimerMillis += Time.deltaTime;
            if (dpsTimerMillis >= MaxDpsTimerMillis)
            {
                canTakeDamage = true;
                dpsTimerMillis = 0;
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.transform.position - rigidBody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
        rigidBody.velocity = -transform.up * movementSpeed;
    }

    private void CheckHit(Collider2D collider, string name, float hp)
    {
        if (collider.CompareTag(name))
        {
            hasTakenDamage = true;
            homingHp -= hp;
            collider.gameObject.SetActive(false);
            if (homingHp <= 0)
            {
                Transform explosion = Instantiate(explosionPrefabs[0]);
                explosion.position = this.transform.position;
                collider.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }

    private void CheckHitStay(Collider2D collider, string name, float hp)
    {
        if (canTakeDamage)
        {
            if (collider.CompareTag(name))
            {
                hasTakenDamage = true;
                canTakeDamage = false;
                homingHp -= hp;
                if (homingHp <= 0)
                {
                    Transform explosion = Instantiate(explosionPrefabs[0]);
                    explosion.position = this.transform.position;
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    private void CheckHitExplosives(Collider2D collider, Transform explosives, string name, float hp)
    {
        if (collider.CompareTag(name))
        {
            Transform projectileExplosion = Instantiate(explosives);
            projectileExplosion.position = collider.transform.position;
            hasTakenDamage = true;
            homingHp -= hp;
            collider.gameObject.SetActive(false);
            if (homingHp <= 0)
            {
                Transform explosion = Instantiate(explosionPrefabs[0]);
                explosion.position = this.transform.position;
                collider.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CheckHit(collider, "Bullet", 5 * bulletDamage);
        CheckHit(collider, "MiniBullet", 2.5f);
        CheckHitExplosives(collider, explosionPrefabs[1], "Bomb", 15 * bombDamage);
        CheckHitStay(collider, "BombExplosion", 10 * bombExplosionDamage);
        CheckHit(collider, "Missile", 2.5f * missileDamage);
        CheckHitStay(collider, "Flame", 1);
        CheckHitExplosives(collider, explosionPrefabs[2], "SmokeBomb", 10);
        CheckHitStay(collider, "Smoke", 5);
        CheckHitStay(collider, "Boomerang", 7.5f);
        CheckHitExplosives(collider, explosionPrefabs[3], "BlackHoleProjectile", 0);
        CheckHit(collider, "LaserLevel01", 2);
        CheckHit(collider, "LaserLevel02", 8);
        CheckHitStay(collider, "LaserLevel03", 8);
        CheckHitStay(collider, "LaserLevel04", 8);
        CheckHitStay(collider, "LaserLevel05", 4);
        if (gameState.hasGainedPowerUps[1])
        {
            if (collider.CompareTag("SlowArea"))
            {
                damageColor = new Color(0, 255, 255);
                spriteRenderer.color = damageColor;
                if (gameState.upgradeLevel[11] == 0)
                {
                    slowed = 0.9f;
                }
                if (gameState.upgradeLevel[11] == 1)
                {
                    slowed = 0.8f;
                }
            }

            if (gameState.upgradeLevel[11] == 2)
            {
                CheckHitStay(collider, "SlowArea", 5);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        CheckHitStay(collider, "Flame", 1);
        CheckHitStay(collider, "Smoke", 2);
        CheckHitStay(collider, "LaserLevel05", 4);
        if (gameState.hasGainedPowerUps[1])
        {
            if (collider.CompareTag("SlowArea"))
            {
                damageColor = new Color(0, 255, 255);
                spriteRenderer.color = damageColor;
                if (gameState.upgradeLevel[11] == 0)
                {
                    slowed = 0.9f;
                }
                if (gameState.upgradeLevel[11] == 1)
                {
                    slowed = 0.8f;
                }
            }

            if (gameState.upgradeLevel[11] == 2)
            {
                CheckHitStay(collider, "SlowArea", 5);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (gameState.hasGainedPowerUps[1])
        {
            if (collider.CompareTag("SlowArea"))
            {
                damageColor = new Color(255, 255, 255);
                spriteRenderer.color = damageColor;
                slowed = 1f;
            }
        }
    }
}
