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
    //    public string category; // ī�װ� (�˸�)
    //    public string alert_seq; // ��� ��ġ
    //    public string alert_type; // �˸� ���� (ȭ�� ���� ����...)
    //    public string currentAlert; // �︰ ����
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
            // ���ø����̼��� Resume ���¿� ���� �� ������ �ڵ�
            if (_pluginInstance != null)
            {
                _pluginInstance.Call("getSaveSendData");
            }
        }
        else
        {
            Debug.Log("Application Lost Focus (Might be Paused)");
            // ���ø����̼��� ��Ŀ���� �Ҿ��� �� ������ �ڵ�
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

    // �� �޼���� Android���� Unity�� �����͸� ������ �� ȣ��˴ϴ�.
    public void OnApiResponseReceived(string jsonString)
    {
        Debug.Log($"kks getData");
        Debug.Log("�ȵ���̵�κ��� JSON ������ ����: " + jsonString);

        // JSON �����͸� ������ȭ�Ͽ� ��ü�� ��ȯ
        try
        {
            // AlertData ��ü�� ������ȭ
            AlertData alertData = JsonConvert.DeserializeObject<AlertData>(jsonString);

            string categoryToText = "";

            switch (alertData.Category)
            {
                case 1:
                    categoryToText = "ȭ��";
                    break;
                case 2:
                    categoryToText = "����";
                    break;
                case 3:
                    categoryToText = "�ܼ�";
                    break;
                case 4:
                    categoryToText = "��� ����";
                    break;
                case 5:
                    categoryToText = "���";
                    break;
            }
            firebaseInit.JsonNotification(alertData.DeviceID, categoryToText, alertData.Floor, alertData.Location, alertData.CreateDate, alertData.ManagementNumber);
        }
        catch (JsonException ex)
        {
            Debug.LogError("JSON �Ľ� ����: " + ex.Message);
        }
    }

    private void ProcessApiResponse(string jsonData)
    {
        Debug.Log($"kks jsonData : {jsonData}");
        // jsonData�� �Ľ��ϰų� ������ ������ ��ȯ
        // ���� ���, JSON ��ƿ��Ƽ�� ����Ͽ� �����͸� �Ľ��� �� �ֽ��ϴ�.
        // MyData data = JsonUtility.FromJson<MyData>(jsonData);

        // �����͸� Ȱ���� ���� ���� �߰�
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