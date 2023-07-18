using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoShooterController : MonoBehaviour
{
    public int upperRandomRange;
    public GameObject[] projectilePrefab;

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

                for (int j = 0; j <= 7; j++)
                {
                    projectile[j].transform.position = this.gameObject.transform.GetChild(j).transform.position;
                }

                projectile[0].transform.localRotation = this.transform.localRotation;
                projectile[1].transform.localRotation = this.transform.localRotation * Quaternion.Euler(0, 0, 45);
                projectile[2].transform.localRotation = this.transform.localRotation * Quaternion.Euler(0, 0, 90);
                projectile[3].transform.localRotation = this.transform.localRotation * Quaternion.Euler(0, 0, 135);
                projectile[4].transform.localRotation = this.transform.localRotation * Quaternion.Euler(0, 0, 180);
                projectile[5].transform.localRotation = this.transform.localRotation * Quaternion.Euler(0, 0, 225);
                projectile[6].transform.localRotation = this.transform.localRotation * Quaternion.Euler(0, 0, 270);
                projectile[7].transform.localRotation = this.transform.localRotation * Quaternion.Euler(0, 0, 315);
            }
        }
    }
}
