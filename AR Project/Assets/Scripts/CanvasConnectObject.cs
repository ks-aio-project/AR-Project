using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TextShow;

public class CanvasConnectObject : MonoBehaviour
{
    public GameObject canvas;

    public GameObject info_name_number_text, info_model_code_text, info_install_date_text, info_power_usage_text, info_management_text;

    public GameObject event_history_text_1, event_history_text_2, event_history_text_3, event_history_text_4;
    public GameObject as_history_text_1, as_history_text_2, as_history_text_3, as_history_text_4;

    public GameObject as_name_number_text, as_manufacturer_install_date_text, as_request_title, as_request_date, as_applicant, as_request_body;

    public GameObject provisioning_name_text, provisioning_location_text, provisioning_key_text, provisioning_management_text, provisioning_protocol_text, provisioning_mac_text;

    public GameObject button_provisioning_next, button_provisioning_previous, button_provisioning_save;

    public GameObject infoPanel, historyPanel, asPanel, provisioningPanel;

    public GameObject button_info, button_history, button_as, button_provisioning, button_close, button_request_as;

    public GameObject as_webViewObject, provisioning_webViewObject;

    public GameObject floorBaseImage;

    public List<GameObject> sphereImages;

    private Dictionary<string, List<GameObject>> objectGroups = new Dictionary<string, List<GameObject>>();

    public Image[] as_history_images = new Image[4];

    private void Awake()
    {
        TextShow textshow = GetComponentInParent<TextShow>();

        button_info.GetComponent<Button>().onClick.AddListener(() => textshow.ChangeTextIndex(0));
        button_history.GetComponent<Button>().onClick.AddListener(() => textshow.ChangeTextIndex(1));
        button_as.GetComponent<Button>().onClick.AddListener(() => textshow.ChangeTextIndex(2));
        button_provisioning.GetComponent<Button>().onClick.AddListener(() => textshow.ChangeTextIndex(3));
        button_close.GetComponent<Button>().onClick.AddListener(() => textshow.OnClickClose());
        button_request_as.GetComponent<Button>().onClick.AddListener(() => textshow.RequestASSend());
        //button_provisioning_next.GetComponent<Button>().onClick.AddListener(() => textshow.ProvisioningIndexChange(1));
        //button_provisioning_previous.GetComponent<Button>().onClick.AddListener(() => textshow.ProvisioningIndexChange(-1));
        button_provisioning_save.GetComponent<Button>().onClick.AddListener(() => textshow.RequestProvisioningUpdate());

        as_history_images[0].GetComponent<Button>().onClick.AddListener(() => textshow.ChangeASState(as_history_images[0].gameObject));
        as_history_images[1].GetComponent<Button>().onClick.AddListener(() => textshow.ChangeASState(as_history_images[1].gameObject));
        as_history_images[2].GetComponent<Button>().onClick.AddListener(() => textshow.ChangeASState(as_history_images[2].gameObject));
        as_history_images[3].GetComponent<Button>().onClick.AddListener(() => textshow.ChangeASState(as_history_images[3].gameObject));

        // A스크립트의 변수들을 B스크립트 변수에 할당
        textshow.canvas = canvas;

        textshow.info_name_number_text = info_name_number_text;
        textshow.info_model_code_text = info_model_code_text;
        textshow.info_install_date_text = info_install_date_text;
        textshow.info_power_usage_text = info_power_usage_text;
        textshow.info_management_text = info_management_text;

        textshow.event_history_text_1 = event_history_text_1;
        textshow.event_history_text_2 = event_history_text_2;
        textshow.event_history_text_3 = event_history_text_3;
        textshow.event_history_text_4 = event_history_text_4;

        textshow.as_history_text_1 = as_history_text_1;
        textshow.as_history_text_2 = as_history_text_2;
        textshow.as_history_text_3 = as_history_text_3;
        textshow.as_history_text_4 = as_history_text_4;

        textshow.as_history_images[0] = as_history_images[0];
        textshow.as_history_images[1] = as_history_images[1];
        textshow.as_history_images[2] = as_history_images[2];
        textshow.as_history_images[3] = as_history_images[3];

        textshow.as_name_number_text = as_name_number_text;
        textshow.as_manufacturer_install_date_text = as_manufacturer_install_date_text;
        textshow.as_request_title = as_request_title;
        textshow.as_request_date = as_request_date;
        textshow.as_applicant = as_applicant;
        textshow.as_request_body = as_request_body;

        textshow.infoPanel = infoPanel;
        textshow.historyPanel = historyPanel;
        textshow.asPanel = asPanel;
        textshow.provisioningPanel = provisioningPanel;

        textshow.button_info = button_info;
        textshow.button_history = button_history;
        textshow.button_as = button_as;
        textshow.button_provisioning = button_provisioning;
        textshow.button_close = button_close;
        textshow.button_request_as = button_request_as;

        textshow.eventHistoryTexts[0] = event_history_text_1.GetComponent<TMP_InputField>();
        textshow.eventHistoryTexts[1] = event_history_text_2.GetComponent<TMP_InputField>();
        textshow.eventHistoryTexts[2] = event_history_text_3.GetComponent<TMP_InputField>();
        textshow.eventHistoryTexts[3] = event_history_text_4.GetComponent<TMP_InputField>();

        textshow.maintenanceHistoryTexts[0] = as_history_text_1.GetComponent<TMP_InputField>();
        textshow.maintenanceHistoryTexts[1] = as_history_text_2.GetComponent<TMP_InputField>();
        textshow.maintenanceHistoryTexts[2] = as_history_text_3.GetComponent<TMP_InputField>();
        textshow.maintenanceHistoryTexts[3] = as_history_text_4.GetComponent<TMP_InputField>();

        textshow.sphereImages = sphereImages;
        textshow.provisioning_location_text = provisioning_location_text;
        textshow.provisioning_key_text = provisioning_key_text;
        textshow.provisioning_management_text = provisioning_management_text;
        textshow.provisioning_protocol_text = provisioning_protocol_text;
        textshow.provisioning_mac_text = provisioning_mac_text;
        textshow.provisioning_key_text = provisioning_key_text;
        textshow.provisioning_name_text = provisioning_name_text;
        //textshow.button_provisioning_next = button_provisioning_next;
        //textshow.button_provisioning_previous = button_provisioning_previous;
        textshow.button_provisioning_save = button_provisioning_save;

        textshow.as_webViewObject = as_webViewObject;
        textshow.provisioning_webViewObject = provisioning_webViewObject;

        foreach (GameObject obj in sphereImages)
        {
            // 오브젝트의 이름에서 색상 부분 추출
            string key = GetKeyFromName(obj.name);

            if (!string.IsNullOrEmpty(key))
            {
                // 딕셔너리에 키가 없으면 추가
                if (!objectGroups.ContainsKey(key))
                {
                    objectGroups[key] = new List<GameObject>();
                }

                // 해당 키의 리스트에 오브젝트 추가
                objectGroups[key].Add(obj);
            }
        }

        textshow.objectGroups = objectGroups;

        infoPanel.SetActive(false);
        asPanel.SetActive(false);
        historyPanel.SetActive(false);
        provisioningPanel.SetActive(false);

        // 디바이스 타입이 센서나 모듈이면 True
        canvas.SetActive(false);
    }

    // 이름에서 키 추출 (예: "red1" -> "red")
    private string GetKeyFromName(string name)
    {
        if (name.StartsWith("red"))
        {
            return "red";
        }
        else if (name.StartsWith("blue"))
        {
            return "blue";
        }
        else if (name.StartsWith("green"))
        {
            return "green";
        }
        else if (name.StartsWith("purple"))
        {
            return "purple";
        }
        else if (name.StartsWith("yellow"))
        {
            return "yellow";
        }
        else
        {
            return null;
        }
    }

    private void Update()
    {

        // Get direction to the camera
        Vector3 direction = Camera.main.transform.position - transform.position;

        // Zero out the Y component to keep the canvas upright
        direction.y = 0;

        // Calculate the rotation
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Apply the rotation
        transform.rotation = rotation;
    }
}
