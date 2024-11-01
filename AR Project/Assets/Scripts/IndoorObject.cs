using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorObject : MonoBehaviour
{
    [Header("배치 가능한 오브젝트")]
    public GameObject placeableObject;
    public GameObject wallObject;

    public List<GameObject> exitObject;
}
