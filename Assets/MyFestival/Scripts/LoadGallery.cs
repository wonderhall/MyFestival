using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class LoadGallery : MonoBehaviour
{
    public RawImage img;


    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }
    public void OnclickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo selected = new FileInfo(file);
            //용량 제한
            if (selected.Length > 50000000) return;
            //불러오기
            if (!string.IsNullOrEmpty(file))
            {
                StartCoroutine(LoadImage(file));
            }
        });
    }

    IEnumerator LoadImage(string path)
    {
        yield return null;
        byte[] fileData = File.ReadAllBytes(path);
        string filename = Path.GetFileName(path).Split('.')[0];
        string savePath = Application.persistentDataPath + "/image";
        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

        File.WriteAllBytes(savePath + filename + ".png", fileData);
        var temp = File.ReadAllBytes(savePath + filename + ".png");
      
        //-->바이트배열을 택스쳐로 변경
        Texture2D tex = new Texture2D(0,0);
        tex.LoadImage(temp);
        //<--

        img.texture = tex;
    }
}
