using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;


public class Manager : MonoBehaviour
{
    public static Manager instance = null;

    [Header("�������̵�")]
    public string userID;

    [Header("����")]
    public string date;
    [Header("����")]
    public double latitude;
    [Header("�浵")]
    public double longitude;

    [Header("���� ������ ���ø� �̸�")]
    public string TempName;
    [Header("���� ���õ� ������Ʈ ������")]
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
