using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

/// <summary>
/// ������
/// </summary>
public class UpdatePosition : MonoBehaviour
{
    public List<GameObject> objs;

    private void Awake()
    {
        // ī�޶� ���� ��û
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }

    void Update()
    {
        //GetComponent<TextMeshProUGUI>().text = "";

        //for (int i = 0; i < objs.Count; i++)
        //{
        //    GetComponent<TextMeshProUGUI>().text += $"name : {objs[i].name} / pos : {objs[i].transform.position} / loc : {objs[i].transform.localPosition}\n";

        //    if (objs[4].transform.parent != null)
        //    {
        //        objs[4].transform.position = Vector3.zero;
        //    }
        //}
    }
}
