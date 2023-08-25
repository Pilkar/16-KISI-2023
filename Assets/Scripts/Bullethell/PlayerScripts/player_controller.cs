using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class player_controller : MonoBehaviour
{
    Collider2D attack;

    Vector2 moveInput;

    public float WalkSpeed = 2f;
    Rigidbody2D rb;
    Animator animator;

    public int Score;

	
    private bool _isMoving = false;
    private int _direction = 0;


    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool("moving",value);
        }
    }

    public int Direction
    {
        get
        {
            return _direction;
        }
        private set
        {
            _direction = value;
            animator.SetInteger("direction", value);
        }
    }

    private void Awake()
    { 
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attack = GetComponentInChildren<BoxCollider2D>();

        boxCollider2D = attack.GetComponent<BoxCollider2D>();
        spriteRenderer = attack.GetComponent<SpriteRenderer>();

        Score = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * WalkSpeed, moveInput.y * WalkSpeed);
        if (Math.Abs(moveInput.y) < Math.Abs(moveInput.x))
        {
            if (moveInput.x < 0)
            {
                Direction = (int) Directions.LEFT;
                attack.transform.position = new Vector2(this.transform.position.x - 0.5f, this.transform.position.y);
            }
            else
            {
                Direction = (int) Directions.RIGHT;
                attack.transform.position = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
            }
        }
        else
        {
            if (moveInput.y < 0)
            {
                Direction = (int)Directions.DOWN;
                attack.transform.position = new Vector2(this.transform.position.x, this.transform.position.y  - 0.5f);
            }
            else
            {
                Direction = (int)Directions.UP;
                attack.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
            }
        }

       if(!IsMoving)
           attack.transform.position = new Vector2(this.transform.position.x, this.transform.position.y);

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

        if (!IsMoving)
            if (Math.Abs(moveInput.y) < Math.Abs(moveInput.x))
            {
                if (moveInput.x < 0)
                    Direction = (int) Directions.LEFT;
                else
                    Direction = (int) Directions.RIGHT;
            }
            else
            {
                if (moveInput.y < 0)
                    Direction = (int) Directions.DOWN;
                else
                    Direction = (int) Directions.UP;
            }
    }

    public void OnAttak(InputAction.CallbackContext context)
    {
        //Debug.Log("atak");
        Connect.GenerateRecord("Player Attack", "Attack was triggered");

        if (this.GetComponent<Damageble>().IsAlive)
            StartCoroutine(passiveMe());

        IEnumerator passiveMe()
        {
            boxCollider2D.enabled = true;
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(2);
            boxCollider2D.enabled = false;
            spriteRenderer.enabled = false;
        }

    }
}
