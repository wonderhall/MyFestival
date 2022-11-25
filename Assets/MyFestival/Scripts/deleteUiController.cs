using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RuntimeHandle;

public class deleteUiController : MonoBehaviour
{
    Transform lookObj;
    Transform target;

    bool isMoving;

    public bool reverse = false;

    private Camera handleCamera;
    public float autoScaleFactor=1;

    // Start is called before the first frame update

    private void OnEnable()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (handleCamera == null)
            handleCamera = Camera.main;

        if (scene.name == "CreateNewTemp" && Manager.instance.DeleteUI != null) Manager.instance.DeleteUI.gameObject.SetActive(false);
        if (scene.name == "CreateNewTemp") Manager.instance.DeleteUI = this.gameObject;

        if (this.transform.parent.GetComponent<MoveController>())
            isMoving = this.transform.parent.GetComponent<MoveController>().Moving;
    }
    public void deleteThis()
    {
        if (this.transform.parent != null)
        {
            Destroy(this.transform.parent.gameObject);
            RuntimeHandle.RuntimeTransformHandle.Destroy(GameObject.Find("handler"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //자동 스케일

        transform.localScale =
    Vector3.one * (Vector3.Distance(handleCamera.transform.position, transform.position) * autoScaleFactor) / 15;
//
        if (reverse == false)
        {
            if (this.transform.parent.tag != "2D")
            {
                target = GameObject.FindGameObjectWithTag("MainCamera").transform;
                lookObj = this.transform;

                transform.LookAt(target);
                lookObj.transform.eulerAngles = new Vector3(0, (transform.eulerAngles.y) + 180f, 0); // lock x and z axis to zero
            }


        }
        else
        {
            if (this.transform.parent.tag != "2D")
            {
                target = GameObject.FindGameObjectWithTag("MainCamera").transform;
                lookObj = this.transform;

                transform.LookAt(target);
                lookObj.transform.eulerAngles = new Vector3(0, (transform.eulerAngles.y), 0); // lock x and z axis to zero
            }

        }

        if (isMoving)
        {
            this.gameObject.SetActive(false);
        }
    }
}
