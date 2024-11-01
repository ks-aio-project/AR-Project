using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Test1 : MonoBehaviour
{
    public bool trigger = false;

    UIConnetor uiConnetor;

    private void Start()
    {
        uiConnetor = GetComponentInParent<UIConnetor>();
        uiConnetor.pairs.Add(gameObject, trigger);
        uiConnetor.objs.Add(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.name.Equals("Ceiling"))
        {
            Debug.Log($"KKS ceiling stay : {transform.name}");
            trigger = true;

            uiConnetor.pairs[gameObject] = trigger;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.name.Equals("Ceiling"))
        {
            Debug.Log($"KKS ceiling exit : {transform.name}");
            trigger = false;

            uiConnetor.pairs[gameObject] = trigger;
        }
    }
}
