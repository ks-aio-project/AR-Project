using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class FastAPIConnectorForPC : MonoBehaviour
{
    int deviceID;

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string newJson = "{\"items\":" + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
    public class Devices
    {
        public int DeviceID { get; set; }
        public string DeviceType { get; set; }
        public string ModelName { get; set; }
        public string Location { get; set; }
        public string InstallationDate { get; set; }
        public int CurrentStatus { get; set; }
        public string ProductName { get; set; }
        public string ManagementNumber { get; set; }
        public string Manufacturer { get; set; }
        public string PurchaseDate { get; set; }
    }

    void Start()
    {
        StartCoroutine(GetRequest($"http://allione.iptime.org:9080/device/search/?column=ManagementNumber&value={transform.parent.name}"));
    }

    IEnumerator GetRequest(string uri)
    {
        transform.GetComponent<Renderer>().material = Instantiate(transform.GetComponent<Renderer>().material);

        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                StartCoroutine(GetRequest($"http://allione.iptime.org:9080/device/search/?column=ManagementNumber&value={transform.parent.name}"));
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("test : " + www.downloadHandler.text);

                // JSON 배열을 List<Device>로 역직렬화
                List<Devices> devices = JsonConvert.DeserializeObject<List<Devices>>(www.downloadHandler.text);

                // 결과 출력
                foreach (var device in devices)
                {
                    Debug.Log(device.CurrentStatus);
                    deviceID = device.DeviceID;

                    if (device.CurrentStatus == 0)
                    {
                        transform.GetComponent<Renderer>().material.color = Color.blue;
                    }
                    else
                    {
                        transform.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }
        }
    }
}
