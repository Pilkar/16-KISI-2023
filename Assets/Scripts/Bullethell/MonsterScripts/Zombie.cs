using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private float walkSpeed = 1f;
    private float accelerationDistance = 5f;
    private float accelerationMultiplier = 2.5f;
    private GameObject player;
    private Transform playerTransform;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        playerTransform = player.GetComponent<Transform>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        //Debug.Log("Distance: "+ distance);
        if(distance > accelerationDistance)
        {
            rb.velocity = new Vector2(walkSpeed * accelerationMultiplier * direction.x, walkSpeed * accelerationMultiplier * direction.y);    
        }
        else
        {
            rb.velocity = new Vector2(walkSpeed * direction.x, walkSpeed * direction.y);
        }
		
    }
}
