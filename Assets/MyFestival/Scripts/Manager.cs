using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;


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

    [Header("현재 선택한 탬플릿 이름")]
    public string TempName;
    [Header("현재 선택된 오브젝트 유아이")]
    public GameObject DeleteUI;

    public bool isShowMyList = false;
    public bool isShowTemplete = false;

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

    public void ArReset()
    {
        ARSession session = GameObject.Find("AR Session").GetComponent<ARSession>();
        session.Reset();
    }
}
