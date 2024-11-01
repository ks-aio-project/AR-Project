using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR.ARFoundation;

public class AndroidCommunicate : MonoBehaviour
{
    public FirebaseInit firebaseInit;
    AndroidJavaObject _pluginInstance;

    //[System.Serializable]
    //public class Alert
    //{
    //    public string category; // 카테고리 (알림)
    //    public string alert_seq; // 장비 위치
    //    public string alert_type; // 알림 종류 (화재 누수 누전...)
    //    public string currentAlert; // 울린 센서
    //}

    [Serializable]
    public class AlertData
    {
        public int AlertID { get; set; }
        public int DeviceID { get; set; }
        public int Category { get; set; }
        public string CreateDate { get; set; }
        public string ReleaseDate { get; set; }
        public string DeviceType { get; set; }
        public string ModelName { get; set; }
        public string Location { get; set; }
        public string InstallationDate { get; set; }
        public int? CurrentStatus { get; set; }
        public string ProductName { get; set; }
        public string ManagementNumber { get; set; }
        public string Manufacturer { get; set; }
        public int Floor { get; set; }
        public string PurchaseDate { get; set; }
        public string Department { get; set; }
        public string Manager { get; set; }
        public string NetworkType { get; set; }
        public string Mac { get; set; }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Debug.Log("Application Focused (Resumed)");
            // 애플리케이션이 Resume 상태에 있을 때 실행할 코드
            if (_pluginInstance != null)
            {
                _pluginInstance.Call("getSaveSendData");
            }
        }
        else
        {
            Debug.Log("Application Lost Focus (Might be Paused)");
            // 애플리케이션이 포커스를 잃었을 때 실행할 코드
        }
    }

    private void Awake()
    {
        Application.runInBackground = true;
        var pluginClass = new AndroidJavaClass("kr.allione.mylibrary.UnityPlug");

        _pluginInstance = pluginClass.CallStatic<AndroidJavaObject>("instance");

        Debug.Log("kks Awake");
    }


    private void Start()
    {
        //_pluginInstance.Call("unitySendMessage", gameObject.name, "CallByAndroid", "Hello Android Toast");
        _pluginInstance.Call("startService");
        Debug.Log("kks Start");
    }

    void CallByAndroid(string message)
    {
        _pluginInstance.Call("showToast", message);

        Debug.Log("kks call android");
    }

    public void OnStartService()
    {
        //_pluginInstance.Call("unitySendMessage", gameObject.name, "CallByAndroid", "Hello Android Toast222");
        _pluginInstance.Call("unitySendMessage", gameObject.name, "StartService", "");
        Debug.Log("kks OnStartService");
    }

    // 이 메서드는 Android에서 Unity로 데이터를 전달할 때 호출됩니다.
    public void OnApiResponseReceived(string jsonString)
    {
        Debug.Log($"kks getData");
        Debug.Log("안드로이드로부터 JSON 데이터 수신: " + jsonString);

        // JSON 데이터를 역직렬화하여 객체로 변환
        try
        {
            // AlertData 객체로 역직렬화
            AlertData alertData = JsonConvert.DeserializeObject<AlertData>(jsonString);

            string categoryToText = "";

            switch (alertData.Category)
            {
                case 1:
                    categoryToText = "화재";
                    break;
                case 2:
                    categoryToText = "누수";
                    break;
                case 3:
                    categoryToText = "단선";
                    break;
                case 4:
                    categoryToText = "통신 에러";
                    break;
                case 5:
                    categoryToText = "재실";
                    break;
            }
            firebaseInit.JsonNotification(alertData.DeviceID, categoryToText, alertData.Floor, alertData.Location, alertData.CreateDate, alertData.ManagementNumber);
        }
        catch (JsonException ex)
        {
            Debug.LogError("JSON 파싱 오류: " + ex.Message);
        }
    }

    private void ProcessApiResponse(string jsonData)
    {
        Debug.Log($"kks jsonData : {jsonData}");
        // jsonData를 파싱하거나 데이터 구조로 변환
        // 예를 들어, JSON 유틸리티를 사용하여 데이터를 파싱할 수 있습니다.
        // MyData data = JsonUtility.FromJson<MyData>(jsonData);

        // 데이터를 활용한 게임 로직 추가
    }

    public void StartService()
    {
        Debug.Log("kks call startService");
        _pluginInstance.Call("startService");
    }

    public void StopService()
    {
        _pluginInstance.Call("stopService");
        Debug.Log("kks call stopService");
    }
}