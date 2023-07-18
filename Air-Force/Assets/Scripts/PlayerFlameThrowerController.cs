using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlameThrowerController : MonoBehaviour
{
    GameState gameState;

    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }

    BoxCollider2D collider;
    ParticleSystem flame;
    public float horizontalSpeed, verticalSpeed;
    protected bool hasHitLeft, hasHitRight, hasHitUp, hasHitDown;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        flame = this.gameObject.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
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
        if(gameState.upgradeLevel[5] == 3)
        {
            collider.size = new Vector2(4, 18.2f);
            collider.offset = new Vector2(0, 8.5f);
            flame.startSpeed = 3.5f;
        }
    }

    void Update()
    {
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

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt) || gameState.numAmmo[1] <= 0 || gameState.playerHp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("LeftWall"))
        {
            hasHitLeft = true;
        }
        if (collider.CompareTag("RightWall"))
        {
            hasHitRight = true;
        }
        if (collider.CompareTag("Player"))
        {
            hasHitDown = true;
        }
        if (collider.CompareTag("FlameUpWall"))
        {
            hasHitUp = true;
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
