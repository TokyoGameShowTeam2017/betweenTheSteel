using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SamneilCapture : MonoBehaviour {

    // ThumbnailCamera
    private Camera camera;
    private RenderTexture RenderTextureRef;
    public bool capture;
    private string Filename;

    public int stageNum;

    private void Start()
    {

    }

    void OnPostRender()
    {
        if (capture)
        {
            camera = GetComponent<Camera>();
            RenderTextureRef = camera.targetTexture;
            // キャプチャー
            Take();
        }

        // 破棄
        Destroy(this);
    }

    protected void Take()
    {
        Texture2D tex = new Texture2D(RenderTextureRef.width, RenderTextureRef.height, TextureFormat.RGB24, false);
        RenderTexture.active = RenderTextureRef;
        tex.ReadPixels(new Rect(0, 0, RenderTextureRef.width, RenderTextureRef.height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Object.Destroy(tex);


        Filename = "/Fujimaki/CaptureImage/Stage" + stageNum + ".png";
        //Write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + Filename, bytes);
    }
}
