using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    float AscendSpeed = 2.2f;
    Vector2 destination;
    DummyPulse dummyPulse;
    float heartRate;
    [SerializeField] Transform PlayerPos;
    private void Start()
    {
        dummyPulse = FindObjectOfType<DummyPulse>();
        destination = transform.position + new Vector3(0, 10, 0);
    }
    void Update()
    {

        if(Mathf.Abs(PlayerPos.transform.position.y - this.transform.position.y) > 8) // 15 is how far you must run away to bring lava closer
        {
            this.transform.position = PlayerPos.transform.position - new Vector3(0, 5, 0); // 5 is how close lava is brought after running away
            destination = transform.position + new Vector3(0, 10, 0);
        }
        if(Mathf.Abs(transform.position.y - destination.y) <= 0.1f)
        {
            destination = transform.position + new Vector3(0, 10,0);
        }
        heartRate = CheckHeartRate();
        float diff = heartRate/Connect.ReadBaseHeartRate();
        //Moves the GameObject from it's current position to destination over time
        transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime*AscendSpeed*diff);
        // Debug.Log("Lava speed: "+AscendSpeed*diff);
    }

    public void OnTriggerEnter2D(Collider2D collision)
	{
        collision.GetComponent<IcyPlayerController>().die();
	}

    float CheckHeartRate()
    {
        // return Connect.heartRateVal;
        return dummyPulse.Pulse;
    }
}
