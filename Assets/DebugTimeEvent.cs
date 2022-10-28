using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTimeEvent : MonoBehaviour
{
    //public currentTime cT;
    private SpecifyTime sT;

    //�ý��� ��¥ �޾ƿ��鼭 0��0��0�ʷ� �ʱ�ȭ
    private int nextEventTime;
    private int endEvevtTime, min = int.MaxValue;

    //�̺�Ʈ ���� �ð� ����Ʈ
    public List<TimeEventAttribute> eventList = new List<TimeEventAttribute>();
    //�����̺�Ʈ
    public TimeEventAttribute temp_nextEvent;
    public List<ParticleSystem> par;
    public bool IsKillParticle = false;



    public float interval = 0.25f;

    //������ ����
    private float nextTime = 0;

    public int totalCurrentSeconds;

    private void Awake()
    {
        sT = GameObject.FindObjectOfType<SpecifyTime>();

        eventList.AddRange(this.GetComponentsInChildren<TimeEventAttribute>());
  
    }
    private void Start()
    {
        //�ϴܿ�����Ʈ �����
        for (int i = 0; i < eventList.Count; i++)
        {
            eventList[i].eventPrefab.SetActive(false);
        }
        //�Ⱓ���ذ�������
        //currentTime();
        totalCurrentSeconds = (int)sT.totalGameSeconds;
        approximationTime(eventList.ToArray());

    }
    private void Update()
    {
        //if (Time.time >= nextTime)
        //{
        //    print("check");
        //    totalCurrentSeconds = (int)sT.totalGameSeconds;
        //    approximationTime(eventList.ToArray());
        //    nextTime += interval;
        //}
        //if (temp_nextEvent != null)
        //{
        //    if (totalCurrentSeconds > nextEventTime && totalCurrentSeconds < endEvevtTime && !temp_nextEvent.eventPrefab.activeSelf)
        //    {
        //        temp_nextEvent.eventPrefab.SetActive(true);

        //        print("show");
        //    }


        //    if (totalCurrentSeconds > endEvevtTime - 1 && temp_nextEvent.eventPrefab.activeSelf && !IsKillParticle)
        //    {
        //        print("hide");
        //        print($"����ð����� ����ð����� ũ��");
        //        temp_nextEvent.eventPrefab.SetActive(false);
        //        //approximationTime(eventList.ToArray());
        //    }

        //}

    }



    private void approximationTime(TimeEventAttribute[] eventlist)
    {
        int eventlistT = 0;
        for (int i = 0; i < eventlist.Length; i++)
        {
            eventlistT = eventlist[i].secondForEvent;
            endEvevtTime = eventlist[i].secondForEndEvent;
            Debug.Log($"����Ʈ({eventlist[i].name})");
            Debug.Log($"����ð�({totalCurrentSeconds})�� �̺�Ʈ���۽ð�{eventlistT} ���� ũ�ٴ� �񱳴�{totalCurrentSeconds > eventlistT}");
            if (totalCurrentSeconds > eventlistT && totalCurrentSeconds < endEvevtTime)
            {
                    min = Mathf.Abs(eventlistT - totalCurrentSeconds);
                    Debug.Log($"�ּҰ��� {min}");
                    temp_nextEvent = eventlist[i];
                    Debug.Log("������ ������(" + eventlist[i].name + ")�� �ֱ�");
                    nextEventTime = eventlistT;
                    endEvevtTime = eventlist[i].secondForEndEvent;

            }
    
        }

    }
}
