using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static UnityEngine.GraphicsBuffer;

public class Alerts
{
    public int id;
    public int alert1;
    public int alert2;
    public int alert3;
    public int alert4;
    public int alert5;
}

public class Devices
{
    public int id;
    public string type;
    public string modelName;
    public string location;
    public string installationDate;
    public int currentStatus;
    public string productName;
    public string managementNumber;
    public string manufacturer;
    public string purchaseDate;
}

public static class JsonHelper
{
    // JSON 배열 파싱을 도와주는 유틸리티
    public static List<T> FromJson<T>(string json)
    {
        string newJson = "{\"Items\":" + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}

public class TrackedImageInfomation1 : MonoBehaviour
{
    bool[] isObjectPlaced = new bool[5];

    public XROrigin xrOrigin;

    public ARTrackedImageManager trackedImageManager;
    public GameObject[] arObjectPrefab;
    public Transform mainTransform = null;

    public GameObject placeListCanvas;
    public GameObject placeListHideButton;
    public GameObject placeListShowButton;

    [HideInInspector]
    public GameObject createdPrefab;

    [HideInInspector]
    public GameObject currentTrackingObject;

    [HideInInspector]
    public string currentForward = "";

    public GameObject currentForwardObject;

    // API에서 받은 응답 데이터를 처리하는 함수 (필요시 구현)
    void ProcessResponse(string jsonResponse)
    {
        // JSON을 Unity에서 다루기 위해 C# 객체로 변환하는 등의 처리
        // 예시: JsonUtility를 사용해 데이터를 파싱할 수 있습니다
        // EquipmentData equipment = JsonUtility.FromJson<EquipmentData>(jsonResponse);
        // 필요한 장치만 출력하거나 사용

        List<Devices> devices = JsonConvert.DeserializeObject<List<Devices>>(jsonResponse);

        // 필요한 장치만 출력하거나 사용
        foreach (Devices device in devices)
        {
            Debug.Log("Name: " + device.modelName);
            // 필요한 데이터만 사용
            if (device.modelName.StartsWith("pc"))
            {
                // "pcmain"인 경우 처리
                if (device.modelName == "pcmain")
                {
                    Debug.Log("Using device: pcmain");

                    GameObject obj = GameObject.Find("pcmain");

                    Material mat = Instantiate(obj.GetComponent<Renderer>().material);

                    mat.color = device.currentStatus == 1 ? Color.red : Color.blue;  // 원하는 색상으로 처리
                    obj.GetComponent<Renderer>().material = mat;
                }
                else
                {
                    int deviceNumber = int.Parse(device.modelName.Substring(2));
                    if (deviceNumber >= 1 && deviceNumber <= 41)
                    {
                        // 여기에 필요한 로직 추가 (예: 장치 처리)
                        Debug.Log("Using device: " + deviceNumber);

                        GameObject obj = GameObject.Find($"pc{deviceNumber}");

                        Material mat = Instantiate(obj.GetComponent<Renderer>().material);

                        mat.color = device.currentStatus == 1 ? Color.red : Color.blue;
                        obj.GetComponent<Renderer>().material = mat;
                    }
                }
            }
        }
    }

    // API 호출 함수
    IEnumerator GetInstalledEquipment(string equipmentName, string location)
    {
        while (true) // 무한 루프
        {
            Debug.Log("kks start Coroutine");
            string url;
            // URL 생성 (API의 엔드포인트에 쿼리 파라미터 추가)
            if (location != "")
            {
                url = $"http://bola.iptime.org:9080/device/searchlocation/?location={location}";
            }
            else
            {
                url = $"http://192.168.1.155:9080/device/all/";
            }

            Debug.Log("kks url : " + url);
            // UnityWebRequest를 사용해 GET 요청 보내기
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // 요청을 보내고 응답이 올 때까지 기다림
                yield return webRequest.SendWebRequest();

                // 네트워크 에러 체크
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("KKS Error: " + webRequest.error);
                }
                else
                {
                    // 성공적으로 데이터를 받았을 때
                    Debug.Log("KKS Response: " + webRequest.downloadHandler.text);

                    // 받은 데이터를 처리 (필요한 경우 JSON 파싱 등)
                    ProcessResponse(webRequest.downloadHandler.text);
                }
            }
            // 5초 대기
            yield return new WaitForSeconds(5);
        }
    }
    void OnEnable()
    {
        trackedImageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    void OnDisable()
    {
        trackedImageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        // 새롭게 트래킹된 이미지에 대해 처리
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            CreateOrUpdateARObject(trackedImage);
        }

        // 업데이트된 이미지에 대해 처리
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            CreateOrUpdateARObject(trackedImage);
        }
    }

    public static void DeepCopyTransform(Transform source, Transform target)
    {
        // Position, Rotation, Scale 복사
        target.position = source.position;
        target.rotation = source.rotation;
        target.localScale = source.localScale;

        // 자식들까지 복사하기 위해 재귀적으로 호출
        foreach (Transform child in source)
        {
            // 새 자식 오브젝트를 생성
            GameObject newChild = new GameObject(child.name);
            newChild.transform.SetParent(target); // 새로운 자식의 부모를 설정
            DeepCopyTransform(child, newChild.transform); // 재귀적으로 자식의 Transform 복사
        }
    }

    void CreateOrUpdateARObject(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage.name == "8221" || GlobalVariable.Instance.last_qrcode == "8221")
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                // 객체가 이미 배치된 상태인지 확인
                if (!isObjectPlaced[0])
                {
                    GlobalVariable.Instance.isSeeAble = false;
                    mainTransform = null;

                    // 다른 객체 비활성화
                    arObjectPrefab[1].SetActive(false);
                    arObjectPrefab[2].SetActive(false);
                    arObjectPrefab[3].SetActive(false);
                    arObjectPrefab[4].SetActive(false);

                    isObjectPlaced[1] = false;
                    isObjectPlaced[2] = false;
                    isObjectPlaced[3] = false;
                    isObjectPlaced[4] = false;

                    currentTrackingObject = arObjectPrefab[0];

                    // 트래킹된 이미지의 위치 저장
                    Vector3 initialPosition = trackedImage.transform.position + Camera.main.transform.forward * 1;

                    GameObject spawnedObject = arObjectPrefab[0];
                    spawnedObject.SetActive(true);

                    // 객체의 위치를 최초 트래킹된 이미지의 위치로 고정
                    spawnedObject.transform.position = initialPosition; // 최초 위치 고정
                    spawnedObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);

                    // 객체 배치 완료 플래그 설정
                    isObjectPlaced[0] = true;

                    Debug.Log("Object's position fixed at: " + initialPosition);
                }
                else
                {
                    Debug.Log("Object's already placed and position is fixed.");
                }
            }
        }

        if (trackedImage.referenceImage.name == "8119" || GlobalVariable.Instance.last_qrcode == "8119")
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                // 객체가 이미 배치된 상태인지 확인
                if (!isObjectPlaced[1])
                {
                    GlobalVariable.Instance.isSeeAble = false;
                    mainTransform = null;

                    // 다른 객체 비활성화
                    arObjectPrefab[0].SetActive(false);
                    arObjectPrefab[2].SetActive(false);
                    arObjectPrefab[3].SetActive(false);
                    arObjectPrefab[4].SetActive(false);

                    isObjectPlaced[0] = false;
                    isObjectPlaced[2] = false;
                    isObjectPlaced[3] = false;
                    isObjectPlaced[4] = false;

                    currentTrackingObject = arObjectPrefab[1];

                    // 트래킹된 이미지의 위치 저장
                    Vector3 initialPosition = trackedImage.transform.position + Camera.main.transform.forward * 1;

                    GameObject spawnedObject = arObjectPrefab[1];
                    spawnedObject.SetActive(true);

                    // 객체의 위치를 최초 트래킹된 이미지의 위치로 고정
                    spawnedObject.transform.position = initialPosition; // 최초 위치 고정
                    spawnedObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y + 90, 0);

                    // 객체 배치 완료 플래그 설정
                    isObjectPlaced[1] = true;

                    Debug.Log("Object's position fixed at: " + initialPosition);
                }
                else
                {
                    Debug.Log("Object's already placed and position is fixed.");
                }
            }
            // 2차년도 부분
            //placeListCanvas.SetActive(true);
        }

        if (trackedImage.referenceImage.name == "8212-2" || GlobalVariable.Instance.last_qrcode == "8212-2")
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                // 객체가 이미 배치된 상태인지 확인
                if (!isObjectPlaced[2])
                {
                    GlobalVariable.Instance.isSeeAble = false;
                    mainTransform = null;

                    // 다른 객체 비활성화
                    arObjectPrefab[0].SetActive(false);
                    arObjectPrefab[1].SetActive(false);
                    arObjectPrefab[3].SetActive(false);
                    arObjectPrefab[4].SetActive(false);

                    isObjectPlaced[0] = false;
                    isObjectPlaced[1] = false;
                    isObjectPlaced[3] = false;
                    isObjectPlaced[4] = false;

                    currentTrackingObject = arObjectPrefab[2];

                    // 트래킹된 이미지의 위치 저장
                    Vector3 initialPosition = trackedImage.transform.position + Camera.main.transform.forward * 1;

                    GameObject spawnedObject = arObjectPrefab[2];
                    spawnedObject.SetActive(true);

                    // 객체의 위치를 최초 트래킹된 이미지의 위치로 고정
                    spawnedObject.transform.position = initialPosition; // 최초 위치 고정
                    spawnedObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y, 0);
                    
                    // 객체 배치 완료 플래그 설정
                    isObjectPlaced[2] = true;

                    Debug.Log("Object's position fixed at: " + initialPosition);
                }
                else
                {
                    Debug.Log("Object's already placed and position is fixed.");
                }
            }
            // 2차년도 부분
            //placeListCanvas.SetActive(true);
        }

        if (trackedImage.referenceImage.name == "box" || GlobalVariable.Instance.last_qrcode == "box")
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                // 객체가 이미 배치된 상태인지 확인
                if (!isObjectPlaced[3])
                {
                    GlobalVariable.Instance.isSeeAble = false;
                    mainTransform = null;

                    // 다른 객체 비활성화
                    arObjectPrefab[0].SetActive(false);
                    arObjectPrefab[1].SetActive(false);
                    arObjectPrefab[2].SetActive(false);
                    arObjectPrefab[4].SetActive(false);

                    isObjectPlaced[0] = false;
                    isObjectPlaced[1] = false;
                    isObjectPlaced[2] = false;
                    isObjectPlaced[4] = false;

                    currentTrackingObject = arObjectPrefab[3];

                    // 트래킹된 이미지의 위치 저장
                    Vector3 initialPosition = trackedImage.transform.position + Camera.main.transform.forward * 1f;

                    GameObject spawnedObject = arObjectPrefab[3];
                    spawnedObject.SetActive(true);

                    // 객체의 위치를 최초 트래킹된 이미지의 위치로 고정
                    spawnedObject.transform.position = initialPosition; // 최초 위치 고정
                    spawnedObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y + 270, 0);

                    // 객체 배치 완료 플래그 설정
                    isObjectPlaced[3] = true;

                    Debug.Log("Object's position fixed at: " + initialPosition);
                }
                else
                {
                    Debug.Log("Object's already placed and position is fixed.");
                }
            }
            // 2차년도 부분
            //placeListCanvas.SetActive(true);
        }


        if (trackedImage.referenceImage.name == "module" || GlobalVariable.Instance.last_qrcode == "module")
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                // 객체가 이미 배치된 상태인지 확인
                if (!isObjectPlaced[4])
                {
                    GlobalVariable.Instance.isSeeAble = false;
                    arObjectPrefab[0].SetActive(false);
                    arObjectPrefab[1].SetActive(false);
                    arObjectPrefab[2].SetActive(false);
                    arObjectPrefab[3].SetActive(false);

                    isObjectPlaced[0] = false;
                    isObjectPlaced[1] = false;
                    isObjectPlaced[2] = false;
                    isObjectPlaced[3] = false;

                    currentTrackingObject = arObjectPrefab[4];

                    // 트래킹된 이미지의 위치 저장
                    Vector3 initialPosition = trackedImage.transform.position + Camera.main.transform.forward * 1;

                    GameObject spawnedObject = arObjectPrefab[4];
                    spawnedObject.SetActive(true);

                    // 객체의 위치를 최초 트래킹된 이미지의 위치로 고정
                    spawnedObject.transform.position = initialPosition; // 최초 위치 고정
                    spawnedObject.transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.y + 90, 0);

                    // 객체 배치 완료 플래그 설정
                    isObjectPlaced[4] = true;

                    Debug.Log("Object's position fixed at: " + initialPosition);
                }
                else
                {
                    Debug.Log("Object's already placed and position is fixed.");
                }
            }
            // 2차년도 부분
            //placeListCanvas.SetActive(true);
        }
    }
}