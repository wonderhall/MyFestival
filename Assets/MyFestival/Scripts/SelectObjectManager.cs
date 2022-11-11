using RuntimeHandle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjectManager : MonoBehaviour
{
    Vector3 m_vecMouseDownPos;

    void Update()
    {
#if UNITY_EDITOR
        // ���콺 Ŭ�� ��
        if (Input.GetMouseButtonDown(0))
#else
        // ��ġ ��
        if (Input.touchCount > 0)
#endif
        {

#if UNITY_EDITOR
            m_vecMouseDownPos = Input.mousePosition;
#else
            m_vecMouseDownPos = Input.GetTouch(0).position;
            if(Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif
            // ī�޶󿡼� ��ũ���� ���콺 Ŭ�� ��ġ�� ����ϴ� ������ ��ȯ�մϴ�.
            Ray ray = Camera.main.ScreenPointToRay(m_vecMouseDownPos);
            RaycastHit hit;

            // �������� �浹�� collider�� hit�� �ֽ��ϴ�.
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {


                    // � ������Ʈ���� �α׸� ����ϴ�.
                    Debug.Log(hit.collider.name);
                    //����� ����
                    if (GameObject.FindObjectOfType<RuntimeTransformHandle>())
                        GameObject.Find("handler").GetComponent<RuntimeTransformHandle>().target = hit.transform;
                    else
                        RuntimeTransformHandle.Create(hit.transform, 0).name = "handler";
                    //����� �Ӽ�����
                    RuntimeTransformHandle handler = GameObject.Find("handler").GetComponent<RuntimeTransformHandle>();
                    handler.autoScale = true;
                    handler.autoScaleFactor = 1.5f;

                    #region getNameTest
                    //// ������Ʈ ���� �ڵ带 �ۼ��� �� �ֽ��ϴ�.
                    //if (hit.collider.name == "Cube")
                    //    Debug.Log("Cube Hit");
                    //else if (hit.collider.name == "Capsule")
                    //    Debug.Log("Capsule Hit");
                    //else if (hit.collider.name == "Sphere")
                    //    Debug.Log("Sphere Hit");
                    //else if (hit.collider.name == "Cylinder")
                    //    Debug.Log("Cylinder Hit");
                    #endregion
                }
            }

        }
    }
}
