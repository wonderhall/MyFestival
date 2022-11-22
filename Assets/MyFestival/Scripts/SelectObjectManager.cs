using RuntimeHandle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjectManager : MonoBehaviour
{
    private Vector3 m_vecMouseDownPos;
    //같은 컬리전 히트 체크
    private Collider curCol;
    private int num = 0;
    //더블클릭
    public float m_DoubleClickSecond = 0.25f;
    private bool m_IsOneClick = false;
    private double m_Timer = 0;
    void Update()
    {
#if UNITY_EDITOR
        // 마우스 클릭 시
        if (Input.GetMouseButtonDown(0))
#else
        // 터치 시
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
            // 카메라에서 스크린에 마우스 클릭 위치를 통과하는 광선을 반환합니다.
            Ray ray = Camera.main.ScreenPointToRay(m_vecMouseDownPos);
            RaycastHit hit;

            //더블클릭
            if (m_IsOneClick && ((Time.time - m_Timer) > m_DoubleClickSecond))
            {
                m_IsOneClick = false;
            }



            // 광선으로 충돌된 collider를 hit에 넣습니다.
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Player")
                {

                    curCol = hit.collider;
                    // 어떤 오브젝트인지 로그를 찍습니다.
                    Debug.Log(hit.collider.name);
                    //기즈모 부착
                    if (GameObject.FindObjectOfType<RuntimeTransformHandle>())
                        GameObject.Find("handler").GetComponent<RuntimeTransformHandle>().target = hit.transform;
                    else
                        RuntimeTransformHandle.Create(hit.transform, 0).name = "handler";
                    //기즈모 속성변경
                    RuntimeTransformHandle handler = GameObject.Find("handler").GetComponent<RuntimeTransformHandle>();
                    handler.autoScale = true;
                    handler.autoScaleFactor = 1.5f;


                    //더블클릭 구현
                    if (!m_IsOneClick)
                    {
                        m_Timer = Time.time;
                        m_IsOneClick = true;
                    }
                    else if (m_IsOneClick && ((Time.time - m_Timer) < m_DoubleClickSecond))
                    {
                        m_IsOneClick = false;

                        //ui 온
                        Debug.Log(hit.transform.GetChild(hit.transform.childCount - 1).gameObject.name);
                        hit.transform.GetChild(hit.transform.childCount - 1).gameObject.SetActive(true);

                        //기즈모 변경
                        if (curCol = hit.collider)
                        {
                            if (num > 2) num = 0;
                            else num++;
 
                            switch (num)
                            {
                                case 0:
                                    handler.type = HandleType.POSITION;
                                    break;
                                case 1:
                                    handler.type = HandleType.ROTATION;
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
                        //아래에 더블클릭에서 처리하고싶은 이벤트 작성
                    }




                    #region getNameTest
                    //// 오브젝트 별로 코드를 작성할 수 있습니다.
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
