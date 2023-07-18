using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooterController : MonoBehaviour
{
    public int upperRandomRange;
    public GameObject[] projectilePrefab;
    public int numShooters;

    // Update is called once per frame
    void Update()
    {
        if (!Pause.gameIsPaused)
        {
            int random = Random.Range(1, upperRandomRange);
            if (random == 1)
            {
                GameObject[] projectile = new GameObject[projectilePrefab.Length];

                for (int i = 0; i < projectilePrefab.Length; i++)
                {
                    projectile[i] = Instantiate(projectilePrefab[i]);
                }

                for (int j = 0; j <= numShooters; j++)
                {
                    projectile[j].transform.position = this.gameObject.transform.GetChild(j).transform.position;
                    projectile[j].transform.localRotation = this.transform.localRotation;
                }
            }
        }
    }
}
