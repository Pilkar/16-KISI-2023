using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Aidlab.BLE;
using System.IO;

public class Connect : MonoBehaviour
{
    static Connect instance;
    public static int SENSOR_CONNECTED;
    public static int heartRateVal = 77;
    public static int baseHeartRate = 80; //UPDATE THIS AFTER CALIBRATION
    public static int maxHeartRateSoFar = 80;
    public static string wearState="";
    private bool received = false;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Establishing Connection");
            instance = this;
            DontDestroyOnLoad(gameObject);
            Aidlab.AidlabSDK.init();
            Aidlab.AidlabSDK.aidlabDelegate.heartRate.Subscribe(ReceiveData);
            Aidlab.AidlabSDK.aidlabDelegate.wearState.Subscribe(ReceiveState);
        }
    }

    void Start()
    {
        
    }

    private void ReceiveData()
    {
        heartRateVal = Aidlab.AidlabSDK.aidlabDelegate.heartRate.value;
        received = true;
        Debug.Log(heartRateVal);
    }
    private void ReceiveState()
    {
        wearState = Aidlab.AidlabSDK.aidlabDelegate.wearState.value.ToString();
        //Debug.Log(wearState);

        if (wearState == "Deatached")
            GameObject.Find("SensorStatus").GetComponent<Image>().color = Color.red;
        else if(wearState == "PlacedProperly")
            GameObject.Find("SensorStatus").GetComponent<Image>().color = Color.green;
        else if (wearState == "Loose")
            GameObject.Find("SensorStatus").GetComponent<Image>().color = Color.yellow;
    }

    public void Calibrate()
    {
        StartCoroutine(CollectData());
    }
    /// <summary>
    /// Reads the current heart rate saved in Aidlab script
    /// </summary>
    /// <returns>Last readed heart rate</returns>
    public static int ReadHeartRate()
    {
        if(heartRateVal > maxHeartRateSoFar) maxHeartRateSoFar = heartRateVal;
        return heartRateVal;
    }
    /// <summary>
    /// Reads the base heart rate saved in Aidlab script
    /// </summary>
    /// <returns>Saved base heart rate</returns>
    public static int ReadBaseHeartRate()
    {
        return baseHeartRate;
    }
    /// <summary>
    /// Reads the difference between base and current heart rate saved in Aidlab script
    /// </summary>
    /// <returns>difference current - base heart rate</returns>
    public static int ReadDiff()
    {
        return (heartRateVal - baseHeartRate);
    }
    IEnumerator CollectData()
    {
        int timeout = 0;
        int loopCount = 0;
        int sum = 0;
        bool endLoop = true;

        while (endLoop)
        {
            // Debug.Log("Calibrating");
            GameObject.Find("InfoTxt").GetComponent<TMPro.TMP_Text>().text = "stay still..." + timeout + "sec";
            if (received)
            {
                // Debug.Log("Received");
                sum += heartRateVal;
                received = false;
                GameObject.Find("ProgressBar").GetComponent<Slider>().value +=1;
                loopCount++;
            }
            if (loopCount == 5 || timeout == 30) // 5 readouts or 30 sec timeout
            {
                endLoop = false;
            }
            timeout++;
            yield return new WaitForSeconds(1);
        }
        if (loopCount != 0)
        {
            GameObject.Find("ProgressBar").SetActive(false);
            baseHeartRate = Mathf.RoundToInt(sum / loopCount);
            GameObject.Find("currentRefTxt").GetComponent<TMPro.TMP_Text>().text = baseHeartRate.ToString();
            Debug.Log("Avarage= "+baseHeartRate);
        }
    }

    public static void GenerateRecord(string ev, string des)
    {
        string time = System.DateTime.Now.Hour.ToString() + ":"+System.DateTime.Now.Minute.ToString()+":"+System.DateTime.Now.Second.ToString() + ":" + System.DateTime.Now.Millisecond.ToString();
        string sensor = ReadHeartRate().ToString() + ";" + ReadDiff().ToString() + ";" + Aidlab.AidlabSDK.aidlabDelegate.wearState.value.ToString();
        // Debug.Log(ev + ";" + des + ";" + time + ";" + sensor);
        string path = Application.persistentDataPath + "/test.csv";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(ev + ";" + des + ";" + time + ";" + sensor);
        writer.Close();
    }
}
