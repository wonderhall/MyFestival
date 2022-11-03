using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager instance = null;

    [Header("유저아이디")]
    public string userID;


    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if(instance!=this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SceneLoad(string ScName)
    {
        SceneManager.LoadSceneAsync(ScName, LoadSceneMode.Single);
    }
   public void ResetScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneLoad(scene.name);
    }

 
}
