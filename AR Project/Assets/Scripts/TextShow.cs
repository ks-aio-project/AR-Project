using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using static TextShow;
using System.Security.Policy;
using System.Globalization;
using Vuplex.WebView;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class TextShow : MonoBehaviour
{
    public GameObject canvas;

    public GameObject info_name_number_text, info_model_code_text, info_install_date_text, info_power_usage_text, info_management_text;

    public GameObject event_history_text_1, event_history_text_2, event_history_text_3, event_history_text_4;
    public GameObject as_history_text_1, as_history_text_2, as_history_text_3, as_history_text_4;

    public Image[] as_history_images = new Image[4];

    public GameObject as_name_number_text, as_manufacturer_install_date_text, as_request_title, as_request_date, as_applicant, as_request_body;

    public GameObject provisioning_name_text, provisioning_location_text, provisioning_key_text, provisioning_management_text, provisioning_protocol_text, provisioning_mac_text;

    public GameObject button_provisioning_next, button_provisioning_previous, button_provisioning_save;

    public GameObject infoPanel, historyPanel, asPanel, provisioningPanel;

    public GameObject button_info, button_history, button_as, button_provisioning, button_close, button_request_as;

    public GameObject as_webViewObject, provisioning_webViewObject;

    public GameObject floorBaseImage;

    public List<GameObject> sphereImages;

    public Dictionary<string, List<GameObject>> objectGroups = new Dictionary<string, List<GameObject>>();


    [HideInInspector]
    public TMP_InputField[] eventHistoryTexts = new TMP_InputField[4];

    [HideInInspector]
    public TMP_InputField[] maintenanceHistoryTexts = new TMP_InputField[4];

    int textIndex = 0;

    [HideInInspector]
    public bool isVisible = false;

    int _DeviceID;
    string _ModelName;
    string _Manufacturer;
    string _PurchaseDate;

    string _ProductName;
    string _ManagementNumber;
    string _InstallationDate;

    int[] _MaintenanceID = new int[4];

    Coroutine currentImageActiveCoroutine;


    public class Devices
    {
        public int DeviceID;
        public string DeviceType;
        public string ModelName;
        public string Location;
        public DateTime? InstallationDate; // Nullable DateTime
        public int CurrentStatus;
        public string ProductName;
        public string ManagementNumber;
        public string Manufacturer;
        public DateTime? PurchaseDate;     // Nullable DateTime
        public float? LocationX;
        public float? LocationY;
        public float? LocationZ;
        public int? Floor;
        public int? PowerConsumption;
    }

    public class Provisioning
    {
        public int DeviceID;
        public string management_number;
        public string model_name;
        public string location;
        public string mac;

        public string DeviceType;
        public string ModelName;
        public string Location;
        public DateTime? InstallationDate; // Nullable DateTime
        public int CurrentStatus;
        public string ProductName;
        public string ManagementNumber;
        public string Manufacturer;
        public DateTime? PurchaseDate;     // Nullable DateTime
        public float? LocationX;
        public float? LocationY;
        public float? LocationZ;
        public int? Floor;
        public int? PowerConsumption;
        public string Department;
        public string Manager;
        public string NetworkType;
        public string Mac;
    }

    public class ResponseData
    {
        public List<MaintenanceRecord> maintenance_list { get; set; }
        public List<EventRecord> events_list { get; set; }
    }

    public class MaintenanceRecord
    {
        public int MaintenanceID { get; set; }
        public int DeviceID { get; set; }
        public DateTime? RepairDate { get; set; }
        public string RepairDetails { get; set; }
        public string Title { get; set; }
        public string Applicant { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string MalfunctionSymptoms { get; set; }
        public int Status { get; set; }
    }

    public class EventRecord
    {
        public int EventRecordsID { get; set; }
        public int DeviceID { get; set; }
        public DateTime EventDate { get; set; }
        public string EventDetails { get; set; }
    }

    public class ServiceRequests
    {
        public int RequestID;
        public int DeviceID;
        public string RequestDate;
        public string Applicant;
        public string MalfunctionSymptoms;
    }

    [Serializable]
    public class RequestAS
    {
        public int device_id;
        public string title;
        public string applicant;
        public DateTime request_date;
        public string malfunction_symptoms;
    }

    private void Start()
    {
        StartCoroutine(GetInitRequest($"http://work.allione.kr:9080/device/search?column=ManagementNumber&value={transform.name}"));

        button_provisioning.SetActive(transform.name.Contains("MODULE"));
    }

    public void SetVisible()
    {
        Debug.Log($"kks textshow setvisible : {transform.name}");
        if (GlobalVariable.Instance.isSeeAble)
        {
            Debug.Log($"kks textshow setvisible return");
            return;
        }

        isVisible = !isVisible;

        if (canvas != null)
        {
            Debug.Log($"kks canvas not null");
            canvas.SetActive(isVisible);
            
            if (isVisible)
            {
                Debug.Log($"kks textshow isvisible true");
                GlobalVariable.Instance.isSeeAble = isVisible;

                button_info.GetComponent<ButtonImageChange>().SetImageSelect();
                button_as.GetComponent<ButtonImageChange>().SetImageUnSelect();
                button_history.GetComponent<ButtonImageChange>().SetImageUnSelect();
                button_provisioning.GetComponent<ButtonImageChange>().SetImageUnSelect();

                infoPanel.SetActive(true);
                historyPanel.SetActive(false);
                asPanel.SetActive(false);
                provisioningPanel.SetActive(false);

                StartCoroutine(GetRequest0($"http://work.allione.kr:9080/device/search?column=ManagementNumber&value={transform.name}"));
            }
        }
    }

    public void OnClickClose()
    {
        GlobalVariable.Instance.isSeeAble = false;

        canvas.SetActive(false);
    }

    public void ChangeTextIndex(int value)
    {
        Debug.Log($"Test Debug ChangeTextIndex : {value}");
        textIndex = value;

        infoPanel.SetActive(false);
        historyPanel.SetActive(false);
        asPanel.SetActive(false);
        provisioningPanel.SetActive(false);

        button_info.GetComponent<ButtonImageChange>().SetImageUnSelect();
        button_history.GetComponent<ButtonImageChange>().SetImageUnSelect();
        button_as.GetComponent<ButtonImageChange>().SetImageUnSelect();
        button_provisioning.GetComponent<ButtonImageChange>().SetImageUnSelect();

        switch (textIndex)
        {
            case 0:
                StartCoroutine(GetRequest0($"http://work.allione.kr:9080/device/search?column=ManagementNumber&value={transform.name}"));
                button_info.GetComponent<ButtonImageChange>().SetImageSelect();
                infoPanel.SetActive(true);
                break;
            case 1:
                StartCoroutine(GetRequest1($"http://work.allione.kr:9080/device/{transform.name}/event_maintenance"));
                button_history.GetComponent<ButtonImageChange>().SetImageSelect();
                historyPanel.SetActive(true);
                break;
            case 2:
                // AS
                button_as.GetComponent<ButtonImageChange>().SetImageSelect();
                asPanel.SetActive(true);

                as_name_number_text.GetComponent<TMP_InputField>().text =
                    string.IsNullOrEmpty(_ProductName) ? "-" : $"{_ProductName}";

                as_name_number_text.GetComponent<TMP_InputField>().text +=
                    string.IsNullOrEmpty(_ManagementNumber) ? "-" : $" / {_ManagementNumber}";

                as_manufacturer_install_date_text.GetComponent<TMP_InputField>().text =
                    string.IsNullOrEmpty(_Manufacturer) ? "-" : $"{_Manufacturer}";

                as_manufacturer_install_date_text.GetComponent<TMP_InputField>().text +=
                    string.IsNullOrEmpty(_InstallationDate) ? "-" : $" / {_InstallationDate}";

                DateTime today = DateTime.Now;

                // 오늘 날짜 바로 입력
                as_request_date.GetComponent<TMP_InputField>().text = today.ToString("yyyy-MM-ddTHH:mm:ss");
                if (as_webViewObject != null)
                {
                    if (transform.name.Contains("PC"))
                    {
                        as_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "https://www.youtube.com/embed/dpPjCnSl-i0?autoplay=1&mute=1&enablejsapi=1";
                    }
                    else if (transform.name.Contains("AC"))
                    {
                        as_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "https://www.youtube.com/embed/ggELi8v0hns?autoplay=1&mute=1&enablejsapi=1";
                    }
                    else if (transform.name.Contains("BEAM"))
                    {
                        as_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "https://www.youtube.com/embed/MrJdUJ_83JM?autoplay=1&mute=1&enablejsapi=1";
                    }
                    else if (transform.name.Contains("FAN"))
                    {
                        as_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "https://www.youtube.com/embed/i8fIXpSC28I?autoplay=1&mute=1&enablejsapi=1";
                    }
                    else if (transform.name.Contains("LIGHT"))
                    {
                        as_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "https://www.youtube.com/embed/ADShvubx3T0?autoplay=1&mute=1&enablejsapi=1";
                    }
                }
                break;
            case 3:
                switch(transform.name[^1])
                {
                    case '1':
                        provisioning_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "http://work.allione.kr:3000/public-dashboards/fcc5b27ea34d4805b23b85789dc192ad";
                        break;
                    case '2':
                        provisioning_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "http://work.allione.kr:3000/public-dashboards/950e1e154b824c2a84a781f6cb336391";
                        break;
                    case '3':
                        provisioning_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "http://work.allione.kr:3000/public-dashboards/3db5058f1d2644e89c1fab26de7fab6d";
                        break;
                    case '4':
                        provisioning_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "http://work.allione.kr:3000/public-dashboards/1041e2bbffb74972b0f0856ed01cb3a8";
                        break;
                    case '5':
                        provisioning_webViewObject.GetComponent<WebViewPrefab>().InitialUrl = "http://work.allione.kr:3000/public-dashboards/5d78273596884e85a84f1708896f384a";
                        break;
                }

                StartCoroutine(GetRequest2($"http://work.allione.kr:9080/device/search?column=ManagementNumber&value={transform.name}"));
                button_provisioning.GetComponent<ButtonImageChange>().SetImageSelect();
                provisioningPanel.SetActive(true);

                button_provisioning_previous.SetActive(false);
                button_provisioning_next.SetActive(true);
                break;
        }
    }

    public void RequestASSend()
    {
        button_request_as.SetActive(false);
        StartCoroutine(PostRequestAS("http://work.allione.kr:9080/maintenance"));
    }

    public void RequestProvisioningUpdate()
    {
        button_provisioning_save.SetActive(false);
        StartCoroutine(SaveProvisioning("http://work.allione.kr:9080/device/provisioning"));
    }

    IEnumerator SaveProvisioning(string uri)
    {
        Provisioning provisioning = new Provisioning
        {
            management_number = transform.name,
            model_name = provisioning_name_text.GetComponent<TMP_InputField>().text,
            location = provisioning_location_text.GetComponent<TMP_InputField>().text,
            mac = provisioning_mac_text.GetComponent<TMP_InputField>().text
        };

        // 데이터를 JSON 문자열로 직렬화
        string jsonData = JsonConvert.SerializeObject(provisioning);

        byte[] putData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // 요청 생성 - 메서드를 "PUT"으로 변경
        UnityWebRequest request = new UnityWebRequest(uri, "PUT");
        request.uploadHandler = new UploadHandlerRaw(putData);
        request.downloadHandler = new DownloadHandlerBuffer();

        // 헤더 설정 - API 예제에 맞게 "text/plain"으로 설정
        request.SetRequestHeader("Content-Type", "text/plain");
        request.SetRequestHeader("Accept", "application/json");

        // 요청 보내기
        yield return request.SendWebRequest();

        // 응답 처리
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"요청 실패: {request.error}, 응답 코드: {request.responseCode}");
            GlobalVariable.Instance.ShowToast("모듈 데이터 변경 실패. 추후에 재시도 해주세요.");
        }
        else
        {
            Debug.Log("요청 성공! 응답 데이터: " + request.downloadHandler.text);
            GlobalVariable.Instance.ShowToast("모듈 데이터 변경 완료");
            StartCoroutine(GetRequest2($"http://work.allione.kr:9080/device/search?column=ManagementNumber&value={transform.name}"));
        }
        button_provisioning_save.SetActive(true);
    }

    public void ChangeASState(GameObject obj)
    {
        switch (obj.name)
        {
            case "as_history_images1":
                if (string.IsNullOrEmpty(as_history_text_1.GetComponent<TMP_InputField>().text))
                {
                    return;
                }
                else
                {
                    string value = as_history_images[0].GetComponent<Image>().color == Color.red ? "1" : "0";
                    StartCoroutine(PostChangeASState($"http://work.allione.kr:9080/maintenance/{_MaintenanceID[0]}/{value}"));
                }
                break;

            case "as_history_images2":
                if (string.IsNullOrEmpty(as_history_text_2.GetComponent<TMP_InputField>().text))
                {
                    return;
                }
                else
                {
                    string value = as_history_images[1].GetComponent<Image>().color == Color.red ? "1" : "0";
                    StartCoroutine(PostChangeASState($"http://work.allione.kr:9080/maintenance/{_MaintenanceID[1]}/{value}"));
                }
                break;
            
            case "as_history_images3":
                if (string.IsNullOrEmpty(as_history_text_3.GetComponent<TMP_InputField>().text))
                {
                    return;
                }
                else
                {
                    string value = as_history_images[2].GetComponent<Image>().color == Color.red ? "1" : "0";
                    StartCoroutine(PostChangeASState($"http://work.allione.kr:9080/maintenance/{_MaintenanceID[2]}/{value}"));
                }
                break;
            
            case "as_history_images4":
                if (string.IsNullOrEmpty(as_history_text_4.GetComponent<TMP_InputField>().text))
                {
                    return;
                }
                else
                {
                    string value = as_history_images[3].GetComponent<Image>().color == Color.red ? "1" : "0";
                    StartCoroutine(PostChangeASState($"http://work.allione.kr:9080/maintenance/{_MaintenanceID[3]}/{value}"));
                }
                break;
        }
    }

    IEnumerator PostChangeASState(string url)
    {
        // UnityWebRequest로 PUT 요청을 생성합니다.
        using (UnityWebRequest request = UnityWebRequest.Put(url, ""))
        {
            // 요청을 보내고 응답을 기다립니다.
            yield return request.SendWebRequest();

            // 에러가 없는 경우 응답을 처리합니다.
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                StartCoroutine(GetRequest1($"http://work.allione.kr:9080/device/{transform.name}/event_maintenance"));
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }

    IEnumerator PostRequestAS(string uri)
    {
        // JSON 데이터를 ResponseData 객체로 역직렬화
        var settings = new JsonSerializerSettings
        {
            DateFormatString = "yyyy-MM-ddTHH:mm:ss"
        };

        // 전송할 데이터 객체 생성
        RequestAS data = new RequestAS
        {
            device_id = _DeviceID,
            title = as_request_title.GetComponent<TMP_InputField>().text,
            applicant = as_applicant.GetComponent<TMP_InputField>().text,
            request_date = DateTime.ParseExact(as_request_date.GetComponent<TMP_InputField>().text, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
            malfunction_symptoms = as_request_body.GetComponent<TMP_InputField>().text
        };

        // 데이터를 JSON 문자열로 직렬화
        string jsonData = JsonConvert.SerializeObject(data);
        Debug.Log("전송할 JSON 데이터: " + jsonData);

        // 바이트 배열로 변환
        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        // 요청 생성
        UnityWebRequest request = new UnityWebRequest(uri, "POST");
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();

        // 헤더 설정
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");

        // 요청 보내기
        yield return request.SendWebRequest();

        // 응답 처리
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("요청 실패: " + request.error);
            GlobalVariable.Instance.ShowToast("AS 접수 오류. 추후에 재시도 해주세요/");
        }
        else
        {
            Debug.Log("요청 성공! 응답 데이터: " + request.downloadHandler.text);
            as_request_title.GetComponent<TMP_InputField>().text = "";
            as_applicant.GetComponent<TMP_InputField>().text = "";
            as_request_body.GetComponent<TMP_InputField>().text = "";
            GlobalVariable.Instance.ShowToast("AS 접수 완료");
        }
        button_request_as.SetActive(true);
    }

    IEnumerator GetInitRequest(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                // JsonSerializerSettings 설정
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd",
                    NullValueHandling = NullValueHandling.Ignore
                };

                try
                {
                    // JSON 배열을 List<Devices>로 역직렬화
                    List<Devices> devices = JsonConvert.DeserializeObject<List<Devices>>(www.downloadHandler.text, settings);

                    // 결과 출력
                    foreach (var device in devices)
                    {
                        if (string.IsNullOrEmpty(_ModelName))
                        {
                            _DeviceID = device.DeviceID;
                            _ModelName = device.ModelName;
                            _Manufacturer = device.Manufacturer;
                            _PurchaseDate = device.PurchaseDate.HasValue ? device.PurchaseDate.Value.ToString("yyyy-MM-dd") : "";

                            _ProductName = device.ProductName;
                            _ManagementNumber = device.ManagementNumber;
                            _InstallationDate = device.InstallationDate.HasValue ? device.InstallationDate.Value.ToString("yyyy-MM-dd") : "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("GetInitRequest 역직렬화 중 오류 발생: " + ex.Message);
                }
            }
        }
    }

    IEnumerator GetRequest0(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("test : " + www.downloadHandler.text);

                // JsonSerializerSettings 설정
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd",
                    NullValueHandling = NullValueHandling.Ignore
                };

                try
                {
                    // JSON 배열을 List<Devices>로 역직렬화
                    List<Devices> devices = JsonConvert.DeserializeObject<List<Devices>>(www.downloadHandler.text, settings);

                    info_name_number_text.GetComponent<TMP_InputField>().text = "";
                    info_model_code_text.GetComponent<TMP_InputField>().text = "";
                    info_install_date_text.GetComponent<TMP_InputField>().text = "";
                    info_power_usage_text.GetComponent<TMP_InputField>().text = "";
                    info_management_text.GetComponent<TMP_InputField>().text = "";

                    // 결과 출력
                    foreach (var device in devices)
                    {
                        if (string.IsNullOrEmpty(_ModelName))
                        {
                            _DeviceID = device.DeviceID;
                            _ModelName = device.ModelName;
                            _Manufacturer = device.Manufacturer;
                            _PurchaseDate = device.PurchaseDate.HasValue ? device.PurchaseDate.Value.ToString("yyyy-MM-dd") : "";

                            _ProductName = device.ProductName;
                            _ManagementNumber = device.ManagementNumber;
                            _InstallationDate = device.InstallationDate.HasValue ? device.InstallationDate.Value.ToString("yyyy-MM-dd") : "";
                        }

                        info_name_number_text.GetComponent<TMP_InputField>().text =
                            string.IsNullOrEmpty(device.ProductName) == true ?
                            $"{device.ManagementNumber}" :
                            $"{device.ProductName} / {device.ManagementNumber}";

                        info_model_code_text.GetComponent<TMP_InputField>().text = 
                            string.IsNullOrEmpty(device.ModelName) ? "-" : device.ModelName;

                        info_install_date_text.GetComponent<TMP_InputField>().text = device.InstallationDate.HasValue ? device.InstallationDate.Value.ToString("yyyy-MM-dd") : "-";

                        info_power_usage_text.GetComponent<TMP_InputField>().text =
                            device.PowerConsumption == null ? "-" : device.PowerConsumption.ToString();

                        info_management_text.GetComponent<TMP_InputField>().text =
                            string.IsNullOrEmpty(device.ManagementNumber) ? "-" : device.ManagementNumber;

                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("역직렬화 중 오류 발생: " + ex.Message);
                }
            }
        }
    }

    IEnumerator GetRequest1(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Response: " + www.downloadHandler.text);

                // JSON 데이터를 ResponseData 객체로 역직렬화
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-ddTHH:mm:ss"
                };

                ResponseData responseData = JsonConvert.DeserializeObject<ResponseData>(www.downloadHandler.text, settings);

                // 이벤트 리스트 출력
                for (int i = 0; i < 4; i++)
                {
                    if (i < responseData.events_list.Count)
                    {
                        var eventRecord = responseData.events_list[i];
                        eventHistoryTexts[i].text = $"{eventRecord.EventDate} / {eventRecord.EventDetails}";
                    }
                    else
                    {
                        // 남은 UI 요소는 빈 문자열로 처리하거나 비활성화
                        eventHistoryTexts[i].text = "-";
                    }
                }

                // 유지보수 리스트 출력
                for (int i = 0; i < maintenanceHistoryTexts.Length; i++)
                {
                    if (i < responseData.maintenance_list.Count)
                    {
                        var maintenanceRecord = responseData.maintenance_list[i];
                        _MaintenanceID[i] = maintenanceRecord.MaintenanceID;
                        maintenanceHistoryTexts[i].text = string.IsNullOrEmpty(maintenanceRecord.Title) ? "-" : $"{maintenanceRecord.Title}";

                        maintenanceHistoryTexts[i].text += string.IsNullOrEmpty(maintenanceRecord.Applicant) ? "-" : $" / {maintenanceRecord.Applicant}";
                        // 박스 이미지 컬러가 변하지 않음 必
                        as_history_images[i].color = maintenanceRecord.Status == 1 ? Color.green : Color.red;
                    }
                    else
                    {
                        // 남은 UI 요소는 빈 문자열로 처리하거나 비활성화
                        maintenanceHistoryTexts[i].text = "-";
                    }
                }
            }
        }
    }

    IEnumerator GetRequest2(string uri)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(uri))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("test : " + www.downloadHandler.text);

                // JsonSerializerSettings 설정
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "yyyy-MM-dd",
                    NullValueHandling = NullValueHandling.Ignore
                };

                try
                {
                    // JSON 배열을 List<Devices>로 역직렬화
                    List<Provisioning> devices = JsonConvert.DeserializeObject<List<Provisioning>>(www.downloadHandler.text, settings);

                    provisioning_name_text.GetComponent<TMP_InputField>().text = "";
                    provisioning_location_text.GetComponent<TMP_InputField>().text = "";
                    provisioning_key_text.GetComponent<TMP_InputField>().text = "";
                    provisioning_management_text.GetComponent<TMP_InputField>().text = "";
                    provisioning_protocol_text.GetComponent<TMP_InputField>().text = "";
                    provisioning_mac_text.GetComponent<TMP_InputField>().text = "";

                    // 결과 출력
                    foreach (var device in devices)
                    {
                        provisioning_name_text.GetComponent<TMP_InputField>().text =
                            string.IsNullOrEmpty(device.ModelName) ? "-" : $"{device.ModelName}";

                        provisioning_location_text.GetComponent<TMP_InputField>().text =
                            string.IsNullOrEmpty(device.Location) ? "-" : $"{device.Location}";

                        provisioning_key_text.GetComponent<TMP_InputField>().text = $"{device.DeviceID}";

                        provisioning_management_text.GetComponent<TMP_InputField>().text = 
                            string.IsNullOrEmpty(device.ManagementNumber) ? "-" : $"{device.ManagementNumber}";

                        provisioning_protocol_text.GetComponent<TMP_InputField>().text = 
                            string.IsNullOrEmpty(device.NetworkType) ? "-" : $"{device.NetworkType}";

                        provisioning_mac_text.GetComponent<TMP_InputField>().text = 
                            string.IsNullOrEmpty(device.Mac) ? "-" : $"{device.Mac}";
                    }

                    if (currentImageActiveCoroutine != null)
                    {
                        StopCoroutine(currentImageActiveCoroutine);
                        
                        currentImageActiveCoroutine = null;

                        foreach (var i in sphereImages)
                        {
                            i.SetActive(true);
                        }
                    }

                    // 시작은 화재 감지 모듈부터
                    currentImageActiveCoroutine = StartCoroutine(ColorImagesChangeActive());
               }
                catch (Exception ex)
                {
                    Debug.LogError("역직렬화 중 오류 발생: " + ex.Message);
                }
            }
        }
    }

    IEnumerator ColorImagesChangeActive()
    {
        string groupKey = "";
        switch (transform.name[^1])
        {
            case '1':
                groupKey = "red";
                break;
            case '2':
                groupKey = "blue";
                break;
            case '3':
                groupKey = "green";
                break;
            case '4':
                groupKey = "purple";
                break;
            case '5':
                groupKey = "yellow";
                break;
        }

        if (!objectGroups.ContainsKey(groupKey))
        {
            Debug.LogWarning($"'{groupKey}' 그룹이 존재하지 않습니다.");
            yield break;
        }

        List<GameObject> groupObjects = objectGroups[groupKey];

        // 초기 활성화 상태
        bool isActive = true;

        while (true)
        {
            // 활성화 상태 토글
            isActive = !isActive;

            foreach (GameObject obj in groupObjects)
            {
                obj.SetActive(isActive);
            }

            // 1초 대기
            yield return new WaitForSeconds(1f);
        }
    }
}
