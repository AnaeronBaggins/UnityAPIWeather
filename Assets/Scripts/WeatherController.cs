using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherController : MonoBehaviour
{
    private const string API_KEY = "85b601064e2707e68ae6326707632af5";
    private const float API_CHECK_MAXTIME = 10 * 60.0f; //10 minutes
    public GameObject SnowSystem;
    public string CityId;
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    void Start()
    {
        CheckSnowStatus();
    }
    void Update()
    {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CheckSnowStatus();
            apiCheckCountdown = API_CHECK_MAXTIME;
        }
    }
    public void CheckSnowStatus()
    {
        bool snowing = GetWeather().weather[0].main.Equals("Snow");
        if (snowing)
            SnowSystem.SetActive(true);
        else
            SnowSystem.SetActive(false);
    }
    private WeatherInfo GetWeather()
    {
        Debug.Log("About to create the object of request. ");
        HttpWebRequest request =
        (HttpWebRequest)WebRequest.Create(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}",
         CityId, API_KEY));
        Debug.Log("Request done, about to get the response. ");
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string jsonResponse = reader.ReadToEnd();
        Debug.Log("The response is : " + jsonResponse);
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        return info;
    }
}


