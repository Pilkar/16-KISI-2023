using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private float movingThreshold;
    [SerializeField]
    private float movementSpeed  = 1f;
    // public float baseMovementSpeed = 2f;
    private Vector3 direction;
    Vector3 startingPosition;
    public float leftPillarXPosition = -8.293f;
    public float rightPillarXPosition = -0.197f;
    void Awake()
    {
        movingThreshold = GetComponent<SpriteRenderer>().bounds.size.x;
        direction = Vector3.right;
        movementSpeed = Random.Range(0.5f, 3f);
        startingPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        float width = GetComponent<SpriteRenderer>().bounds.size.x;
        transform.position += direction * movementSpeed * Time.deltaTime;
        if(Mathf.Abs(gameObject.transform.position.x - startingPosition.x) >= movingThreshold || gameObject.transform.position.x - (width/2) < leftPillarXPosition || gameObject.transform.position.x + (width/2) > rightPillarXPosition)
        {
            if(direction == Vector3.right)
            {
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.right;
            }
        }
    }
}
