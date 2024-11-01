using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreatePlaceObject : MonoBehaviour
{
    public List<GameObject> placeableObjects;
    public List<GameObject> originalPlaceableObjects;

    bool placeMode = false;

    GameObject ceiling;

    GameObject placeingObject;

    GameObject buttonCanvas;

    int placeID = int.MaxValue;

    // 생성된 오브젝트 리셋 기능이 생길 수 있음에 대비해 리스트에 추가
    List<GameObject> placedNewObjects = new List<GameObject>();

    public GameObject imageCanvas;
    public List<Texture> imageList;


    public TextMeshProUGUI testText;

    /// <summary>
    /// 생성된 오브젝트 모두 파괴 (리셋)
    /// </summary>
    void ResetObjects()
    {
        for (int i = placedNewObjects.Count - 1; i >= 0; i--)
        {
            Destroy(placedNewObjects[i]);
            placedNewObjects.RemoveAt(i);
        }
    }

    void Update()
    {
        //if(GetComponent<TrackedImageInfomation1>().createdPrefab != null && GetComponent<TrackedImageInfomation1>().currentTrackingObjectName == "room1")
        //{
        //    ceiling = GetComponent<TrackedImageInfomation1>().createdPrefab.GetComponent<IndoorObject>().placeableObject;
        //}

        // 터치
        if (Input.touchCount > 0)
        {
            // UI 터치시 반환
            if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit[] hits = Physics.RaycastAll(ray);

                // 배치모드일때
                if (hits.Length > 0)
                {
                    PlaceModeTouch(hits);
                }
                if (hits.Length > 0)
                {
                    // 배치 모드 아닐 때
                    if (!placeMode)
                    {
                        Debug.Log("KKS 터치 배치모드 X");
                        for (int i = 0; i < hits.Length; i++)
                        {
                            for (int j = 0; j < hits.Length; j++)
                            {
                                if (hits[j].collider.CompareTag("ObjectStatusButton"))
                                {
                                    Debug.Log($"kks 터치 테스트 hits[j].collider : {hits[j].collider.name}");
                                    if (hits[j].collider.name.Equals("Button_explane")
                                        || hits[j].collider.name.Equals("Button_event")
                                        || hits[j].collider.name.Equals("Button_history")
                                        || hits[j].collider.name.Equals("Button_as")
                                        )
                                    {
                                        //hits[j].collider.GetComponent<ObjectCanvasConnector>().SelectButtonExplane();
                                    }
                                    hits[j].collider.transform.GetComponent<Button>().onClick.Invoke();
                                    return;
                                }
                            }

                            if (hits[i].collider.CompareTag("UIButton"))
                            {
                                if (hits[i].collider.name == "Button_Delete")
                                {
                                    placedNewObjects.Remove(hits[i].collider.transform.root.gameObject);
                                    Destroy(hits[i].collider.transform.root.gameObject);
                                    break;
                                }

                                if (hits[i].collider.name.Equals("CloseFloorPlan Button"))
                                {
                                    OnClickCloseImageCanvasButton(false);
                                    //imageCanvas.SetActive(false);
                                }
                            }

                            if (hits[i].collider.CompareTag("Touchable"))
                            {
                                Debug.Log($"kks touchable name : {hits[i].collider.name}");
                                if (hits[i].collider.GetComponent<TextShow>())
                                {
                                    Debug.Log($"kks hit object textshow true");
                                    hits[i].collider.GetComponent<TextShow>().SetVisible();
                                    break;
                                }
                            }

                            if (hits[i].collider.CompareTag("PlacedObject"))
                            {
                                // Active 상태가 실행부터 false로 설정되어 있는 경우에는 true값을 넣어 줌.
                                if (!hits[i].collider.transform.GetComponentInChildren<Canvas>(true).gameObject.activeSelf)
                                {
                                    hits[i].collider.transform.GetComponentInChildren<Canvas>(true).gameObject.SetActive(true);
                                    Transform canvas = hits[i].collider.transform.GetComponentInChildren<Canvas>().transform;
                                    canvas.rotation = Quaternion.LookRotation(canvas.position - Camera.main.transform.position);
                                }
                                else
                                {
                                    hits[i].collider.transform.GetComponentInChildren<Canvas>(true).gameObject.SetActive(false);
                                }
                            }

                            Debug.Log("KKS 배전반인지 확인 진입 시도");
                            if (hits[i].collider.name.Contains("distribution_box_"))
                            {
                                Debug.Log("KKS 배전반 확인");

                                OnClickCloseImageCanvasButton(true);
                                //imageCanvas.SetActive(true);
                                imageCanvas.transform.position = Camera.main.transform.position + GlobalVariable.Instance.imageCanvas_offset;
                                switch (hits[i].collider.name)
                                {
                                    case "distribution_box_101":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[0];
                                        break;
                                    case "distribution_box_102":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[1];
                                        break;
                                    case "distribution_box_103":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[2];
                                        break;
                                    case "distribution_box_104":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[3];
                                        break;
                                    case "distribution_box_105":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[0];
                                        break;
                                    case "distribution_box_106":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[1];
                                        break;
                                    case "distribution_box_107":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[2];
                                        break;
                                    case "distribution_box_201":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[3];
                                        break;
                                    case "distribution_box_202":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[0];
                                        break;
                                    case "distribution_box_203":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[1];
                                        break;
                                    case "distribution_box_204":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[2];
                                        break;
                                    case "distribution_box_205":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[3];
                                        break;
                                    case "distribution_box_206":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[0];
                                        break;
                                    case "distribution_box_207":
                                        imageCanvas.GetComponentInChildren<RawImage>(true).texture = imageList[1];
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void PlaceModeTouch(RaycastHit[] hits)
    {
        if (placeMode)
        {
            bool touched = false;
            // 터치한 구간에 ui가 있을시 ui 우선 순위
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("UIButton"))
                {
                    touched = true;

                    if (hits[i].collider.name == "Button_OK")
                    {
                        GameObject newPlaceObject = Instantiate(originalPlaceableObjects[placeID]);
                        placedNewObjects.Add(newPlaceObject);

                        newPlaceObject.transform.position = placeingObject.transform.position;
                        newPlaceObject.transform.rotation = placeingObject.transform.rotation;
                        newPlaceObject.transform.localScale = placeingObject.transform.localScale;
                        newPlaceObject.transform.parent = GetComponent<TrackedImageInfomation1>().createdPrefab.transform;

                        EndPlaceMode();
                    }
                    else if (hits[i].collider.name == "Button_Cancel")
                    {
                        EndPlaceMode();
                    }
                    else if (hits[i].collider.name == "Button_Rotate")
                    {
                        switch (placeID)
                        {
                            case 0:
                                placeingObject.transform.Rotate(0, 30, 0);
                                break;
                            case 2:
                                placeingObject.transform.Rotate(0, 0, 30);
                                break;
                        }
                    }
                    break;
                }
            }

            if (touched) return;

            // 터치 구간에 UI가 없었을 경우
            for (int i = 0; i < hits.Length; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject() == false)
                {
                    Vector3 previousPosition;
                    Quaternion previousRotation;
                    Vector3 newPosition;
                    if (placeID == 1)
                    {
                        if (hits[i].collider.name.Contains("Wall"))
                        {
                            previousPosition = placeingObject.transform.position;
                            previousRotation = placeingObject.transform.rotation;
                            switch (hits[i].collider.name)
                            {
                                case "Back Wall":
                                    newPosition = hits[i].point;
                                    newPosition.z = hits[i].collider.transform.position.z;
                                    placeingObject.transform.position = newPosition;
                                    placeingObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                                    break;
                                case "Front Wall":
                                    newPosition = hits[i].point;
                                    newPosition.z = hits[i].collider.transform.position.z;
                                    placeingObject.transform.position = newPosition;
                                    placeingObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                                    break;
                                case "Left Wall":
                                    newPosition = hits[i].point;
                                    newPosition.x = hits[i].collider.transform.position.x;
                                    placeingObject.transform.position = newPosition;
                                    placeingObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                                    break;
                                case "Right Wall":
                                    newPosition = hits[i].point;
                                    newPosition.x = hits[i].collider.transform.position.x;
                                    placeingObject.transform.position = newPosition;
                                    placeingObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                                    break;
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (hits[i].collider.name == "Ceiling")
                        {
                            previousPosition = placeingObject.transform.position;
                            previousRotation = placeingObject.transform.rotation;
                            if (placeID == 2)
                            {
                                newPosition = hits[i].point;
                                newPosition.y = ceiling.transform.position.y; // y축 고정
                                placeingObject.transform.position = newPosition;
                            }
                            else
                            {
                                newPosition = hits[i].point;
                                newPosition.y = ceiling.transform.position.y; // y축 고정
                                placeingObject.transform.position = newPosition;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    public void OnClickCloseImageCanvasButton(bool value)
    {
        imageCanvas.SetActive(value);
        StartCoroutine(SendPostRequest());
    }

    // 서버에 POST 요청을 보내는 메서드
    IEnumerator SendPostRequest()
    {
        Debug.Log($"kks sendPosrtRequest");
        string url = "http://192.168.1.139/api/alarm/release";

        // seq 값을 포함한 JSON 데이터
        string jsonData = $"{{\"seq\" : {GetComponent<FirebaseInit>().alert_seq}}}";
        Debug.Log($"kks alert_seq : {GetComponent<FirebaseInit>().alert_seq}");

        // 요청 생성
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // Content-Type 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");

        // 요청 보내기
        yield return request.SendWebRequest();

        // 요청 결과 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    public void RoomItemListSet(GameObject obj)
    {
        switch(obj.name)
        {
            case "Button ListHide":
                GetComponent<TrackedImageInfomation1>().placeListShowButton.SetActive(true);
                GetComponent<TrackedImageInfomation1>().placeListCanvas.transform.GetChild(0).gameObject.SetActive(false);
                //GetComponent<FirebaseInit>().LocalNotification("notification", "알림", "화재/4층/4010호");
                obj.SetActive(false);
                break;
            case "Button ListShow":
                GetComponent<TrackedImageInfomation1>().placeListHideButton.SetActive(true);
                GetComponent<TrackedImageInfomation1>().placeListCanvas.transform.GetChild(0).gameObject.SetActive(true);
                obj.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// 오브젝트 배치 종료시 호출하는 함수
    /// </summary>
    private void EndPlaceMode()
    {
        Destroy(placeingObject);
        placeingObject = null;
        buttonCanvas.SetActive(true);
        placeMode = false;
        placeID = int.MaxValue;
    }

    /// <summary>
    /// OnClick 이벤트에 등록되어 있음
    /// </summary>
    /// <param name="btn"></param>
    public void OnCreateButtonClick(GameObject btn)
    {
        if(buttonCanvas == null)
        {
            buttonCanvas = btn.transform.root.gameObject;
        }

        GameObject prefab = GetComponent<TrackedImageInfomation1>().createdPrefab;

        // 2차년도 수정 必
        if (GetComponent<TrackedImageInfomation1>().currentTrackingObject != null)
        {
            if (prefab != null)
            {
                switch (btn.name)
                {
                    case "AirConditioner":
                        placeID = 0;
                        placeingObject = Instantiate(placeableObjects[placeID]);
                        placeingObject.transform.position = prefab.GetComponent<IndoorObject>().placeableObject.transform.position;
                        break;
                    case "TV":
                        placeID = 1;
                        placeingObject = Instantiate(placeableObjects[placeID]);
                        placeingObject.transform.position = prefab.GetComponent<IndoorObject>().wallObject.transform.position;
                        break;
                    case "FluorescentLight":
                        placeID = 2;
                        placeingObject = Instantiate(placeableObjects[placeID]);
                        placeingObject.transform.position = prefab.GetComponent<IndoorObject>().placeableObject.transform.position;
                        break;
                    case "reset":
                        ResetObjects();
                        break;
                }

                if(btn.name != "reset")
                {
                    buttonCanvas.SetActive(false);
                    placeMode = true;
                }
            }
        }
    }
}