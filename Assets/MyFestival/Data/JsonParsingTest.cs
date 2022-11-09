using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public class LottoNumbers
{
    public List<Lotto> winning; 
}
[Serializable]
public class testCase //aray of objects
{
    public int test1;
    public string test2;
    public int[] test3;
}
[Serializable]
public class Lotto
{
    public int id;
    public string date;
    public int[] number;
    public int bonus;
    public testCase[] test; /* 추가 */

    public void printNumbers()
    {
        string str = "numbers : ";
        for (int i = 0; i < 6; i++) str += number[i] + " ";

        Debug.Log(str);
        Debug.Log("bonus : " + bonus);

        for (int i = 0; i < test.Length; i++)
        {
            Debug.Log("test1: " + test[i].test1);
            Debug.Log("test2: " + test[i].test2);

            for (int k = 0; k < test[i].test3.Length; k++)
                Debug.Log("test3 [" + k + "] " + test[i].test3[k]);
        }
    }
}
public class JsonParsingTest : MonoBehaviour
{
    Dictionary<int, Lotto> lottoDic = new Dictionary<int, Lotto>();
    private void Start()
    {

        string data = SaveLoadTemplete.SavePath + "LottoWinningNumber1.json";
        //JObject root = JObject.Parse(data);
        LottoNumbers lottoNumbers = DataFromPath(data);

        //foreach (var lt in lottoNumbers.winning)
        //{
        //    lottoDic.Add(lt.id, lt);
        //}

        //foreach (var ltkey in lottoDic.Keys)
        //{
        //    lottoDic[ltkey].printNumbers();
        //    Debug.Log("======");
        //}
        //foreach (KeyValuePair<int, Lotto> lt in lottoDic)
        //{
        //    Debug.Log(lt.Key);
        //    lt.Value.printNumbers();
        //    Debug.Log("=============");
        //}

        //배열내 배열 파싱
        foreach (Lotto lt in lottoNumbers.winning)
        {
            lt.printNumbers();
            Debug.Log("=============");
        }
    }
    public LottoNumbers DataFromPath(string path)
    {
        try
        {
            string rawJson;
            using (var reader = new StreamReader(path))
            {
                rawJson = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<LottoNumbers>(rawJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return default(LottoNumbers);
        }
    }
}
