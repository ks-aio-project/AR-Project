using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public GameObject buttonGroup, buttonGroup1;
    public Transform image;
    public Transform canvas;

    public Transform ProvisionCanvas;

    void Start()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    public void OnClickButtons(GameObject obj)
    {
        switch(obj.name)
        {
            case "+x":
                image.transform.Translate(5f, 0, 0);
                break;
            case "-x":
                image.transform.Translate(-5f, 0, 0);
                break;
            case "+y":
                image.transform.Translate(0, 5f, 0);
                break;
            case "-y":
                image.transform.Translate(0, -5f, 0);
                break;
            case "+z":
                image.transform.Translate(0, 0, 5f);
                break;
            case "-z":
                image.transform.Translate(0, 0, -5f);
                break;
            case "hide":
                buttonGroup.SetActive(false);
                buttonGroup1.SetActive(false);
                break;
            case "showcanvas":
                canvas.gameObject.SetActive(true);
                canvas.gameObject.transform.position = Camera.main.transform.position + new Vector3(0, 0, 0.2f);
                break;
            case "hidecanvas":
                canvas.gameObject.SetActive(false);
                break;
            case "hideall":
                gameObject.SetActive(false);
                break;
            case "hideimage":
                image.gameObject.SetActive(false);
                break;
            case "showimage":
                image.gameObject.SetActive(true);
                break;
            case "+xp":
                canvas.transform.Translate(1f, 0, 0);
                break;
            case "+yp":
                canvas.transform.Translate(0, 1f, 0);
                break;
            case "+zp":
                canvas.transform.Translate(0, 0, 1f);
                break;
            case "-xp":
                canvas.transform.Translate(-1f, 0, 0);
                break;
            case "-yp":
                canvas.transform.Translate(0, -1f, 0);
                break;
            case "-zp":
                canvas.transform.Translate(0, 0, -1f);
                break;


            case "+xrp":
                canvas.transform.rotation = Quaternion.Euler(1, 0, 0);
                break;
            case "+yrp":
                canvas.transform.rotation = Quaternion.Euler(0, 1, 0);
                break;
            case "+zrp":
                canvas.transform.rotation = Quaternion.Euler(0, 0, 1);
                break;
            case "-xrp":
                canvas.transform.rotation = Quaternion.Euler(-1, 0, 0);
                break;
            case "-yrp":
                canvas.transform.rotation = Quaternion.Euler(0, -1, 0);
                break;
            case "-zrp":
                canvas.transform.rotation = Quaternion.Euler(0, 0, -1);
                break;
        }
    }
}
