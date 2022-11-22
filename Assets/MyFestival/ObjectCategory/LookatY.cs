using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RuntimeHandle;

public class LookatY : MonoBehaviour
{
    Transform lookObj;
    Transform target;

    public bool reverse = false;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "CreateNewTemp"&&Manager.instance.DeleteUI != null) Manager.instance.DeleteUI.gameObject.SetActive(false);
        if(scene.name == "CreateNewTemp") Manager.instance.DeleteUI = this.gameObject;


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
        if (reverse == false)
        {
            target = GameObject.FindGameObjectWithTag("MainCamera").transform;
            lookObj = this.transform;

            transform.LookAt(target);
            lookObj.transform.eulerAngles = new Vector3(0, (transform.eulerAngles.y) + 180f, 0); // lock x and z axis to zero

        }
        else
        {
            target = GameObject.FindGameObjectWithTag("MainCamera").transform;
            lookObj = this.transform;

            transform.LookAt(target);
            lookObj.transform.eulerAngles = new Vector3(0, (transform.eulerAngles.y), 0); // lock x and z axis to zero
        }
    }
}
