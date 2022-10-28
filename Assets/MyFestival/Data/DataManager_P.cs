using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DataManager_P : MonoBehaviour
{

    public static DataManager_P Instance = null;


    [Serializable]
    public class Stats
    {
        int level;
        int hp;
        int attack;
    }
    [Serializable]
    public class StatData
    {
        public List<Stats> stats = new List<Stats>();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this.gameObject)
            Instance = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        TextAsset Text = Resources.Load<TextAsset>("userdata");
        print(Text.text);
        StatData data = JsonUtility.FromJson<StatData>(Text.text);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
