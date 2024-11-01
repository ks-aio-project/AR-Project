using Gpm.WebView;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Application;

public class WebviewConnect : MonoBehaviour
{
    void Start()
    {
        //ShowUrlPopupPositionSize("https://ks-aio-project.github.io/", 0, 0, 0.5f, 0.5f);
        ShowUrlPopupPositionSize("http://work.allione.kr:3000/public-dashboards/950e1e154b824c2a84a781f6cb336391?orgId=1", 0, 0, 0.5f, 0.5f);
    }

    public void ShowUrlFullScreen()
    {
        GpmWebView.ShowUrl(
            "https://google.com/",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.FULLSCREEN,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                backgroundColor = "#FFFFFF",
                isNavigationBarVisible = true,
                navigationBarColor = "#4B96E6",
                title = "The page title.",
                isBackButtonVisible = true,
                isForwardButtonVisible = true,
                isCloseButtonVisible = true,
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE
#endif
            },
            // See the end of the code example
            OnCallback,
            new List<string>()
            {
            "USER_ CUSTOM_SCHEME"
            });
    }

    // Popup default
    public void ShowUrlPopupDefault()
    {
        Debug.Log("ShowUrlPopupDefault");
        GpmWebView.ShowUrl(
            "https://google.com/",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                isCloseButtonVisible = true,
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
            },
            // See the end of the code example
            OnCallback,
            new List<string>()
            {
            "USER_ CUSTOM_SCHEME"
            });
    }
    public void ShowUrlPopupDefault(string url)
    {
        Debug.Log("ShowUrlPopupDefault");
        GpmWebView.ShowUrl(
            url,
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                isCloseButtonVisible = true,
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
            },
            // See the end of the code example
            OnCallback,
            new List<string>()
            {
            "USER_ CUSTOM_SCHEME"
            });
    }

    // Popup custom position and size
    public void ShowUrlPopupPositionSize()
    {
        GpmWebView.ShowUrl(
            "https://google.com/",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                isCloseButtonVisible = true,
                position = new GpmWebViewRequest.Position
                {
                    hasValue = true,
                    x = (int)(Screen.width * 0.8f),
                    y = (int)(Screen.height * 0.8f)
                },
                size = new GpmWebViewRequest.Size
                {
                    hasValue = true,
                    width = (int)(Screen.width * 0.3f),
                    height = (int)(Screen.height * 0.5f)
                    //width = (int)(Screen.width * 0.8f),
                    //height = (int)(Screen.height * 0.8f)
                },
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
            }, null, null);
    }

    public void ShowUrlPopupPositionSize(string url)
    {
        GpmWebView.ShowUrl(
            url,
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                isCloseButtonVisible = true,
                position = new GpmWebViewRequest.Position
                {
                    hasValue = true,
                    x = (int)(Screen.width * 0.5f),
                    y = (int)(Screen.height * 0.5f)
                },
                size = new GpmWebViewRequest.Size
                {
                    hasValue = true,
                    width = (int)(Screen.width * 0.5f),
                    height = (int)(Screen.height * 0.5f)
                    //width = (int)(Screen.width * 0.8f),
                    //height = (int)(Screen.height * 0.8f)
                },
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
            }, null, null);
    }

    /// <summary>
    /// url, x , y , width , height (0~1)
    /// </summary>
    /// <param name="url"></param>
    /// <param name="_x">position x(0~1)</param>
    /// <param name="_y">position y(0~1)</param>
    /// <param name="_width">width size(0~1)</param>
    /// <param name="_height">height size(0~1)</param>
    public void ShowUrlPopupPositionSize(string url, float _x, float _y, float _width, float _height)
    {
        GpmWebView.ShowUrl(
            url,
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                isCloseButtonVisible = true,
                backgroundColor = "#FFFFFF00",
                position = new GpmWebViewRequest.Position
                {
                    hasValue = true,
                    x = (int)(Screen.width * _x),
                    y = (int)(Screen.height * _y)
                },
                size = new GpmWebViewRequest.Size
                {
                    hasValue = true,
                    width = (int)(Screen.width * _width),
                    height = (int)(Screen.height * _height)
                    //width = (int)(Screen.width * 0.8f),
                    //height = (int)(Screen.height * 0.8f)
                },
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
            }, null, null);
    }

    // Popup custom margins
    public void ShowUrlPopupMargins()
    {
        GpmWebView.ShowUrl(
            "https://google.com/",
            new GpmWebViewRequest.Configuration()
            {
                style = GpmWebViewStyle.POPUP,
                orientation = GpmOrientation.UNSPECIFIED,
                isClearCookie = true,
                isClearCache = true,
                isNavigationBarVisible = true,
                isCloseButtonVisible = true,
                margins = new GpmWebViewRequest.Margins
                {
                    hasValue = true,
                    left = (int)(Screen.width * 0.1f),
                    top = (int)(Screen.height * 0.1f),
                    right = (int)(Screen.width * 0.1f),
                    bottom = (int)(Screen.height * 0.1f)
                },
                supportMultipleWindows = true,
#if UNITY_IOS
            contentMode = GpmWebViewContentMode.MOBILE,
            isMaskViewVisible = true,
#endif
            }, null, null);
    }

    private void OnCallback(
        GpmWebViewCallback.CallbackType callbackType,
        string data,
        GpmWebViewError error)
    {
        Debug.Log("OnCallback: " + callbackType);
        switch (callbackType)
        {
            case GpmWebViewCallback.CallbackType.Open:
                if (error != null)
                {
                    Debug.LogFormat("Fail to open WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.Close:
                if (error != null)
                {
                    Debug.LogFormat("Fail to close WebView. Error:{0}", error);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageStarted:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("PageStarted Url : {0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.PageLoad:
                if (string.IsNullOrEmpty(data) == false)
                {
                    Debug.LogFormat("Loaded Page:{0}", data);
                }
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowOpen:
                Debug.Log("MultiWindowOpen");
                break;
            case GpmWebViewCallback.CallbackType.MultiWindowClose:
                Debug.Log("MultiWindowClose");
                break;
            case GpmWebViewCallback.CallbackType.Scheme:
                if (error == null)
                {
                    if (data.Equals("USER_ CUSTOM_SCHEME") == true || data.Contains("CUSTOM_SCHEME") == true)
                    {
                        Debug.Log(string.Format("scheme:{0}", data));
                    }
                }
                else
                {
                    Debug.Log(string.Format("Fail to custom scheme. Error:{0}", error));
                }
                break;
            case GpmWebViewCallback.CallbackType.GoBack:
                Debug.Log("GoBack");
                break;
            case GpmWebViewCallback.CallbackType.GoForward:
                Debug.Log("GoForward");
                break;
            case GpmWebViewCallback.CallbackType.ExecuteJavascript:
                Debug.LogFormat("ExecuteJavascript data : {0}, error : {1}", data, error);
                break;
#if UNITY_ANDROID
            case GpmWebViewCallback.CallbackType.BackButtonClose:
                Debug.Log("BackButtonClose");
                break;
#endif
        }
    }
}
