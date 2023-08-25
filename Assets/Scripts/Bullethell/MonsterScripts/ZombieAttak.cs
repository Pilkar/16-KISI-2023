using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttak : MonoBehaviour
{
	public float attackDamage = 5;

	private void OnTriggerStay2D(Collider2D collision)
	{
		Damageble damageble = collision.GetComponent<Damageble>();
		//Debug.Log(collision.gameObject.name);
		if (damageble != null)
		{
			damageble.Hit(attackDamage);
		}

	}
}
