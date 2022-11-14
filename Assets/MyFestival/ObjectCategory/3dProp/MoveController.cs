using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public bool Moving = false;

    private PathFollower path;
    private ParticleSystem ps;
    private void Awake()
    {
        path = this.transform.GetChild(0).GetComponent<PathFollower>();
        ps = this.transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Moving)
        {
            if (path != null)
                path.enabled = true;
            if (ps != null)
                ps.gameObject.SetActive(true);


        }
        else
        {
            if (path != null)
                path.enabled = false;
            if (ps != null)
                ps.gameObject.SetActive(false);


        }

    }
}
