using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;
using ZXing.Common;
using Toggle = UnityEngine.UI.Toggle;

public class QRCodeScan : MonoBehaviour
{
    public XROrigin xrOrigin;
    public ARCameraManager cameraManager;
    public ARPlaneManager arPlaneManager;
    public ARAnchorManager arAnchorManager;
    public ARRaycastManager arRaycastManager;
    public GameObject provisioningCanvas;

    string lastQRCodeContent;

    public GameObject rectPrefab;
    public GameObject GPM_webView;
    GameObject createPrefab;

    public Toggle PlaneShowToggle;
    private void Start()
    {
        PlaneShowToggle.onValueChanged.AddListener(OnPlaneToggleValueChanged);
    }

    void Update()
    {
        if (!provisioningCanvas.activeSelf)
        {
            if (cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            {
                using (image)
                {
                    var conversionParams = new XRCpuImage.ConversionParams(image, TextureFormat.R8, XRCpuImage.Transformation.MirrorY);
                    if (conversionParams != null)
                    {
                        var dataSize = image.GetConvertedDataSize(conversionParams);
                        var grayscalePixels = new byte[dataSize];

                        unsafe
                        {
                            fixed (void* ptr = grayscalePixels)
                            {
                                image.Convert(conversionParams, new System.IntPtr(ptr), dataSize);
                            }
                        }

                        IBarcodeReader barcodeReader = new BarcodeReader();
                        var result = barcodeReader.Decode(grayscalePixels, image.width, image.height, RGBLuminanceSource.BitmapFormat.Gray8);

                        if (result != null && lastQRCodeContent != result.Text)
                        {
                            Debug.Log($"KKS QR 텍스트 : {result.Text}");
                            lastQRCodeContent = result.Text;

                            GlobalVariable.Instance.last_qrcode = lastQRCodeContent;
                        }
                    }
                }
            }
        }
    }

    // Toggle 값이 변경될 때 실행되는 함수
    void OnPlaneToggleValueChanged(bool isOn)
    {
        if (isOn)
        {
            ShowPlaneVisualizers();
        }
        else
        {
            HidePlaneVisualizers();
        }
    }

    void HidePlaneVisualizers()
    {
        arPlaneManager.planePrefab.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
        arPlaneManager.planePrefab.GetComponent<MeshRenderer>().enabled = false;

        // ARPlaneManager가 관리하는 모든 평면들에 접근
        foreach (var plane in arPlaneManager.trackables)
        {
            var visualizer = plane.GetComponent<ARPlaneMeshVisualizer>();
            if (visualizer != null)
            {
                visualizer.enabled = false; // 시각화 비활성화
            }

            var meshRenderer = plane.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false; // MeshRenderer도 끄기
            }
        }
    }

    void ShowPlaneVisualizers()
    {
        arPlaneManager.planePrefab.GetComponent<ARPlaneMeshVisualizer>().enabled = true;
        arPlaneManager.planePrefab.GetComponent<MeshRenderer>().enabled = true;

        // ARPlaneManager가 관리하는 모든 평면들에 접근
        foreach (var plane in arPlaneManager.trackables)
        {
            var visualizer = plane.GetComponent<ARPlaneMeshVisualizer>();
            if (visualizer != null)
            {
                visualizer.enabled = true; // 시각화 비활성화
            }

            var meshRenderer = plane.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = true; // MeshRenderer도 끄기
            }
        }
    }

    public void ProvisioningCanvasSetInVisible()
    {
        provisioningCanvas.SetActive(false);
        lastQRCodeContent = string.Empty;
        createPrefab.SetActive(false);
    }
}