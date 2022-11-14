using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Proyecto26;
using System.Collections.Generic;
using System.Text;
using EasyUI.Toast;

public class LogiSceneHandler : MonoBehaviour
{
    private readonly string basePath = "https://flyingart-server.8hlab.com";
    private RequestHelper currentRequest;

    public TMP_InputField email_inputField;
    public TMP_InputField password_InputField;
    public Button loginButton;

    // Start is called before the first frame update
    void Start()
    {
        email_inputField.text = "chanoo";
        password_InputField.text = "chanoo";
        loginButton.onClick.AddListener(OnClickLogin);

        var token = PlayerPrefs.GetString("token");
        Debug.Log(token);
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
        Debug.Log("You have clicked the button!");
         
        string username = email_inputField.text;
        string password = password_InputField.text;

        currentRequest = new RequestHelper
        {
            Uri = basePath + "/users/signin",
            Params = new Dictionary<string, string> {
                { "username", username },
                { "password", password }
            },
            EnableDebug = true
        };
        RestClient.Post(currentRequest)
        .Then(response => {

            var token = response.Text;
            PlayerPrefs.SetString("token", token);
            var json = decodeToken(token);
            Toast.Show(json);
        })
        .Catch(err => Toast.Show("아이디 또는 비밀번호가 틀렸습니다."));
    }

    private string decodeToken(string token)
    {
        byte[] SecretKeyByte = Encoding.UTF8.GetBytes("daxib-secret-key");
        return Jose.JWT.Decode(token, SecretKeyByte);
    }

}
