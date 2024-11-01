using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIConnetor : MonoBehaviour
{
    public GameObject button_ok, button_cancel, button_rotation;
    public Dictionary<GameObject, bool> pairs = new Dictionary<GameObject, bool>();

    public List<GameObject> objs = new List<GameObject>();

    public bool isAble = true;

    private void Update()
    {
        if (button_ok != null)
        {
            button_ok.transform.rotation = Quaternion.LookRotation(button_ok.transform.position - Camera.main.transform.position);
        }

        if (button_cancel != null)
        {
            button_cancel.transform.rotation = Quaternion.LookRotation(button_cancel.transform.position - Camera.main.transform.position);
        }

        if (button_rotation != null)
        {
            button_rotation.transform.rotation = Quaternion.LookRotation(button_rotation.transform.position - Camera.main.transform.position);
        }
    }
}
