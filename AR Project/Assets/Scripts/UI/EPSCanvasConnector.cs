using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class EPSCanvasConnector : MonoBehaviour
{
    public GameObject canvas;

    public List<GameObject> pointsList = new();

    public List<GameObject> buttonObjectList = new();

    private void Awake()
    {
        foreach (var i in buttonObjectList)
        {
            i.GetComponent<Button>().onClick.AddListener(() => OnClickEPSList(i));
        }
    }

    public void OnClickEPSList(GameObject obj)
    {
        string numbersOnly = Regex.Replace(obj.name, @"\D", "");

        foreach (var i in pointsList) 
        {
            i.SetActive(Regex.Replace(i.name, @"\D", "") == numbersOnly);
        }
    }
}
