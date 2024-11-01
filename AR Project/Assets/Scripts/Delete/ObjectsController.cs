using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{
    public Vector3 pos;
    public Quaternion rot;

    private void Update()
    {
        transform.position = pos;
        transform.rotation = rot;
    }
}
