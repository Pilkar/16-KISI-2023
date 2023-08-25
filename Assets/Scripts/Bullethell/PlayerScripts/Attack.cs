using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    public float attackDamage = 25;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
	{
		boxCollider2D = GetComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
    public void OnTriggerStay2D(Collider2D collision)
	{
        Damageble damageble = collision.GetComponent<Damageble>();

		if (damageble != null)
		{
			damageble.Hit(attackDamage);
		}

	}
}
