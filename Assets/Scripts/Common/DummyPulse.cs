using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPulse : MonoBehaviour
{
    private float timer;
    private bool reverse = false;

    public int Pulse { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Pulse = 80;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        
        if (timer>1) 
        { 
            timer = 0;
            changePulse();
        }
    }

    private void changePulse()
    {
        if (reverse)
        {
            Pulse -= UnityEngine.Random.Range(0, 5);
        }
        else
        {
            Pulse += UnityEngine.Random.Range(0, 5);
        }
        
        if (Pulse >= 150)
        {
            reverse = true;
        } else if (Pulse <= 60)
        {
            reverse = false;
        }

        //Debug.Log(Pulse);
    }
}
