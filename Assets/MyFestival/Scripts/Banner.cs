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
    public float blinkTime = 2f;
    private float t;

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

       
        t += Time.deltaTime;
        if (t > blinkTime)
        {
            StartCoroutine(blinkText(text.gameObject));
            t = 0;
        }
    }

    IEnumerator blinkText(GameObject obj)
    {
        obj.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        obj.SetActive(true);
    }
    public void WaitTyping()
    {
        text.gameObject.SetActive(false);
        mainInputField.gameObject.SetActive(true);
        mainInputField.ActivateInputField();
        mainInputField.onEndEdit.AddListener(delegate { ValueChanged(mainInputField); });
    }
    void ValueChanged(TMP_InputField thisInputField) //이때는 원하는 매개변수를 사용할 수 있다
    {
        if (thisInputField.text.Length > 0)
            text.text = thisInputField.text;
        text.gameObject.SetActive(true);
        thisInputField.gameObject.SetActive(false);
        Debug.Log(thisInputField.name);
        mainInputField.onEndEdit.RemoveListener(delegate { ValueChanged(mainInputField); });
    }


}
