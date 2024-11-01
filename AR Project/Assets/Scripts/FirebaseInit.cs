using Firebase;
using Firebase.Extensions;
using Firebase.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using Unity.Notifications.Android;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Windows;
using static AndroidCommunicate;

public class FirebaseInit : MonoBehaviour
{
    string CHANNEL_ID = "myChannel";
    int apiLevel;
    public GameObject notificationCanvas, notificationImage, notificationPanel, notificationFloorPlanImage, notificationText;
    public List<Texture> notificationTextures;

    public List<Texture> floorPlanImages;
    public List<Texture> floorPlanBaseImages;

    public int alert_seq;
    
    public List<Button> floorButtons;

    Dictionary<string, int> notificationImageKeyValue = new Dictionary<string, int>();

    public List<GameObject> notificationImages_location;
    public List<Texture> notificationImages;

    public List<GameObject> communicationObjects;

    Coroutine currentCoroutine;

    bool communicationError = false;

    //    void Start()
    //    {
    //        for (int i = 0; i < notificationTextures.Count; i++)
    //        {
    //            switch (notificationTextures[i].name)
    //            {
    //                case "fire":
    //                    notificationImageKeyValue.Add("ȭ��", i);
    //                    break;
    //                case "water":
    //                    notificationImageKeyValue.Add("����", i);
    //                    break;
    //                case "data":
    //                    notificationImageKeyValue.Add("������ ��� ����", i);
    //                    break;
    //                case "electric":
    //                    notificationImageKeyValue.Add("����", i);
    //                    break;
    //                case "air":
    //                    notificationImageKeyValue.Add("�ó��� ������ �ս� ����", i);
    //                    break;
    //            }
    //        }
    //#if UNITY_ANDROID && !UNITY_EDITOR
    //        InitializeAndroidLocalPush();
    //        InitializeFCM();
    //#endif
    //    }

    //public void InitializeAndroidLocalPush()
    //{
    //    Debug.Log($"KKS : Enter Push");
    //    string androidInfo = SystemInfo.operatingSystem;
    //    Debug.Log("androidInfo: " + androidInfo);
    //    apiLevel = int.Parse(androidInfo.Substring(androidInfo.IndexOf("-") + 1, 2));
    //    Debug.Log("apiLevel: " + apiLevel);

    //    if (apiLevel >= 33 &&
    //        !Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
    //    {
    //        Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
    //    }

    //    if (apiLevel >= 26)
    //    {
    //        var channel = new AndroidNotificationChannel()
    //        {
    //            Id = CHANNEL_ID,
    //            Name = "test",
    //            Importance = Importance.High,
    //            Description = "for test",
    //        };
    //        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    //    }
    //}

    //public void InitializeFCM()
    //{
    //    Debug.Log($"KKS : Enter FCM");
    //    FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
    //        var dependencyStatus = task.Result;
    //        if (dependencyStatus == DependencyStatus.Available)
    //        {
    //            Debug.Log("Google Play version OK");

    //            FirebaseMessaging.TokenReceived += OnTokenReceived;
    //            FirebaseMessaging.MessageReceived += OnMessageReceived;
    //            FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(task => {
    //                Debug.Log("push permission: " + task.Status.ToString());
    //            });
    //        }
    //        else
    //        {
    //            Debug.LogError(string.Format(
    //                "Could not resolve all Firebase dependencies: {0}",
    //                dependencyStatus
    //            ));
    //        }
    //    });
    //}

    public void NotificationClick()
    {
        Debug.Log($"KKS : Enter NotiClick");
        notificationImage.SetActive(false);
        notificationPanel.SetActive(true);
        notificationFloorPlanImage.SetActive(true);
        notificationFloorPlanImage.GetComponent<RawImage>().texture = floorPlanImages[0];
        // �˸� â ���� �� ��
    }

    public void NotificationClose()
    {
        Debug.Log($"KKS : NotiClose");
        notificationImage.SetActive(true);
        notificationCanvas.SetActive(false);
        notificationPanel.SetActive(false);

        // �˸� â ������ ��
        Debug.Log($"kks close : {alert_seq}");
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }

        StartCoroutine(PUTAlertRelease($"http://work.allione.kr:9080/alerts_release/{alert_seq}"));

        communicationError = false;
    }

    IEnumerator PUTAlertRelease(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                GlobalVariable.Instance.ShowToast("�˸��� ���� �Ǿ����ϴ�.");
            }
        }
    }

    //public void OnTokenReceived(object sender, TokenReceivedEventArgs token)
    //{
    //    Debug.Log("OnTokenReceived: " + token.Token);
    //}

    //public void OnMessageReceived(object sender, MessageReceivedEventArgs e)
    //{
    //    Debug.Log($"KKS : OnMessageReceived");
    //    string type = "";
    //    string title = "";
    //    string body = "";

    //    // for notification message
    //    if (e.Message.Notification != null)
    //    {
    //        type = "notification";
    //        title = e.Message.Notification.Title;
    //        body = e.Message.Notification.Body;
    //    }
    //    // for data message
    //    else if (e.Message.Data.Count > 0)
    //    {
    //        type = "data";
    //        title = e.Message.Data["title"];
    //        body = e.Message.Data["body"];

    //        foreach (var pair in e.Message.Data)
    //        {
    //            Debug.Log($"Message data:{pair.Key}: {pair.Value}");
    //        }
    //    }
    //    Debug.Log("message type: " + type + ", title: " + title + ", body: " + body);

    //    if(!notificationCanvas.activeSelf)
    //    {
    //        notificationCanvas.SetActive(true);
    //    }

    //    string[] bodySplit = body.Split("/");

    //    notificationText.GetComponent<TextMeshProUGUI>().text = $"�˸� : {bodySplit[0]}\n" +
    //        $"��ġ : {bodySplit[1]}\n" +
    //        $"�Ͻ� : {bodySplit[2]}";

    //    // ���ø����̼� Ȱ��ȭ ���̸� �ٷ� ���� ���
    //    if (Application.isFocused)
    //    {
    //        // x��, xx���� �и�
    //        int floor = int.Parse(bodySplit[1].Split("��")[0]);

    //        notificationPanel.SetActive(true);
    //        notificationFloorPlanImage.SetActive(true);
    //        notificationFloorPlanImage.GetComponent<RawImage>().texture = floorPlanImages[0];

    //        floorButtons[floor - 1].GetComponent<Button>().Select();
    //        EventSystem.current.SetSelectedGameObject(floorButtons[floor - 1].gameObject);
    //    }

    //    SetNotificationImage(bodySplit[0]);

    //    var notification = new AndroidNotification();
    //    notification.SmallIcon = "icon_0";
    //    notification.Title = title;
    //    notification.Text = body;

    //    if (apiLevel >= 26)
    //    {
    //        AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);
    //    }
    //    else
    //    {
    //        Debug.LogError("Android 8.0 �̻��� ����̽������� Ǫ�� �˸��� ���������� ǥ�õ˴ϴ�.");
    //    }
    //}

    //private void SetNotificationImage(string value)
    //{
    //    switch (value)
    //    {
    //        case "ȭ��":
    //            notificationImage.GetComponent<RawImage>().texture = notificationTextures[0];
    //            StartExitScript();
    //            break;

    //        case "����":
    //            notificationImage.GetComponent<RawImage>().texture = notificationTextures[1];
    //            break;

    //        case "����":
    //            notificationImage.GetComponent<RawImage>().texture = notificationTextures[2];
    //            break;

    //        case "������ ��� ����":
    //            notificationImage.GetComponent<RawImage>().texture = notificationTextures[3];
    //            break;

    //        case "�ó��� ������ �ս� ����":
    //            notificationImage.GetComponent<RawImage>().texture = notificationTextures[4];
    //            break;
    //    }
    //}

    public void FloorPlanImageChange(int floor)
    {
        if (communicationError)
        {
            notificationFloorPlanImage.GetComponent<RawImage>().texture = floorPlanBaseImages[floor - 1];
        }
        else
        {
            notificationFloorPlanImage.GetComponent<RawImage>().texture = floorPlanImages[floor - 1];
        }

        for (int i = 0; i < notificationImages_location.Count; i++)
        {
            notificationImages_location[i].SetActive(false);
        }

        for(int i = 0; i < floorButtons.Count; i++)
        {
            floorButtons[i].GetComponent<ButtonImageChange>().SetImageUnSelect();
        }

        floorButtons[floor - 1].GetComponent<ButtonImageChange>().SetImageSelect();
    }

    public void JsonNotification(int alert_id, string alert_type, int floor, string location, string createDate, string managementNumber)
    {
        alert_seq = alert_id;

        if (!notificationCanvas.activeSelf)
        {
            notificationCanvas.SetActive(true);
        }

        string _location = location == "CENTER" ? "�߾�" : location+"ȣ";

        notificationText.GetComponent<TextMeshProUGUI>().text = $"�˸� �߻� : {alert_type}\n" +
            $"�߻� ��ġ : {floor}�� {_location}\n" +
            $"�߻��ð� : {createDate}";

        // ���ø����̼� Ȱ��ȭ ���̸� �ٷ� ���� ���
        if (Application.isFocused)
        {
            foreach(var i in floorButtons)
            {
                if (floorButtons[floor - 1] == i)
                {
                    floorButtons[floor - 1].GetComponent<ButtonImageChange>().SetImageSelect();
                }
                else
                {
                    i.GetComponent<ButtonImageChange>().SetImageUnSelect();
                }
            }

            foreach(var i in notificationImages_location)
            {
                i.SetActive(false);
            }

            foreach(var i in communicationObjects)
            {
                i.SetActive(false); 
            }
        
            notificationPanel.SetActive(true);
            notificationFloorPlanImage.SetActive(true);
            notificationFloorPlanImage.GetComponent<RawImage>().texture = floorPlanImages[floor - 1];

            if (alert_type == "��� ����")
            {
                communicationError = true;

                foreach (var obj in communicationObjects)
                {
                    if (obj.name == managementNumber)
                    {
                        obj.SetActive(true);
                        currentCoroutine = StartCoroutine(ColorImagesChangeActive(obj));
                    }
                    else
                    {
                        obj.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (var i in notificationImages_location)
                {
                    if (i.name == location)
                    {
                        i.SetActive(true);
                        switch (alert_type)
                        {
                            case "ȭ��":
                                for(int index = 0; index < i.transform.childCount; index++)
                                {
                                    i.transform.GetChild(index).gameObject.SetActive(false);
                                }

                                i.transform.GetChild(0).gameObject.SetActive(true);
                                break;
                            case "����":
                                i.transform.GetChild(0).gameObject.SetActive(true);
                                break;
                            case "���":
                                foreach(var enter in notificationImages_location)
                                {
                                    if(enter.name == "8210" || enter.name == "8225")
                                    {
                                        enter.SetActive(true);
                                        enter.transform.GetChild(0).gameObject.SetActive(false);
                                        if (enter.name == "8210")
                                        {
                                            StartCoroutine(ColorImagesChangeActive(enter.transform.GetChild(1).gameObject));
                                            StartCoroutine(ColorImagesChangeActive(enter.transform.GetChild(2).gameObject));
                                        }
                                    }
                                    else
                                    {
                                        enter.SetActive(false);
                                    }
                                }

                                i.GetComponent<RawImage>().texture = notificationImages[3];
                                break;
                        }
                    }
                    else
                    {
                        i.SetActive(false);
                        if (alert_type == "���")
                        {
                            notificationImages_location[4].SetActive(true);
                            notificationImages_location[4].transform.GetChild(0).gameObject.SetActive(false);
                            notificationImages_location[4].transform.GetChild(1).gameObject.SetActive(true);
                            notificationImages_location[4].transform.GetChild(2).gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        if(alert_type == "ȭ��")
        {
            StartExitScript();
        }
        
        if(alert_type == "���")
        {
            notificationText.GetComponent<TextMeshProUGUI>().text = $"�˸� �߻� : {alert_type}\n" +
                $"�߻� ��ġ : {floor}�� {"8210ȣ"}\n" +
                $"�߻��ð� : {createDate}";
        }
    }


    IEnumerator ColorImagesChangeActive(GameObject obj)
    {
        // �ʱ� Ȱ��ȭ ����
        bool isActive = true;

        while (true)
        {
            // Ȱ��ȭ ���� ���
            isActive = !isActive;

            obj.SetActive(isActive);

            // 1�� ���
            yield return new WaitForSeconds(1f);
        }
    }

    public void StartExitScript()
    {
        Debug.Log($"kks GetComponent<TrackedImageInfomation1>().currentTrackingObject : {GetComponent<TrackedImageInfomation1>().currentTrackingObject}");
        if (GetComponent<TrackedImageInfomation1>().currentTrackingObject.GetComponentInChildren<ExitScript>(true))
        {
            Debug.Log($"kks exist ExitScript");
            var exit = GetComponent<TrackedImageInfomation1>().currentTrackingObject.GetComponentsInChildren<ExitScript>(true);

            foreach (var i in exit)
            {
                Debug.Log($"kks exist foreach");

                i.StartExit();
            }
        }
    }
}
