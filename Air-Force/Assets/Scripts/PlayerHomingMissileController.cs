using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHomingMissileController : MonoBehaviour
{
    public float angleChangingSpeed;
    public float movementSpeed;
    public GameObject[] target;

    protected int random;
    protected Rigidbody2D rigidBody;
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        random = Random.Range(0, target.Length);
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target[random].transform.position - rigidBody.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rigidBody.angularVelocity = -angleChangingSpeed * rotateAmount;
        rigidBody.velocity = transform.up * movementSpeed;
    }
}
