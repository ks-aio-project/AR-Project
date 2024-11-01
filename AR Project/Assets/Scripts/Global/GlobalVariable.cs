using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GlobalVariable
{
    private static GlobalVariable instance;

    public string alert_id = "";
    public string currentAlert = "";

    public bool isSeeAble = false;
    // 0807
    public Vector3 distribution_box_offset = new Vector3(-0.5f, -0.2f, 1.25f);

    //public Vector3 distribution_box_offset = new Vector3(0.025f, -0.2f, 1f);
    public Vector3 imageCanvas_offset = new Vector3(0f, 0f, 3f);

    // 0807
    public Vector3 room_offset_x = new Vector3(2f, -1.5f, 1f);
    public Vector3 room_offset_nx = new Vector3(2f, -1.5f, -1f);
    public Vector3 room_offset_z = new Vector3(1f, -1.5f, -2f);
    public Vector3 room_offset_nz = new Vector3(-1f, -1.5f, 2f);
    public Vector3 room_offset = new Vector3(2f, -1.5f, 2f);

    public string last_qrcode = "";

    public static GlobalVariable Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new GlobalVariable();
            }
            return instance;
        }
    }

    public void ShowToast(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject curActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        curActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaObject toast = new AndroidJavaObject("android.widget.Toast", curActivity);
            toast.CallStatic<AndroidJavaObject>("makeText", curActivity, message, 0).Call("show");
        }));
    }
}