using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var token = PlayerPrefs.GetString("token");
        if (token.Length > 0)
        {
            SceneManager.LoadScene("Main");
        } else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
