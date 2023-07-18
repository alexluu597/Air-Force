using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int upperRandomRange;
    public float minXPosition;
    public float maxXPosition;
    public float minYPosition;
    public float maxYPosition;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, .2f);
    }
    // Update is called once per frame
    void Update()
    {
        StartCoroutine(WaitTillSpawn());
    }

    IEnumerator WaitTillSpawn()
    {
        yield return new WaitForSeconds(2);

        if (!Pause.gameIsPaused)
        {
            int randomSpawn = Random.Range(1, upperRandomRange);
            if (randomSpawn == 1)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                int randomDirection;

                int randomXPosition;
                int randomYPosition;

                int randomSide = Random.Range(0, 3);

                if (randomSide == 0)
                {
                    randomYPosition = (int)maxXPosition;
                    randomXPosition = Random.Range((int)minXPosition, (int)maxXPosition + 1);

                    if (randomXPosition <= -10)
                    {
                        randomDirection = Random.Range(180, 225);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }
                    else if (randomXPosition >= 10)
                    {
                        randomDirection = Random.Range(135, 180);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }
                    else if (randomXPosition >= -10 && randomXPosition <= 10)
                    {
                        randomDirection = Random.Range(135, 225);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }

                    enemy.transform.position = new Vector3(randomXPosition, randomYPosition, 0);
                }
                else if (randomSide == 1)
                {
                    randomXPosition = (int)minXPosition;
                    randomYPosition = Random.Range((int)minYPosition, (int)maxYPosition + 1);

                    if (randomYPosition <= -10)
                    {
                        randomDirection = Random.Range(270, 315);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }
                    else if (randomYPosition >= 10)
                    {
                        randomDirection = Random.Range(225, 270);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }
                    else if (randomYPosition >= -10 && randomXPosition <= 10)
                    {
                        randomDirection = Random.Range(225, 315);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }

                    enemy.transform.position = new Vector3(randomXPosition, randomYPosition, 0);
                }
                else if (randomSide == 2)
                {
                    randomYPosition = (int)minYPosition;
                    randomXPosition = Random.Range((int)minXPosition, (int)maxXPosition + 1);

                    if (randomXPosition <= -10)
                    {
                        randomDirection = Random.Range(315, 360);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }
                    else if (randomXPosition >= 10)
                    {
                        randomDirection = Random.Range(0, 45);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }
                    else if (randomXPosition >= -10 && randomXPosition <= 10)
                    {
                        randomDirection = Random.Range(315, 405);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }

                    enemy.transform.position = new Vector3(randomXPosition, randomYPosition, 0);
                }
                else if (randomSide == 3)
                {
                    randomXPosition = (int)maxXPosition;
                    randomYPosition = Random.Range((int)minYPosition, (int)maxYPosition + 1);

                    if (randomYPosition <= -10)
                    {
                        randomDirection = Random.Range(45, 90);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }
                    else if (randomYPosition >= 10)
                    {
                        randomDirection = Random.Range(90, 135);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }
                    else if (randomYPosition >= -10 && randomXPosition <= 10)
                    {
                        randomDirection = Random.Range(45, 135);
                        enemy.transform.localRotation = Quaternion.Euler(0, 0, randomDirection);
                    }

                    enemy.transform.position = new Vector3(randomXPosition, randomYPosition, 0);
                }
            }
        }
    }
}
