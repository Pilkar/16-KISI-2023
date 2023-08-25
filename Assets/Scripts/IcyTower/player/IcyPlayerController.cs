using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class IcyPlayerController : MonoBehaviour
{
    Vector2 moveInput;
    public float WalkSpeed = 2f;
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] 
    GameObject Trail;

    [SerializeField]
    public int baseJumpPower = 10;
    private float boost = 1;
    private bool isJumped = false;
    private float startingPositionY;
    // boost variables
    public enum IcyDirections { left = -1, right = 1};
    private IcyDirections directionAfterLanding;
    private bool isGrounded = true;
    private float stillTimeInAir = 0f; 
    private float stillTimeInAirThreshold = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startingPositionY = transform.position.y;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * WalkSpeed, rb.velocity.y);

        if(rb.velocity.y == 0 && !isGrounded)
        {
            isGrounded = true;
            directionAfterLanding = (IcyDirections)transform.localScale.x;
        }
        else if(rb.velocity.y != 0 && isGrounded)
        {
            isGrounded = false;
            //player fell from platform
            if(rb.velocity.y < 0) boost = 1;
        }

        if(isGrounded)
        {
            if ((IcyDirections)transform.localScale.x != directionAfterLanding || rb.velocity.x == 0)
            {
                boost = 1;
            }
        }
        
        if(rb.velocity.y != 0)
        {
            isJumped = true;
            //if player won't move for some time in air then boost is reset
            if(moveInput.x == 0)
            {
                stillTimeInAir += Time.deltaTime;
                if(stillTimeInAir > stillTimeInAirThreshold)
                {
                    boost = 1;
                }
            }
            else
            {
                stillTimeInAir = 0f;
            }
        }
        else
        {
            stillTimeInAir = 0f;
        }

        //jump once after key pressed and add boost
        if (moveInput.y > 0 && !isJumped)
        {
            isJumped = true;
            if(rb.velocity.y <= 0.01f)
            {
                //jump event
                rb.AddForce(new Vector2(0, baseJumpPower*boost));
                if (boost <= 2f)
                {
                    Connect.GenerateRecord("Normal jump", boost.ToString());
                    boost += 0.2f;
                }
                else
                {
                    StartCoroutine(SpawnTrail()); // spawn trail that is fading
                    Connect.GenerateRecord("Big jump", boost.ToString());
                }
                
            }
        }
        else if (moveInput.y == 0 && isJumped)
            isJumped = false;

		
		if(moveInput.x < 0)
			gameObject.transform.localScale = new Vector3(-1,1,1);
		else if(moveInput.x > 0)
			gameObject.transform.localScale = new Vector3(1,1,1);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

    }

    private IEnumerator SpawnTrail()
    {
        float TrailTime = 0.3f; // initial time of fading
        GameObject trail = Instantiate(Trail,this.transform.position,this.transform.rotation,this.transform);
        while (TrailTime > 0)
        {
            TrailTime -= 0.01f; // fading speed
            trail.GetComponent<TrailRenderer>().time = TrailTime;
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(trail);
    }

    public void die()
    {
        GameObject.FindObjectOfType<IcyPlayerStats>().GameOver((int)(transform.position.y - startingPositionY));
        // SceneManager.LoadScene("DeathScene");
    }

}
