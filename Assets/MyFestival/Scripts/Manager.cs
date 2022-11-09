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

    [Header("날자")]
    public string date;
    [Header("위도")]
    public double latitude;
    [Header("경도")]
    public double longitude;

    [Header("현재 선택한 스크립트오브젝트")]
    public ScriptableObject_CategoryItems scriptableObject;
    [Header("현재 선택한 오브젝트 이름")]
    public int SeletedObjectIndex;

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
