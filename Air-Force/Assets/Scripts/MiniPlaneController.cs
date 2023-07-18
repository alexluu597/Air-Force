using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPlaneController : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    public GameObject bullet;

    public float horizontalSpeed, verticalSpeed;
    public string leftCollider, rightCollider;
    protected bool hasHitLeft, hasHitRight, hasHitUp, hasHitDown;

    protected float maxReloadBulletTimerMillis = 0.1f;
    protected float reloadBulletTimerMillis;

    protected bool canShootBullet;

    private void Start()
    {
        reloadBulletTimerMillis = maxReloadBulletTimerMillis;
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
    }

    void Update()
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

        if (gameState.playerHp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void ShootBullet()
    {
        GameObject projectile = Instantiate(bullet);

        projectile.transform.position = this.gameObject.transform.GetChild(0).transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(leftCollider))
        {
            hasHitLeft = true;
        }
        if (collider.CompareTag(rightCollider))
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
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (leftCollider == "Player")
        {
            if (collider.CompareTag(leftCollider))
            {
                hasHitLeft = true;
            }
        }
        if (rightCollider == "Player")
        {
            if (collider.CompareTag(rightCollider))
            {
                hasHitRight = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (leftCollider == "Player")
        {
            if (collider.CompareTag(leftCollider))
            {
                hasHitLeft = false;
            }
        }
        if (rightCollider == "Player")
        {
            if (collider.CompareTag(rightCollider))
            {
                hasHitRight = false;
            }
        }
    }
}
