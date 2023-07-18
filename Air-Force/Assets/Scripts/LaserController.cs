using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    GameState gameState;
    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    public float horizontalSpeed, verticalSpeed;

    protected float maxLaserTimerMillis = 4f;
    protected float laserTimerMillis;

    protected bool hasHitLeft, hasHitRight, hasHitUp, hasHitDown;
    private void Start()
    {
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

        if(gameState.upgradeLevel[9] == 3)
        {
            maxLaserTimerMillis = 6;
        }
    }
    private void Update()
    {
        laserTimerMillis += Time.deltaTime;
        if(laserTimerMillis >= maxLaserTimerMillis)
        {
            this.gameObject.SetActive(false);
        }

        float xTransition = horizontalSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float yTransition = verticalSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        if (xTransition < 0 && !hasHitLeft)
        {
            transform.Translate(new Vector3(xTransition, 0, 0), Space.Self);
            hasHitRight = false;
        }

        if (xTransition > 0 && !hasHitRight)
        {
            transform.Translate(new Vector3(xTransition, 0, 0), Space.Self);
            hasHitLeft = false;
        }

        if (yTransition < 0 && !hasHitDown)
        {
            transform.Translate(new Vector3(0, yTransition, 0), Space.Self);
            hasHitUp = false;
        }

        if (yTransition > 0 && !hasHitUp)
        {
            transform.Translate(new Vector3(0, yTransition, 0), Space.Self);
            hasHitDown = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("LaserLeftWall"))
        {
            hasHitLeft = true;
        }
        if (collider.CompareTag("LaserRightWall"))
        {
            hasHitRight = true;
        }
        if (collider.CompareTag("LaserUpWall"))
        {
            hasHitUp = true;
        }
        if (collider.CompareTag("Player"))
        {
            hasHitDown = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            hasHitDown = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            hasHitDown = false;
        }
    }
}
