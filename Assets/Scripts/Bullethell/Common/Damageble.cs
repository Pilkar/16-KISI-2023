using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Damageble : MonoBehaviour
{
	public HealthBar healthBar;
	
    [SerializeField]
    private float _maxHealth = 100;

    [SerializeField]
    private int _killScore = 100;

    public int KillScore
    {
        get
        {
            return _killScore;
        }
        set
        {
            _killScore = value;
        }
    }

    public float MaxHealth {
        get {
            return _maxHealth;
        }
        set {
            _maxHealth = value;
        }
    }

    private float _health = 100;

    public float Health {
        get {
            return _health;
        }
        set {
            _health = value;
            if(this.gameObject.name == "Player")
                GameObject.Find("ScoreUI").GetComponent<PlayerStats>().UpdateUIStat((int)value, GameObject.Find("Player").GetComponent<player_controller>().Score);
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
	

	public bool IsAlive {
        get {
            return _isAlive;
        }
        set {
            _isAlive = value;
        }
    }
    
    [SerializeField]
    private bool _isOnTimeout;

	public bool IsOnTimeout { 
        get
        {
            return _isOnTimeout;
        }
        private set
        {
            _isOnTimeout = value;
        }
    }

    [SerializeField]
    float time = 0;


	// Start is called before the first frame update
	void Start()
    {
        IsOnTimeout = false;
        IsAlive = true;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(_maxHealth);
        }
    }

	// Update is called once per frame
	private void Update()
	{
        if (IsOnTimeout)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                time = 0;
                IsOnTimeout = false;
            }
        }
	}

	public void Hit(float damage)
    {
        if (IsAlive && !IsOnTimeout)
        {
            if (this.gameObject.name == "Player")
            {
                Connect.GenerateRecord("Player got hit", Health.ToString());
            }
            Health -= damage;
            time = 0.5f;
            IsOnTimeout = true;
            // Debug.Log("Health: " + Health + " Damege: " + damage);
            if (healthBar != null)
            {
                healthBar.SetHealth(Health);
            }
        }
        
        if (Health <=0)
        {
            IsAlive = false;
            if(this.isActiveAndEnabled)
                die();
        }
    }

    public void die()
    {
        if (this.gameObject.name == "Player")
        {
            if (GameObject.Find("NetworkManager").GetComponent<TransferInput>().ReadMulti())
            {
                //is Multiplayer
                GameObject.Find("ScoreUI").GetComponent<PlayerStats>().EndGame(true, GameObject.Find("Player").GetComponent<player_controller>().Score);
            }
            else
            {
                //is Singleplayer
                GameObject.Find("ScoreUI").GetComponent<PlayerStats>().EndGame(false, GameObject.Find("Player").GetComponent<player_controller>().Score);
            }
        }
        else
        {
            string desc = this.name + " was killed";
            Connect.GenerateRecord("Enemy killed", desc);
            Destroy(this.gameObject);
            GameObject.Find("Player").GetComponent<player_controller>().Score += KillScore;
            if (GameObject.Find("NetworkManager").GetComponent<TransferInput>().ReadMulti())
                GameObject.Find("ScoreUI").GetComponent<PlayerStats>().UpdateUIStat((int)GameObject.Find("Player").GetComponent<Damageble>().Health, GameObject.Find("Player").GetComponent<player_controller>().Score);
        }
    }
}
