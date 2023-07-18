using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameState gameState;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public Transform[] explosionPrefabs;

    public GameObject[] coinsPrefabs;
    public GameObject[] ammoPrefabs;
    public GameObject[] powerUpPrefabs;

    public float speed;
    public float scored;
    public float enemyHp;

    protected const float MaxDpsTimerMillis = 0.1f;
    protected const float MaxDamageTimerMillis = 0.1f;

    protected Color damageColor;
    protected float bulletDamage, miniBulletDamage, bombDamage, bombExplosionDamage, missileDamage, 
        flameDamage, smokeBombDamage, smokeDamage, boomerangDamage, blackHoleDamage;
    protected float slowed;
    protected float dpsTimerMillis;
    protected float damageTimerMillis;
    protected bool hasTakenDamage;
    protected bool canTakeDamage;

    private void Start()
    {
        hasTakenDamage = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
        slowed = 1;
        speed *= slowed;
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

        if (gameState.upgradeLevel[8] == 0)
        {
            blackHoleDamage = 0;
        }
        else if (gameState.upgradeLevel[8] == 3)
        {
            blackHoleDamage = 5;
        }

        if(gameState.upgradeLevel[12] == 0)
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
        float yTransition = speed * Time.deltaTime;
        transform.Translate(new Vector3(0, yTransition, 0), Space.Self);
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
        if(!canTakeDamage)
        {
            dpsTimerMillis += Time.deltaTime;
            if(dpsTimerMillis >= MaxDpsTimerMillis)
            {
                canTakeDamage = true;
                dpsTimerMillis = 0;
            }
        }
    }

    private void SpawnRegularCoins(float probability, int index)
    {
        float numCoins = Random.Range(1, probability);

        for (int i = 0; i < numCoins; i++)
        {
            float spawnPosX = Random.Range(-2.0f, 2.0f);
            float spawnPosY = Random.Range(-2.0f, 2.0f);
            GameObject coins = Instantiate(coinsPrefabs[index]);
            coins.transform.position = this.transform.position + (Vector3.right * spawnPosX) + (Vector3.up * spawnPosY);
        }
    }

    private void SpawnRareCoins(float probability, int index)
    {
        float dropChance = Random.Range(1, probability);

        if (dropChance == 1)
        {
            float spawnPosX = Random.Range(-2.0f, 2.0f);
            float spawnPosY = Random.Range(-2.0f, 2.0f);
            GameObject coins = Instantiate(coinsPrefabs[index]);
            coins.transform.position = this.transform.position + (Vector3.right * spawnPosX) + (Vector3.up * spawnPosY);
        }
    }

    private void SpawnAmmo(int probability, int index)
    {
        float dropChance = Random.Range(1, probability);
        if (dropChance == 1)
        {
            float spawnPosX = Random.Range(-2.0f, 2.0f);
            float spawnPosY = Random.Range(-2.0f, 2.0f);
            GameObject ammos = Instantiate(ammoPrefabs[index]);
            ammos.transform.position = this.transform.position + (Vector3.right * spawnPosX) + (Vector3.up * spawnPosY);
        }
    }

    private void SpawnPowerUp(int probability)
    {
        float dropChance = Random.Range(1, probability);
        if (dropChance == 1)
        {
            float powerUpIndex = Random.Range(0, powerUpPrefabs.Length);
            float spawnPosX = Random.Range(-2.0f, 2.0f);
            float spawnPosY = Random.Range(-2.0f, 2.0f);
            GameObject powerUps = Instantiate(powerUpPrefabs[(int)powerUpIndex]);
            powerUps.transform.position = this.transform.position + (Vector3.right * spawnPosX) + (Vector3.up * spawnPosY);
        }
    }

    private void SpawnItems()
    {
        SpawnRegularCoins(5, 0);
        SpawnRegularCoins(3, 1);
        SpawnRegularCoins(2, 2);
        SpawnRareCoins(5, 3);
        SpawnRareCoins(10, 4);
        SpawnRareCoins(50, 5);
        SpawnRareCoins(100, 6);
        SpawnAmmo(5, 0);
        SpawnAmmo(7, 1);
        SpawnAmmo(10, 2);
        SpawnAmmo(12, 3);
        SpawnAmmo(15, 4);
        SpawnPowerUp(10);
    }

    private void CheckHit(Collider2D collider, string name, float hp)
    {
        if (collider.CompareTag(name))
        {
            hasTakenDamage = true;
            enemyHp -= hp;
            collider.gameObject.SetActive(false);
            if (enemyHp <= 0)
            {
                Transform explosion = Instantiate(explosionPrefabs[0]);
                explosion.position = this.transform.position;
                collider.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                gameState.score += scored;
                gameState.enemiesDefeated++;
                SpawnItems();
            }
        }
    }

    private void CheckHitStay(Collider2D collider, string name, float hp)
    {
        if(canTakeDamage)
        {
            if (collider.CompareTag(name))
            {
                hasTakenDamage = true;
                canTakeDamage = false;
                enemyHp -= hp;
                if (enemyHp <= 0)
                {
                    Transform explosion = Instantiate(explosionPrefabs[0]);
                    explosion.position = this.transform.position;
                    this.gameObject.SetActive(false);
                    gameState.score += scored;
                    gameState.enemiesDefeated++;
                    SpawnItems();
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
            enemyHp -= hp;
            collider.gameObject.SetActive(false);
            if (enemyHp <= 0)
            {
                Transform explosion = Instantiate(explosionPrefabs[0]);
                explosion.position = this.transform.position;
                collider.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                gameState.score += scored;
                gameState.enemiesDefeated++;
                SpawnItems();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CheckHit(collider, "Bullet", 5 * bulletDamage);
        CheckHit(collider, "MiniBullet", 2.5f * miniBulletDamage);
        CheckHitExplosives(collider, explosionPrefabs[1], "Bomb", 15 * bombDamage);
        CheckHitStay(collider, "BombExplosion", 10 * bombExplosionDamage);
        CheckHit(collider, "Missile", 2.5f * missileDamage);
        CheckHitStay(collider, "Flame", 1 * flameDamage);
        CheckHitExplosives(collider, explosionPrefabs[2], "SmokeBomb", 10 * smokeBombDamage);
        CheckHitStay(collider, "Smoke", 5 * smokeDamage);
        CheckHitStay(collider, "Boomerang", 7.5f * boomerangDamage);
        CheckHitExplosives(collider, explosionPrefabs[3], "BlackHoleProjectile", 0);
        CheckHitStay(collider, "BlackHole", blackHoleDamage);
        CheckHit(collider, "LaserLevel01", 2);
        CheckHit(collider, "LaserLevel02", 8);
        CheckHitStay(collider, "LaserLevel03", 8);
        CheckHitStay(collider, "LaserLevel04", 8);
        CheckHitStay(collider, "LaserLevel05", 4);
        if (gameState.hasGainedPowerUps[1])
        {
            if(collider.CompareTag("SlowArea"))
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
        CheckHitStay(collider, "BlackHole", blackHoleDamage);
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
