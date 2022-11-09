using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Duck.Http;

public class LogiHandler : MonoBehaviour
{
    public TMP_InputField email_inputField;
    public TMP_InputField password_InputField;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChangedEmailField(string text)
    {
        Debug.Log(text + " - 글자 입력 중");
        email_inputField.text = text;
    }

    public void OnClickLogin()
    {
        var request = Http.Get("http://mywebapi.com/")
            .SetHeader("Authorization", "username:password")
            .OnSuccess(response => Debug.Log(response.Text))
            .OnError(response => Debug.Log(response.StatusCode))
            .OnDownloadProgress(progress => Debug.Log(progress))
            .Send();
    }

}
