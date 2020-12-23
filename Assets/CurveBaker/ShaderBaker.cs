using UnityEngine;
using System.Collections;
using System.IO;

public class ShaderBaker : MonoBehaviour
{
    public GameObject objectToBake;
    public Material uvMaterial;
    public Material dilateMat;
    public Color backgroundColor;

    public Vector2Int textureDim;

    private bool capture;

    void Start()
    {
        capture = false;
        dilateMat.SetColor("_BackgroundColor", backgroundColor);
    }

    void Update()
    {
        capture = false;
        if (Input.GetKeyDown(KeyCode.M))
        {
            Bake();
        }
    }

    public void Bake()
    {
            RenderTexture rt = RenderTexture.GetTemporary(textureDim.x, textureDim.y);
            Mesh M = objectToBake.GetComponent<MeshFilter>().mesh;

            Graphics.SetRenderTarget(rt);
            GL.Clear(true, true, backgroundColor);
            GL.PushMatrix(); 
            GL.LoadOrtho(); 
            uvMaterial.SetPass(0);
            Graphics.DrawMeshNow(M, Matrix4x4.identity);
            Graphics.SetRenderTarget(null);

            RenderTexture rt2 = RenderTexture.GetTemporary(textureDim.x, textureDim.y);
            Graphics.Blit(rt, rt2, dilateMat);

            SaveTexture(rt2, objectToBake.name);

            RenderTexture.ReleaseTemporary(rt);
            RenderTexture.ReleaseTemporary(rt2);
            GL.PopMatrix();
    }
    private void SaveTexture(RenderTexture rt, string mname)
    {
        string fullPath = Application.dataPath + "/Textures/" + mname + ".png";

        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        
        byte[] _bytes = tex.EncodeToPNG();
        File.Delete(fullPath);
        File.WriteAllBytes(fullPath, _bytes);
        Debug.Log("SaveTexture File at: "+fullPath);
    }
}