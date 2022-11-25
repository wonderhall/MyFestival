using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Banner : MonoBehaviour
{
    Transform cam;
    //public InputField mainInputField;
    public TMP_InputField mainInputField;
    public TMP_Text text;

    void Start()
    {
        text.gameObject.SetActive(true);
        mainInputField.gameObject.SetActive(false);
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(cam);
        transform.forward = -transform.forward;
    }
    public void WaitTyping()
    {
        text.gameObject.SetActive(false);
        mainInputField.gameObject.SetActive(true);
        mainInputField.ActivateInputField();
        mainInputField.onEndEdit.AddListener(delegate { ValueChanged(mainInputField); });
    }
    void ValueChanged(TMP_InputField thisInputField) //�̶��� ���ϴ� �Ű������� ����� �� �ִ�
    {
        if (thisInputField.text.Length > 0)
            text.text = thisInputField.text;
        text.gameObject.SetActive(true);
        thisInputField.gameObject.SetActive(false);
        Debug.Log(thisInputField.name);
        mainInputField.onEndEdit.RemoveListener(delegate { ValueChanged(mainInputField); });
    }


}
