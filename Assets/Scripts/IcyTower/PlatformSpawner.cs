using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    
    [SerializeField]
    public GameObject platformObj; // platform prefab
    public GameObject pillarObj; // pillar prefab
    private float height=-4.76f; // Starting Y position
    [SerializeField]
    private Transform playerPos; // player position reference, used for dynamic platform spawning instead of spawning all platforms at once
    // private GameObject newPlatform; // ref for instance of new platform
    public List<GameObject> platforms; // array containing all platforms
    public List<(GameObject, GameObject)> pillars; //array containing all pillars (left pillar, right pillar)
    private float currPillarY = 0f;
    private float leftPillarX = -4.93f;
    private float rightPillarX = 3.56f;

    DummyPulse dummyPulse;

    private void Awake()
    {
        platforms = new List<GameObject>();
        pillars = new List<(GameObject, GameObject)>();
        platforms.Add(GameObject.Find("StartingPlatform"));
        pillars.Add((GameObject.Find("leftStartingPillar"), GameObject.Find("rightStartingPillar")));
        dummyPulse = FindObjectOfType<DummyPulse>();
        leftPillarX = GameObject.Find("leftStartingPillar").transform.position.x;
        rightPillarX = GameObject.Find("rightStartingPillar").transform.position.x;
        currPillarY =  GameObject.Find("leftStartingPillar").transform.position.y + 20;
    }
    void Update()
    {
        LevelSpawning();
    }

    bool IsPlatformMoving()
    {
        float movingPlatformChance = ((CheckHeartRate()/Connect.ReadBaseHeartRate()) - 1)*100;
        float randomNumber = Random.value*100;
        if(randomNumber <= movingPlatformChance)
        {
            return true;
        }
        return false;
    }

    void LevelSpawning()
    {
        //Spawning platforms only to certain height above player
        if (Mathf.Abs(playerPos.position.y - height) < 15)
        {
            //generating platforms at random positions between pillars and random scale
            height++;
            GameObject newPlatform = Instantiate(platformObj, new Vector3(Random.Range(-7.343f, -0.753f), height, 0), new Quaternion(0, 0, 0, 0));
            newPlatform.transform.localScale = new Vector3(Random.Range(0.2f, 0.5f), 0.2f, 1);
            
            platforms.Add(newPlatform);
            if(IsPlatformMoving())
            {
                newPlatform.AddComponent<MovingPlatform>();
            }

            //calculating distance to pillars, and adjusting the scale of platform to make them stay inside tower
            // Constants:
            // 2.5f = platform width/2,
            // -7,843f = left pillar X,
            // -0.253f = right pillar X,
            // 0.2f = Y scale of platform
            if (newPlatform.transform.position.x - 2.5f* newPlatform.transform.localScale.x < leftPillarX)
                newPlatform.transform.localScale = new Vector3(Mathf.Abs(newPlatform.transform.position.x - leftPillarX) / 2.5f, 0.2f, 1);
            else if (newPlatform.transform.position.x + 2.5f * newPlatform.transform.localScale.x > rightPillarX)
                newPlatform.transform.localScale = new Vector3(Mathf.Abs(newPlatform.transform.position.x - rightPillarX) / 2.5f, 0.2f, 1);

            GameObject lastPlatform = platforms[0];
            //delete platforms that are far from player
            if(Mathf.Abs(playerPos.position.y - lastPlatform.transform.position.y) > 30)
            {
                platforms.Remove(lastPlatform);
                Destroy(lastPlatform);
            }

        }

        //spawning pillars
        if(currPillarY - playerPos.position.y < 20)
        {
            //spawn above last pillar
            GameObject leftPillar = Instantiate(pillarObj, new Vector3(leftPillarX, currPillarY, 0), new Quaternion(0, 0, 0, 0));
            GameObject rightPillar = Instantiate(pillarObj, new Vector3(rightPillarX, currPillarY, 0), new Quaternion(0, 0, 0, 0));
            leftPillar.transform.Rotate(0f, 0f, -90f);
            rightPillar.transform.Rotate(0f, 0f, 90f);
            pillars.Add((leftPillar, rightPillar));

            //calculate next pillar Y position
            currPillarY += 20f;

            var lastPillars = pillars[0];
            //delete pillars that are far from player
            if(Mathf.Abs(playerPos.position.y - lastPillars.Item1.transform.position.y) > 20)
            {
                pillars.Remove(lastPillars);
                Destroy(lastPillars.Item1);
                Destroy(lastPillars.Item2);
            }
        }
    }

    float CheckHeartRate()
    {
        return Connect.heartRateVal;
        // return dummyPulse.Pulse;
    }

}
