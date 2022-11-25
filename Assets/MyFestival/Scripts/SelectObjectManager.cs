using RuntimeHandle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjectManager : MonoBehaviour
{
    private Vector3 m_vecMouseDownPos;
    //���� �ø��� ��Ʈ üũ
    private Collider curCol;
    private int num = 0;
    //����Ŭ��
    public float m_DoubleClickSecond = 0.25f;
    private bool m_IsOneClick = false;
    private double m_Timer = 0;
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

            //����Ŭ��
            if (m_IsOneClick && ((Time.time - m_Timer) > m_DoubleClickSecond))
            {
                m_IsOneClick = false;
            }



            // �������� �浹�� collider�� hit�� �ֽ��ϴ�.
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "2D" || hit.collider.tag == "Text")
                {

                    curCol = hit.collider;
                    // � ������Ʈ���� �α׸� ����ϴ�.

                    //ui ��
                    hit.transform.GetChild(hit.transform.childCount - 1).gameObject.SetActive(true);

                    //����� ����
                    if (GameObject.FindObjectOfType<RuntimeTransformHandle>())
                        GameObject.Find("handler").GetComponent<RuntimeTransformHandle>().target = hit.transform;
                    else
                        RuntimeTransformHandle.Create(hit.transform, 0).name = "handler";
                    //����� �Ӽ�����
                    RuntimeTransformHandle handler = GameObject.Find("handler").GetComponent<RuntimeTransformHandle>();
                    handler.autoScale = true;
                    handler.autoScaleFactor = 1.5f;

                    //��Ʈ�� ������Ʈ Ÿ�Կ� ���� �ٸ� aix����
                    if (hit.collider.tag == "2D"|| hit.collider.tag == "Text")
                    {
                        handler.axes = HandleAxes.XY;
                        handler.space = HandleSpace.WORLD;
                    }
                    else
                    {
                        handler.axes = HandleAxes.XYZ;
                        handler.space = HandleSpace.LOCAL;
                    }
                    Debug.Log("��Ʈ�� �±״� = "+hit.collider.tag);


                    //����Ŭ�� ����
                    if (!m_IsOneClick)
                    {
                        m_Timer = Time.time;
                        m_IsOneClick = true;
                    }
                    else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
                    {
                        m_IsOneClick = false;



                        //����� ����
                        if (curCol = hit.collider)
                        {
                            if(num > 1&& hit.collider.tag == "Text")
                            {
                                RuntimeHandle.RuntimeTransformHandle.Destroy(GameObject.Find("handler"));
                                hit.collider.GetComponent<Banner>().WaitTyping();
                            }
                            if (num > 2) num = 0;
                            else num++;
                            if (num == 1 && hit.collider.tag == "2D") num = 2;
                            if (num == 1 && hit.collider.tag == "Text") num = 2;

                            switch (num)
                            {
                                case 0:
                                    handler.type = HandleType.POSITION;
                                    break;
                                case 1:
                                    handler.type = HandleType.ROTATION;
                                    handler.space = HandleSpace.LOCAL;
                                    break;
                                case 2:
                                    handler.type = HandleType.SCALE;
                                    break;
                                default:
                                    handler.type = HandleType.POSITION;
                                    break;
                            }

                        }
                        Debug.Log("same");
                        //�Ʒ��� ����Ŭ������ ó���ϰ���� �̺�Ʈ �ۼ�
                    }




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
