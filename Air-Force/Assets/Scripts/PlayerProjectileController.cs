using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour
{
    GameState gameState;

    private void Awake()
    {
        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    public float[] speedUpgrade;
    public int index;

    protected float speed;
    private void Start()
    {
        if(gameState.upgradeLevel[index] == 0)
        {
            speed = speedUpgrade[0];
        }
        else if (gameState.upgradeLevel[index] == 1)
        {
            speed = speedUpgrade[1];
        }
        else if (gameState.upgradeLevel[index] == 2)
        {
            speed = speedUpgrade[2];
        }
        else if (gameState.upgradeLevel[index] == 3)
        {
            speed = speedUpgrade[3];
        }
    }

    private void Update()
    {
        float yTransition = speed * Time.deltaTime;
        transform.Translate(new Vector3(0, yTransition, 0), Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("UpWall") || collider.CompareTag("DownWall") || collider.CompareTag("LeftWall") || collider.CompareTag("RightWall"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
