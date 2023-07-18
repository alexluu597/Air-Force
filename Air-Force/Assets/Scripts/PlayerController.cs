using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    ParticleSystem slowParticleSystem;
    CircleCollider2D slowAreaCollider;
    SpriteRenderer spriteRenderer;
    GameState gameState;

    private void Awake()
    {
        slowParticleSystem = GameObject.FindGameObjectWithTag("SlowArea").GetComponent<ParticleSystem>();
        slowAreaCollider = GameObject.FindGameObjectWithTag("SlowArea").GetComponent<CircleCollider2D>();
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public Transform explosionPrefab;

    public GameObject bulletPrefab;
    public GameObject[] weaponPrefabs;
    public GameObject[] laserPrefabs;
    public GameObject[] powerUpPrefabs;
    public GameObject leftMiniPlane;
    public GameObject rightMiniPlane;

    public int[] maxAmmoCap;
    public float horizontalSpeed, verticalSpeed, boomerangPower;

    protected int maxWeaponType;

    protected const float maxDamageTimerMillis = 0.1f;

    protected float maxReloadBulletTimerMillis = 0.2f;
    protected float maxReloadBombTimerMillis = 1f;
    protected float maxReloadSmokeBombTimerMillis = 1f;
    protected float maxReloadBoomerangTimerMillis = 0.5f;
    protected float maxReloadBlackHoleTimerMillis = 20f;
    protected float maxReloadLaserTimerMillis = 25f;
    protected float maxHomingMissileTimerMillis = 2f;
    protected float maxHealingTimerMillis = 4f;
    protected float maxDepleteFlameAmmoTimerMillis = 1f;

    protected float maxLevel02ChargeTimerMillis = 1f;
    protected float maxLevel03ChargeTimerMillis = 2f;
    protected float maxLevel04ChargeTimerMillis = 4f;
    protected float maxLevel05ChargeTimerMillis = 8f;

    protected float reloadBulletTimerMillis, reloadBombTimerMillis,
        reloadSmokeBombTimerMillis, reloadBoomerangMillis, reloadBlackHoleTimerMillis, 
        reloadLaserTimerMillis, homingMissileTimerMillis, laserChargeTimerMillis, 
        healingTimerMillis, depleteFlameAmmoTimerMillis, damageTimerMillis;

    protected Color damageColor;
    protected float damageTaken;
    protected float heal;
    protected float maxHp;
    protected int weaponType;

    protected bool hasHitLeft, hasHitRight, hasHitUp, hasHitDown;

    protected int flameIndex, smokeIndex, boomerangIndex, blackHoleIndex, laserIndex;

    protected bool hasUnlockedFlame, hasUnlockedSmoke, hasUnlockedBoomerang, hasUnlockedBlackHole, hasUnlockedLaser;

    protected bool canShootBullet, canShootBomb, canSpawnFlame, 
        canShootFlames, isFlamesActive, canShootSmokeBomb, canShootBoomerang, 
        canShootBlackHole, canShootLaser, canSpawnPlane, hasTakenDamage;

    protected int miniPlaneCount, flameCount;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (gameState.upgradeLevel[0] == 0)
        {
            horizontalSpeed = 10;
            verticalSpeed = 10;
        }
        else if (gameState.upgradeLevel[0] == 1)
        {
            horizontalSpeed = 11;
            verticalSpeed = 11;
        }
        else if (gameState.upgradeLevel[0] == 2)
        {
            horizontalSpeed = 13;
            verticalSpeed = 13;
        }
        else if (gameState.upgradeLevel[0] == 3)
        {
            horizontalSpeed = 16;
            verticalSpeed = 16;
        }

        if (gameState.upgradeLevel[1] == 0)
        {
            maxHp = 100;
        }
        else if (gameState.upgradeLevel[1] == 1)
        {
            maxHp = 125;
        }
        else if (gameState.upgradeLevel[1] == 2)
        {
            maxHp = 150;
        }
        else if (gameState.upgradeLevel[1] == 3)
        {
            maxHp = 200;
        }

        if(gameState.upgradeLevel[9] >= 2)
        {
            maxLevel05ChargeTimerMillis = 6;
        }

        if(gameState.upgradeLevel[5] >= 1)
        {
            hasUnlockedFlame = true;
        }
        else
        {
            hasUnlockedFlame = false;
            flameIndex = 0;
        }

        if (gameState.upgradeLevel[6] >= 1)
        {
            hasUnlockedSmoke = true;
        }
        else
        {
            hasUnlockedSmoke = false;
            smokeIndex = 0;
        }

        if (gameState.upgradeLevel[7] >= 1)
        {
            hasUnlockedBoomerang = true;
        }
        else
        {
            hasUnlockedBoomerang = false;
            boomerangIndex = 0;
        }

        if (gameState.upgradeLevel[8] >= 1)
        {
            hasUnlockedBlackHole = true;
        }
        else
        {
            hasUnlockedBlackHole = false;
            blackHoleIndex = 0;
        }

        if (gameState.upgradeLevel[9] >= 1)
        {
            hasUnlockedLaser = true;
        }
        else
        {
            hasUnlockedLaser = false;
            laserIndex = 0;
        }

        gameState.playerHp = maxHp;
        reloadBulletTimerMillis = maxReloadBulletTimerMillis;
        reloadBombTimerMillis = maxReloadBombTimerMillis;
        reloadSmokeBombTimerMillis = maxReloadSmokeBombTimerMillis;
        reloadBoomerangMillis = maxReloadBoomerangTimerMillis;
        reloadBlackHoleTimerMillis = maxReloadBlackHoleTimerMillis;
        homingMissileTimerMillis = 0;
        depleteFlameAmmoTimerMillis = 0;
        damageTaken = 1;
        heal = 1;
        weaponType = 1;
        maxWeaponType = 1;
        miniPlaneCount = 0;
        flameCount = 0;
        canShootBomb = true;
        canShootFlames = true;
        canSpawnFlame = true;
        isFlamesActive = false;
        canShootSmokeBomb = true;
        canShootBoomerang = true;
        canShootBullet = true;
        canShootBlackHole = true;
        canShootLaser = true;
        canSpawnPlane = true;
        hasTakenDamage = false;
    }

    private void Update()
    {
        float xTransition = horizontalSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float yTransition = verticalSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        reloadBulletTimerMillis += Time.deltaTime;

        if (xTransition < 0 && !hasHitLeft)
        {
            transform.Translate(new Vector3(xTransition, 0, 0), Space.Self);
            hasHitRight = false;
            if(reloadBulletTimerMillis >= maxReloadBulletTimerMillis)
            {
                canShootBullet = true;
                if(canShootBullet)
                {
                    ShootBullet();
                    canShootBullet = false;
                    reloadBulletTimerMillis = 0;
                }
            }
        }

        if (xTransition > 0 && !hasHitRight)
        {
            transform.Translate(new Vector3(xTransition, 0, 0), Space.Self);
            hasHitLeft = false;
            if (reloadBulletTimerMillis >= maxReloadBulletTimerMillis)
            {
                canShootBullet = true;
                if (canShootBullet)
                {
                    ShootBullet();
                    canShootBullet = false;
                    reloadBulletTimerMillis = 0;
                }
            }
        }

        if (yTransition < 0 && !hasHitDown)
        {
            transform.Translate(new Vector3(0, yTransition, 0), Space.Self);
            hasHitUp = false;
            if (reloadBulletTimerMillis >= maxReloadBulletTimerMillis)
            {
                canShootBullet = true;
                if (canShootBullet)
                {
                    ShootBullet();
                    canShootBullet = false;
                    reloadBulletTimerMillis = 0;
                }
            }
        }

        if (yTransition > 0 && !hasHitUp)
        {
            transform.Translate(new Vector3(0, yTransition, 0), Space.Self);
            hasHitDown = false;
            if (reloadBulletTimerMillis >= maxReloadBulletTimerMillis)
            {
                canShootBullet = true;
                if (canShootBullet)
                {
                    ShootBullet();
                    canShootBullet = false;
                    reloadBulletTimerMillis = 0;
                }
            }
        }

        if(gameState.hasGainedPowerUps[0])
        {
            if(gameState.upgradeLevel[10] == 0)
            {
                damageTaken = 0.9f;
            }
            else if (gameState.upgradeLevel[10] == 1)
            {
                damageTaken = 0.85f;
            }
            else if (gameState.upgradeLevel[10] == 2)
            {
                damageTaken = 0.75f;
            }
            else if (gameState.upgradeLevel[10] == 3)
            {
                damageTaken = 0.6f;
            }
        }

        if(!gameState.hasGainedPowerUps[1])
        {
            slowParticleSystem.Play();
            if (gameState.upgradeLevel[11] == 0)
            {
                slowAreaCollider.radius = 5;
                slowParticleSystem.startLifetime = 3;
            }
            else if (gameState.upgradeLevel[11] == 3)
            {
                slowAreaCollider.radius = 10;
                slowParticleSystem.startLifetime = 7;
            }
        }

        if (gameState.hasGainedPowerUps[2])
        {
            if(canSpawnPlane)
            {
                GameObject firstPlane = Instantiate(leftMiniPlane);
                GameObject secondPlane = Instantiate(rightMiniPlane);
                firstPlane.transform.position = this.transform.position + (Vector3.right * -4.05f) + (Vector3.up * -0.5f);
                secondPlane.transform.position = this.transform.position + (Vector3.right * 4.05f) + (Vector3.up * -0.5f);
                miniPlaneCount = 1;
                if(miniPlaneCount == 1)
                {
                    canSpawnPlane = false;
                }
            }
        }

        if (gameState.hasGainedPowerUps[4])
        {
            if (gameState.upgradeLevel[14] == 3)
            {
                maxHealingTimerMillis = 2f;
            }
            healingTimerMillis += Time.deltaTime;

            if (healingTimerMillis >= maxHealingTimerMillis)
            {
                if (gameState.upgradeLevel[14] == 0)
                {
                    heal = 5f;
                }
                else if (gameState.upgradeLevel[14] == 1)
                {
                    heal = 10f;
                }
                else if (gameState.upgradeLevel[14] == 2)
                {
                    heal = 20f;
                }
                gameState.playerHp += heal;
                if (gameState.playerHp + heal > maxHp)
                {
                    gameState.playerHp = maxHp;
                }
                healingTimerMillis = 0f;
            }
        }
        homingMissileTimerMillis += Time.deltaTime;
        if(homingMissileTimerMillis >= maxHomingMissileTimerMillis)
        {
            GameObject projectile = Instantiate(weaponPrefabs[5]);

            projectile.transform.position = this.gameObject.transform.GetChild(0).transform.position;
            homingMissileTimerMillis = 0;
        }

        if(isFlamesActive)
        {
            depleteFlameAmmoTimerMillis += Time.deltaTime;
            if (depleteFlameAmmoTimerMillis >= maxDepleteFlameAmmoTimerMillis)
            {
                gameState.numAmmo[1]--;
                depleteFlameAmmoTimerMillis = 0;
            }
        }

        if(weaponType != flameIndex)
        {
            canShootFlames = true;
            canSpawnFlame = true;
            isFlamesActive = false;
            flameCount = 0;
        }

        if (hasTakenDamage)
        {
            damageTimerMillis += Time.deltaTime;
            damageColor = new Color(255, 0, 0);
            spriteRenderer.color = damageColor;
            if (damageTimerMillis >= maxDamageTimerMillis)
            {
                damageColor = new Color(255, 255, 255);
                spriteRenderer.color = damageColor;
                hasTakenDamage = false;
                damageTimerMillis = 0;
            }
        }

        if (!hasUnlockedFlame)
        {
            if(hasUnlockedSmoke)
            {
                maxWeaponType = 2;
                smokeIndex = 2;
                if (hasUnlockedBoomerang)
                {
                    maxWeaponType = 3;
                    boomerangIndex = 3;
                    if (hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if (hasUnlockedBlackHole)
                {
                    maxWeaponType = 3;
                    blackHoleIndex = 3;
                    if (hasUnlockedBoomerang)
                    {
                        maxWeaponType = 4;
                        boomerangIndex = 3;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedBoomerang)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if (hasUnlockedLaser)
                {
                    maxWeaponType = 3;
                    laserIndex = 3;
                    if (hasUnlockedBoomerang)
                    {
                        maxWeaponType = 4;
                        boomerangIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBoomerang)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
            }
            else if(hasUnlockedBoomerang)
            {
                maxWeaponType = 2;
                boomerangIndex = 2;
                if (hasUnlockedSmoke)
                {
                    maxWeaponType = 3;
                    smokeIndex = 2;
                    boomerangIndex = 3;
                    if (hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if (hasUnlockedBlackHole)
                {
                    maxWeaponType = 3;
                    blackHoleIndex = 3;
                    if (hasUnlockedSmoke)
                    {
                        maxWeaponType = 4;
                        smokeIndex = 2;
                        boomerangIndex = 3;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedSmoke)
                        {
                            maxWeaponType = 5;
                            smokeIndex = 2;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if (hasUnlockedLaser)
                {
                    maxWeaponType = 3;
                    laserIndex = 3;
                    if (hasUnlockedSmoke)
                    {
                        maxWeaponType = 4;
                        smokeIndex = 2;
                        boomerangIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedSmoke)
                        {
                            maxWeaponType = 5;
                            smokeIndex = 2;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
            }
            else if(hasUnlockedBlackHole)
            {
                maxWeaponType = 2;
                blackHoleIndex = 2;
                if (hasUnlockedBoomerang)
                {
                    maxWeaponType = 3;
                    boomerangIndex = 2;
                    blackHoleIndex = 3;
                    if (hasUnlockedSmoke)
                    {
                        maxWeaponType = 4;
                        smokeIndex = 2;
                        boomerangIndex = 3;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedSmoke)
                        {
                            maxWeaponType = 5;
                            smokeIndex = 2;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if (hasUnlockedSmoke)
                {
                    maxWeaponType = 3;
                    smokeIndex = 2;
                    blackHoleIndex = 3;
                    if (hasUnlockedBoomerang)
                    {
                        maxWeaponType = 4;
                        boomerangIndex = 3;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedBoomerang)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if (hasUnlockedLaser)
                {
                    maxWeaponType = 3;
                    laserIndex = 3;
                    if (hasUnlockedBoomerang)
                    {
                        maxWeaponType = 4;
                        boomerangIndex = 2;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedSmoke)
                        {
                            maxWeaponType = 5;
                            smokeIndex = 2;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedSmoke)
                    {
                        maxWeaponType = 4;
                        smokeIndex = 2;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBoomerang)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
            }
            else if(hasUnlockedLaser)
            {
                maxWeaponType = 2;
                laserIndex = 2;
                if (hasUnlockedBoomerang)
                {
                    maxWeaponType = 3;
                    boomerangIndex = 2;
                    laserIndex = 3;
                    if (hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedSmoke)
                        {
                            maxWeaponType = 5;
                            smokeIndex = 2;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedSmoke)
                    {
                        maxWeaponType = 4;
                        smokeIndex = 2;
                        boomerangIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if (hasUnlockedBlackHole)
                {
                    maxWeaponType = 3;
                    blackHoleIndex = 2;
                    laserIndex = 3;
                    if (hasUnlockedBoomerang)
                    {
                        maxWeaponType = 4;
                        boomerangIndex = 2;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedSmoke)
                        {
                            maxWeaponType = 5;
                            smokeIndex = 2;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedSmoke)
                    {
                        maxWeaponType = 4;
                        smokeIndex = 2;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBoomerang)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if (hasUnlockedSmoke)
                {
                    maxWeaponType = 3;
                    smokeIndex = 2;
                    laserIndex = 3;
                    if (hasUnlockedBoomerang)
                    {
                        maxWeaponType = 4;
                        boomerangIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBoomerang)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
            }
        }
        else
        {
            maxWeaponType = 2;
            flameIndex = 2;
            if(!hasUnlockedSmoke)
            {
                if(hasUnlockedBoomerang)
                {
                    maxWeaponType = 3;
                    boomerangIndex = 3;
                    if (hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if(hasUnlockedBlackHole)
                {
                    maxWeaponType = 3;
                    blackHoleIndex = 3;
                    if (hasUnlockedBoomerang)
                    {
                        maxWeaponType = 4;
                        boomerangIndex = 3;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedBoomerang)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else if(hasUnlockedLaser)
                {
                    maxWeaponType = 3;
                    laserIndex = 3;
                    if (hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBoomerang)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                    else if (hasUnlockedBoomerang)
                    {
                        maxWeaponType = 4;
                        boomerangIndex = 3;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            boomerangIndex = 3;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
            }
            else
            {
                maxWeaponType = 3;
                smokeIndex = 3;
                if(!hasUnlockedBoomerang)
                {
                    if(hasUnlockedBlackHole)
                    {
                        maxWeaponType = 4;
                        blackHoleIndex = 4;
                        if (hasUnlockedLaser)
                        {
                            maxWeaponType = 5;
                            laserIndex = 5;
                        }
                    }
                    else if(hasUnlockedLaser)
                    {
                        maxWeaponType = 4;
                        laserIndex = 4;
                        if (hasUnlockedBlackHole)
                        {
                            maxWeaponType = 5;
                            blackHoleIndex = 4;
                            laserIndex = 5;
                        }
                    }
                }
                else
                {
                    maxWeaponType = 4;
                    boomerangIndex = 4;
                }
                if(!hasUnlockedBlackHole)
                {
                    if(hasUnlockedLaser)
                    {
                        maxWeaponType = 5;
                        laserIndex = 5;
                    }
                }
                else
                {
                    maxWeaponType = 5;
                    blackHoleIndex = 5;
                    if(hasUnlockedLaser)
                    {
                        maxWeaponType = 6;
                        laserIndex = 6;
                    }
                }
            }
        }

        gameState.weaponIndex = weaponType;

        if (weaponType == 1)
        {
            ShootBomb();
        }

        if (weaponType == flameIndex)
        {
            ShootFlames();
        }

        if (weaponType == smokeIndex)
        {
            ShootSmokeBomb();
        }

        if (weaponType == boomerangIndex)
        {
            ShootBoomerang();
        }

        if (weaponType == blackHoleIndex)
        {
            ShootBlackHole();
        }

        if (weaponType == laserIndex)
        {
            ShootLaser();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            weaponType--;
            if (weaponType < 1)
            {
                weaponType = maxWeaponType;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightAlt))
        {
            weaponType++;
            if (weaponType > maxWeaponType)
            {
                weaponType = 1;
            }
        }
    }

    private void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);

        bullet.transform.position = this.gameObject.transform.GetChild(0).transform.position;
        if (gameState.hasGainedPowerUps[3])
        {
            if (gameState.upgradeLevel[13] == 0)
            {
                GameObject[] bullets = new GameObject[2];
                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i] = Instantiate(bulletPrefab);
                }
                bullets[0].transform.position = this.gameObject.transform.GetChild(1).transform.position;
                bullets[1].transform.position = this.gameObject.transform.GetChild(2).transform.position;
            }
            else if (gameState.upgradeLevel[13] == 1)
            {
                GameObject[] bullets = new GameObject[6];
                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i] = Instantiate(bulletPrefab);
                }
                bullets[0].transform.position = this.gameObject.transform.GetChild(1).transform.position;
                bullets[1].transform.position = this.gameObject.transform.GetChild(2).transform.position;
                bullets[2].transform.position = this.gameObject.transform.GetChild(3).transform.position;
                bullets[2].transform.localRotation = Quaternion.Euler(0, 0, 45);
                bullets[3].transform.position = this.gameObject.transform.GetChild(4).transform.position;
                bullets[3].transform.localRotation = Quaternion.Euler(0, 0, 315);
                bullets[4].transform.position = this.gameObject.transform.GetChild(5).transform.position;
                bullets[4].transform.localRotation = Quaternion.Euler(0, 0, 90);
                bullets[5].transform.position = this.gameObject.transform.GetChild(6).transform.position;
                bullets[5].transform.localRotation = Quaternion.Euler(0, 0, 270);
            }
            else if (gameState.upgradeLevel[13] == 2)
            {
                GameObject[] bullets = new GameObject[8];
                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i] = Instantiate(bulletPrefab);
                }
                bullets[0].transform.position = this.gameObject.transform.GetChild(1).transform.position;
                bullets[1].transform.position = this.gameObject.transform.GetChild(2).transform.position;
                bullets[2].transform.position = this.gameObject.transform.GetChild(3).transform.position;
                bullets[2].transform.localRotation = Quaternion.Euler(0, 0, 45);
                bullets[3].transform.position = this.gameObject.transform.GetChild(4).transform.position;
                bullets[3].transform.localRotation = Quaternion.Euler(0, 0, 315);
                bullets[4].transform.position = this.gameObject.transform.GetChild(5).transform.position;
                bullets[4].transform.localRotation = Quaternion.Euler(0, 0, 90);
                bullets[5].transform.position = this.gameObject.transform.GetChild(6).transform.position;
                bullets[5].transform.localRotation = Quaternion.Euler(0, 0, 270);
                bullets[6].transform.position = this.gameObject.transform.GetChild(7).transform.position;
                bullets[6].transform.localRotation = Quaternion.Euler(0, 0, 135);
                bullets[7].transform.position = this.gameObject.transform.GetChild(8).transform.position;
                bullets[7].transform.localRotation = Quaternion.Euler(0, 0, 225);
            }
            else if (gameState.upgradeLevel[13] == 3)
            {
                GameObject[] bullets = new GameObject[11];
                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i] = Instantiate(bulletPrefab);
                }
                bullets[0].transform.position = this.gameObject.transform.GetChild(1).transform.position;
                bullets[1].transform.position = this.gameObject.transform.GetChild(2).transform.position;
                bullets[2].transform.position = this.gameObject.transform.GetChild(3).transform.position;
                bullets[2].transform.localRotation = Quaternion.Euler(0, 0, 45);
                bullets[3].transform.position = this.gameObject.transform.GetChild(4).transform.position;
                bullets[3].transform.localRotation = Quaternion.Euler(0, 0, 315);
                bullets[4].transform.position = this.gameObject.transform.GetChild(5).transform.position;
                bullets[4].transform.localRotation = Quaternion.Euler(0, 0, 90);
                bullets[5].transform.position = this.gameObject.transform.GetChild(6).transform.position;
                bullets[5].transform.localRotation = Quaternion.Euler(0, 0, 270);
                bullets[6].transform.position = this.gameObject.transform.GetChild(7).transform.position;
                bullets[6].transform.localRotation = Quaternion.Euler(0, 0, 135);
                bullets[7].transform.position = this.gameObject.transform.GetChild(8).transform.position;
                bullets[7].transform.localRotation = Quaternion.Euler(0, 0, 225);
                bullets[8].transform.position = this.gameObject.transform.GetChild(9).transform.position;
                bullets[8].transform.localRotation = Quaternion.Euler(0, 0, 180);
                bullets[9].transform.position = this.gameObject.transform.GetChild(10).transform.position;
                bullets[9].transform.localRotation = Quaternion.Euler(0, 0, 180);
                bullets[10].transform.position = this.gameObject.transform.GetChild(11).transform.position;
                bullets[10].transform.localRotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

    private void ShootBomb()
    {
        if (gameState.numAmmo[0] <= 0)
        {
            canShootBomb = false;
        }
        else
        {
            reloadBombTimerMillis += Time.deltaTime;
            if (reloadBombTimerMillis >= maxReloadBombTimerMillis)
            {
                canShootBomb = true;
                if (Input.GetKeyDown(KeyCode.Space) && canShootBomb)
                {
                    GameObject projectile = Instantiate(weaponPrefabs[0]);

                    projectile.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                    canShootBomb = false;
                    reloadBombTimerMillis = 0;
                    gameState.numAmmo[0]--;
                }
            }
        }
    }

    private void ShootFlames()
    {
        if (gameState.numAmmo[1] <= 0)
        {
            canShootFlames = false;
            isFlamesActive = false;
            flameCount = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canShootFlames && !isFlamesActive)
        {
            canShootFlames = false;
            if (canSpawnFlame)
            {
                GameObject projectile = Instantiate(weaponPrefabs[1]);
                projectile.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                isFlamesActive = true;

                flameCount = 1;
                if (flameCount == 1)
                {
                    canSpawnFlame = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !canShootFlames && isFlamesActive)
        {
            canShootFlames = true;
            canSpawnFlame = true;
            isFlamesActive = false;
            flameCount = 0;
        }
    }

    private void ShootSmokeBomb()
    {
        if (gameState.numAmmo[2] <= 0)
        {
            canShootSmokeBomb = false;
        }
        else
        {
            reloadSmokeBombTimerMillis += Time.deltaTime;
            if (reloadSmokeBombTimerMillis >= maxReloadSmokeBombTimerMillis)
            {
                canShootSmokeBomb = true;
                if (Input.GetKeyDown(KeyCode.Space) && canShootSmokeBomb)
                {
                    GameObject projectile = Instantiate(weaponPrefabs[2]);

                    projectile.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                    canShootSmokeBomb = false;
                    reloadSmokeBombTimerMillis = 0;
                    gameState.numAmmo[2]--;
                }
            }
        }
    }
    private void ShootBoomerang()
    {
        reloadBoomerangMillis += Time.deltaTime;
        if (reloadBoomerangMillis >= maxReloadBoomerangTimerMillis)
        {
            canShootBoomerang = true;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameObject projectile = Instantiate(weaponPrefabs[3]);

                projectile.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                Vector2 projectileDirection = transform.up;
                projectile.GetComponent<Rigidbody2D>().AddForce(projectileDirection * boomerangPower);
                projectile.GetComponent<Rigidbody2D>().AddTorque(500);
                reloadBoomerangMillis = 0;
                canShootBoomerang = false;
            }
            
        }
    }
    private void ShootBlackHole()
    {
        reloadBlackHoleTimerMillis += Time.deltaTime;
        if(reloadBlackHoleTimerMillis >= maxReloadBlackHoleTimerMillis)
        {
            canShootBlackHole = true;
            if(Input.GetKey(KeyCode.Space))
            {
                GameObject projectile = Instantiate(weaponPrefabs[4]);

                projectile.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                reloadBlackHoleTimerMillis = 0;
                canShootBlackHole = false;
            }
        }
    }
    private void ShootLaser()
    {
        if (!canShootLaser)
        {
            reloadLaserTimerMillis += Time.deltaTime;
            if (reloadLaserTimerMillis >= maxReloadLaserTimerMillis)
            {
                canShootLaser = true;
            }
        }
        if (canShootLaser)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                laserChargeTimerMillis += Time.deltaTime;
                if (laserChargeTimerMillis >= maxLevel05ChargeTimerMillis)
                {
                    GameObject laser = Instantiate(laserPrefabs[4]);

                    laser.transform.position = this.gameObject.transform.GetChild(0).transform.position + (Vector3.up * 20);
                    laserChargeTimerMillis = 0;
                    canShootLaser = false;
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                if (laserChargeTimerMillis < maxLevel02ChargeTimerMillis)
                {
                    GameObject laser = Instantiate(laserPrefabs[0]);

                    laser.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                    laserChargeTimerMillis = 0;
                }
                if (laserChargeTimerMillis >= maxLevel02ChargeTimerMillis && laserChargeTimerMillis < maxLevel03ChargeTimerMillis)
                {
                    GameObject laser = Instantiate(laserPrefabs[1]);

                    laser.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                    laserChargeTimerMillis = 0;
                }
                if (laserChargeTimerMillis >= maxLevel03ChargeTimerMillis && laserChargeTimerMillis < maxLevel04ChargeTimerMillis)
                {
                    GameObject laser = Instantiate(laserPrefabs[2]);

                    laser.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                    laserChargeTimerMillis = 0;
                }
                if (laserChargeTimerMillis >= maxLevel04ChargeTimerMillis && laserChargeTimerMillis < maxLevel05ChargeTimerMillis)
                {
                    GameObject laser = Instantiate(laserPrefabs[3]);

                    laser.transform.position = this.gameObject.transform.GetChild(0).transform.position;
                    laserChargeTimerMillis = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        CheckWall(collider);
        CheckEnemyHit(collider, "EnemyPlanes", 20);
        CheckEnemyHit(collider, "EnemyJets", 30);
        CheckEnemyHit(collider, "EnemyBlimps", 25);
        CheckEnemyHit(collider, "EnemyKamikazes", 50);
        CheckEnemyHit(collider, "EnemyUfos", 25);
        CheckProjectileHit(collider, "EnemyProjectile", 10);
        CheckProjectileHit(collider, "JetBullet", 15);
        CheckProjectileHit(collider, "BlimpRocket", 20);
        CheckProjectileHit(collider, "UfoBullet", 15);
        Coins(collider);
        Ammos(collider);
        PowerUps(collider);
    }

    private void CheckEnemyHit(Collider2D collider, string name, float damage)
    {
        if (collider.CompareTag(name))
        {
            hasTakenDamage = true;
            gameState.playerHp -= damage * damageTaken;
            collider.gameObject.SetActive(false);
            Transform enemyExplosion = Instantiate(explosionPrefab);
            enemyExplosion.position = collider.transform.position;
            if (gameState.playerHp <= 0)
            {
                gameState.playerHp = 0;
                Transform explosion = Instantiate(explosionPrefab);
                explosion.position = this.transform.position;
                explosion.position = collider.transform.position;
                collider.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                gameState.LoadResultsScreen();
            }
        }
    }

    private void CheckProjectileHit(Collider2D collider, string name, float damage)
    {
        if (collider.CompareTag(name))
        {
            hasTakenDamage = true;
            gameState.playerHp -= damage * damageTaken;
            collider.gameObject.SetActive(false);
            if (gameState.playerHp <= 0)
            {
                gameState.playerHp = 0;
                Transform explosion = Instantiate(explosionPrefab);
                explosion.position = this.transform.position;
                collider.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                gameState.LoadResultsScreen();
            }
        }
    }

    private void CheckWall(Collider2D collider)
    {
        if (collider.CompareTag("LeftWall"))
        {
            hasHitLeft = true;
        }
        if (collider.CompareTag("RightWall"))
        {
            hasHitRight = true;
        }
        if (collider.CompareTag("UpWall"))
        {
            hasHitUp = true;
        }
        if (collider.CompareTag("DownWall"))
        {
            hasHitDown = true;
        }
    }

    private void Ammos(Collider2D collider)
    {
        CollectAmmos("BombAmmo", 0, 10, collider);
        CollectAmmos("FlameAmmo", 1, 50, collider);
        CollectAmmos("SmokeBombAmmo", 2, 8, collider);
        CollectAmmos("BlackHoleAmmo", 3, 5, collider);
        CollectAmmos("LaserAmmo", 4, 1, collider);
    } 

    private void Coins(Collider2D collider)
    {
        CollectCoins("SilverCoin", 5, collider);
        CollectCoins("BlueCoin", 10, collider);
        CollectCoins("RedCoin", 25, collider);
        CollectCoins("PurpleCoin", 50, collider);
        CollectCoins("GoldCoin", 100, collider);
        CollectCoins("TealCoin", 500, collider);
        CollectCoins("RedGoldCoin", 1000, collider);
    }

    private void PowerUps(Collider2D collider)
    {
        CollectPowerUps("Shield-PowerUp", 0, collider);
        CollectPowerUps("Slow-PowerUp", 1, collider);
        CollectPowerUps("MinaturePlane-PowerUp", 2, collider);
        CollectPowerUps("Bullet-PowerUp", 3, collider);
        CollectPowerUps("Healing-PowerUp", 4, collider);
    }

    private void CollectAmmos(string name, int index, int numAmmos, Collider2D collider) 
    { 
        if(collider.CompareTag(name))
        {
            gameState.numAmmo[index] += numAmmos;
            collider.gameObject.SetActive(false);
            if(gameState.numAmmo[index] + numAmmos > maxAmmoCap[index])
            {
                gameState.numAmmo[index] = maxAmmoCap[index];
            }
        }
    }

    private void CollectCoins(string name, float coins, Collider2D collider)
    {
        if(collider.CompareTag(name))
        {
            gameState.coins += coins;
            collider.gameObject.SetActive(false);
        }
    }

    private void CollectPowerUps(string name, int index, Collider2D collider)
    {
        if (collider.CompareTag(name))
        {
            gameState.hasGainedPowerUps[index] = true;
            collider.gameObject.SetActive(false);
        }
    }
}
