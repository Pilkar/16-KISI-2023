using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject Zombie;
    public GameObject Skeleton;
    public GameObject Troll;
    DummyPulse dummyPulse;
    [SerializeField]
    public bool freezeWaves = false;
    [SerializeField]
    public float timeBetweenWaves = 5f;
    [SerializeField]
    public int zombieBaseWaveSize = 5;
    public int skeletonBaseWaveSize = 3;
    public int trollBaseWaveSize =  2;
    private float timeSinceLastWave = 10f;
    public float spawnRadius = 20f;
    private Transform playerLocation;

    private void Awake()
    {
        // zombies = new ArrayList();
        playerLocation = GameObject.Find("Player").GetComponent<Transform>();
        dummyPulse = GameObject.FindObjectOfType<DummyPulse>();
    }

    private void FixedUpdate()
    {
        if(timeSinceLastWave <= 0)
        {
            float heartRate = checkHeartRate();
            float diff = heartRate/Connect.ReadBaseHeartRate();
            float zombieWave = Mathf.Round(diff * zombieBaseWaveSize);
            float skeletonWave = Mathf.Round(diff * skeletonBaseWaveSize);
            float trollWave = Mathf.Round(diff * trollBaseWaveSize);
            // Debug.Log("heart Rate: "+heartRate+" zombieWave: "+zombieWave+" skeletonWave: "+skeletonWave+" trollWave: "+trollWave);
            SpawnMonsters(Zombie, (int)zombieWave);
            SpawnMonsters(Skeleton, (int)skeletonWave);
            SpawnMonsters(Troll, (int)trollWave);
            timeSinceLastWave = timeBetweenWaves;
        }
        if(!freezeWaves)
        {
            timeSinceLastWave -= Time.deltaTime;
        }
    }

    private void SpawnMonsters(GameObject monster, int waveSize)
    {
        for(int i=0;i<waveSize;i++)
        {
            Vector2 positionToSpawn = playerLocation.position + Random.insideUnitSphere * spawnRadius;
            Instantiate(monster, positionToSpawn, Quaternion.identity);
        }
    }

    float checkHeartRate()
    {
        return Connect.heartRateVal;
        //return dummyPulse.Pulse;
    }
    
}
