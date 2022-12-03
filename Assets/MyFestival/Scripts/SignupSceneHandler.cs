using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Proyecto26;
using System.Text;
using EasyUI.Toast;
using UnityEngine.SceneManagement;

public class SignupSceneHandler : MonoBehaviour
{
    private readonly string basePath = "https://flyingart-server.8hlab.com";

    public TMP_InputField username_inputField;
    public TMP_InputField password_InputField;
    public TMP_InputField passwordConfirm_InputField;
    public Button signup_Button;
    public Button login_Button;

    // Start is called before the first frame update
    void Start()
    {
        signup_Button.onClick.AddListener(OnClickSignup);
        login_Button.onClick.AddListener(OnClickLogin);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnClickSignup()
    {
        string username = username_inputField.text;
        string email = username_inputField.text;
        string password = password_InputField.text;
        string passwordConfirm = passwordConfirm_InputField.text;
        string[] userRoles = new string[] { "ROLE_CLIENT" };
        
        if (!password.Equals(passwordConfirm))
        {
            Toast.Show("비밀번호 확인이 틀립니다.");
            return;
        }

        RestClient.Post(basePath + "/users/signup", new ReqSignup { username = username, email = email, hashedPassword = password, userRoles = userRoles })
        .Then(response => {
            var token = response.Text;
            PlayerPrefs.SetString("token", token);
            Toast.Show("환영합니다.");
            SceneManager.LoadScene("Main");
        })
        .Catch(err => {
            var error = err as RequestException;
            var exception = JsonUtility.FromJson<ServerException>(error.Response);
            Toast.Show(exception.error);
        });
    }
    public void OnClickLogin()
    {
        SceneManager.LoadScene("LoginScene");
    }
    private string decodeToken(string token)
    {
        byte[] SecretKeyByte = Encoding.UTF8.GetBytes("daxib-secret-key");
        return Jose.JWT.Decode(token, SecretKeyByte);
    }
}
