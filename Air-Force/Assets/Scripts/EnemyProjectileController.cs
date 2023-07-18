using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public float speed;

    private void Update()
    {
        float yTransition = speed * Time.deltaTime;

        transform.Translate(new Vector3(0, yTransition, 0), Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("UpWall") || collider.CompareTag("DownWall") || collider.CompareTag("LeftWall") || collider.CompareTag("RightWall"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
