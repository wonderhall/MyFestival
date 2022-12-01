using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveController : MonoBehaviour
{

    #region before
    //public bool Moving = false;

    //private PathCreator path;
    //private EndOfPathInstruction end;
    //private float dstTravelled;

    ////private PathFollower path;
    //private ParticleSystem ps;

    //private void Awake()
    //{
    //    path = this.transform.GetChild(0).GetComponent<PathCreator>();
    //    //pathC = this.transform.GetChild(1).GetComponent<PathCreation>();
    //    ps = this.transform.GetChild(0).GetComponent<ParticleSystem>();
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //    if (Moving)
    //    {
    //        if (path != null)
    //            path.enabled = true;
    //        if (ps != null)
    //            ps.gameObject.SetActive(true);


    //    }
    //    else
    //    {
    //        if (path != null)
    //            path.enabled = false;
    //        if (ps != null)
    //            ps.gameObject.SetActive(false);

    //        ResetPositio();
    //    }

    //}

    //private void ResetPositio()
    //{
    //    transform.position = path.path.GetPointAtDistance(0, end);
    //    transform.rotation = path.path.GetRotationAtDistance(0, end);
    //} 
    #endregion

    public bool Moving = false;
    public Transform target;
    public ParticleSystem ps;
    public PathCreator pathCreator;
    public EndOfPathInstruction end;
    public float dstTravelled;
    public float speed = 5;


    private void OnEnable()
    {
        if (this.transform.GetChild(0).GetComponent<ParticleSystem>())//�ڽ� 1�� �˻��ؼ� ��ƼŬ�̸� ��ƼŬ�� �־�
            ps = this.transform.GetChild(0).GetComponent<ParticleSystem>();
        else
            target = this.transform.GetChild(0);//������ ������Ʈ �ڽ�1��

        if (this.transform.GetChild(1).GetComponent<PathCreator>())
            pathCreator = this.transform.GetChild(1).GetComponent<PathCreator>();
    }
    private void Update()
    {
        if (Moving)
        {
            if (target != null)
            {
                dstTravelled += speed * Time.deltaTime;
                target.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
                target.transform.rotation = pathCreator.path.GetRotationAtDistance(dstTravelled, end);
            }
            if (ps != null) ps.gameObject.SetActive(true);
        }
        else
        {
            if (target != null)
            {
                dstTravelled = 0;
                target.transform.position = pathCreator.path.GetPointAtDistance(dstTravelled, end);
                target.transform.rotation = pathCreator.path.GetRotationAtDistance(dstTravelled, end);
            }
            if (ps != null) ps.gameObject.SetActive(false);

        }

    }
}
