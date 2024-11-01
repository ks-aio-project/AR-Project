using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class SceneUtil : MonoBehaviour
{
    public List<GameObject> objs;

    private void Start()
    {
        StartCoroutine(InfiniteWaitCoroutine());
    }

    // ���� �ݺ� 1�� ��� �ڷ�ƾ
    IEnumerator InfiniteWaitCoroutine()
    {
        Transform pnt;
        while (true) // ���� ����
        {
            Debug.Log("Action every second");
            foreach (var i in objs)
            {
                Debug.Log($"kks {i.name}/parent/{i.transform.parent}");
                pnt = i.transform.parent;

                while (true)
                {
                    if (pnt != null)
                    {
                        Debug.Log($"kks {pnt.name}/parent/{pnt.transform.parent}");
                        pnt = pnt.parent;
                    }
                    else
                    {
                        break;
                    }
                }
                Debug.Log($"kks {i.name}/position/{i.transform.position}");
                Debug.Log($"kks {i.name}/rotation/{i.transform.rotation.eulerAngles}");
                Debug.Log($"kks {i.name}/localPosition/{i.transform.localPosition}");
                Debug.Log($"kks {i.name}/localRotation/{i.transform.localRotation.eulerAngles}");
            }

            // 1�� ���
            yield return new WaitForSeconds(1.0f);
        }
    }
}
