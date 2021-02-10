using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
public class FacialPattenColorCorrection : EditorWindow
{
    public float ColorCorrection = 0.5f;
    public int BlurDistance = 10;
    public int BlurItCount = 1;
    [SerializeField]
    public Color[] Palette = new Color[9];
    public Texture2D TestTexture;
    public string OutPutPath = "Assets/Textures/FacialPattenTextureMask/";
    public string InputPath = "Assets/Textures/FacialPattenTextureCorrection/";

    List<Texture2D> importTextures;
    //利用构造函数来设置窗口名称
    FacialPattenColorCorrection()
    {
        this.titleContent = new GUIContent("脸谱校色");
    }

    //添加菜单栏用于打开窗口
    [MenuItem("Windows/脸谱校色")]
    static void showWindow()
    {
        EditorWindow.GetWindow(typeof(FacialPattenColorCorrection));
    }
    void OnGUI()
    {
        GUILayout.BeginVertical();

        //绘制标题
        GUILayout.Space(10);
        GUILayout.Label("校正脸谱颜色");
        //脸谱调色板
        GUILayout.Space(10);
        Palette[0] = new Color(0, 0, 0, 1);// (Color)EditorGUILayout.ColorField("黑色", Palette[0]);
        Palette[1] = new Color(1, 1, 1, 1);//(Color)EditorGUILayout.ColorField("白色", Palette[1]);
        Palette[2] = new Color(0.8f, 0.8f, 0.8f, 1);//(Color)EditorGUILayout.ColorField("灰色", Palette[2]);
        Palette[3] = new Color(0.8f, 0.1f, 0.1f, 1);//(Color)EditorGUILayout.ColorField("红色", Palette[3]);
        Palette[4] = new Color(1, 1f, 0, 1);//(Color)EditorGUILayout.ColorField("黄色", Palette[4]);
        Palette[5] = new Color(0, 0.3f, 0.9f, 1);//(Color)EditorGUILayout.ColorField("蓝色", Palette[5]);
        Palette[6] = new Color(0.2f, 0.7f, 0.2f, 1);//(Color)EditorGUILayout.ColorField("绿色", Palette[6]);
        Palette[7] = new Color(1, 0.75f, 0.8f, 1);//(Color)EditorGUILayout.ColorField("粉色", Palette[7]);
        Palette[8] = new Color(1, 0.88f, 0.7f, 1);//(Color)EditorGUILayout.ColorField("肉色", Palette[8]);

        //参数
        GUILayout.Space(10);
        ColorCorrection = EditorGUILayout.FloatField("校正强度", ColorCorrection);
        BlurItCount = EditorGUILayout.IntField("模糊迭代次数", BlurItCount);
        BlurDistance = EditorGUILayout.IntField("模糊距离", BlurDistance);

        //文件路径

        InputPath = EditorGUILayout.TextField("输入路径", InputPath);
        OutPutPath = EditorGUILayout.TextField("输出路径", OutPutPath);

        //测试图片
        TestTexture = (Texture2D)EditorGUILayout.ObjectField("测试图片", TestTexture, typeof(Texture2D), true);
        EditorGUILayout.Space();

        //添加名为"Save Bug"按钮，用于调用SaveBug()函数
        if (GUILayout.Button("Go"))
        {
            // LoadTextures();
            // ProcessTextures();
             Texture2D mask1, mask2, result;
            result = DownSample(TestTexture, 256, 256);
            GenerateMask(TestTexture, out result, out mask1, out mask2);
            result = BlurTexture(result, BlurItCount, BlurDistance);            
            mask1 = BlurTexture(mask1, BlurItCount, BlurDistance);
            mask2 = BlurTexture(mask2, BlurItCount, BlurDistance);
            SaveRenderTextureToPNG(mask1, OutPutPath + TestTexture.name + "_mask1.png");
            SaveRenderTextureToPNG(mask2, OutPutPath + TestTexture.name + "_mask2.png");
            SaveRenderTextureToPNG(result, OutPutPath + TestTexture.name + ".png");
        }


        GUILayout.EndVertical();
    }

    void LoadTextures()
    {

        //获取指定路径下面的所有资源文件  
        if (Directory.Exists(InputPath))
        {

            DirectoryInfo direction = new DirectoryInfo(InputPath);
            FileInfo[] files = direction.GetFiles("*");
            for (int i = 0; i < files.Length; i++)
            {
                //忽略关联文件
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                importTextures.Add(AssetDatabase.LoadAssetAtPath<Texture2D>(InputPath + files[i].Name));
            }
        }
    }
    void ProcessTextures()
    {
        foreach (var tex in importTextures)
        {            
            Texture2D mask1, mask2, result;
            result = DownSample(tex, 256, 256);
            GenerateMask(tex, out result, out mask1, out mask2);
            result = BlurTexture(result, BlurItCount, BlurDistance);            
            mask1 = BlurTexture(mask1, BlurItCount, BlurDistance);
            mask2 = BlurTexture(mask2, BlurItCount, BlurDistance);
            SaveRenderTextureToPNG(mask1, OutPutPath + tex.name + "_mask1.png");
            SaveRenderTextureToPNG(mask2, OutPutPath + tex.name + "_mask2.png");
            SaveRenderTextureToPNG(result, OutPutPath + tex.name + ".png");
        }
    }

    public void GenerateMask(Texture2D texture, out Texture2D result, out Texture2D mask1, out Texture2D mask2)
    {
        result = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, true, true);
        mask1 = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, true, true);
        mask2 = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, true, true);
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Color color = texture.GetPixel(x, y);
                Color mask1Color, mask2Color, resultColor;
                int colorChannelId = DecideColorChannel(color, out resultColor);
                SetMaskPixel(colorChannelId, out mask1Color, out mask2Color);
                mask1.SetPixel(x, y, mask1Color);
                mask2.SetPixel(x, y, mask2Color);
                result.SetPixel(x, y, resultColor);
            }
        }
        result.Apply();
        mask1.Apply();
        mask2.Apply();
    }

    void SetMaskPixel(int id, out Color mask1Color, out Color mask2Color)
    {
        mask1Color = new Color(0, 0, 0, 0);
        mask2Color = new Color(0, 0, 0, 0);
        if (id >= 0)
        {
            if (id < 4)
            {
                mask1Color[id] = 1;
            }
            else
            {
                mask2Color[id - 4] = 1;
            }
        }
    }

    int DecideColorChannel(Color orgin, out Color resultColor)
    {
        resultColor = orgin;
        if (orgin.a < 0.1f)
        {
            return -1;
        }
        int resultID = -1;
        float minColorDis = 999;
        float H, S, V;
        Color.RGBToHSV(orgin, out H, out S, out V);

        if (Vector3.SqrMagnitude(new Vector3(orgin.r - Palette[8].r, orgin.g - Palette[8].g, orgin.b - Palette[8].b)) < 0.025f && S>0.125f)//判断肉色
        {
            resultColor = Palette[8];
            resultColor.a = 0;
        }
        else if (V < 0.2f || (S<0.3f&&V<0.4f))//判断黑色
        {
            resultID = 0;
            resultColor = Color.Lerp(orgin, Palette[0], ColorCorrection);
        }
        else if (V > 0.9f && S < 0.1f)//判断白色
        {
            resultID = 1;
            resultColor = Color.Lerp(orgin, Palette[1], ColorCorrection);
        }
        else if (S < 0.25f)//判断灰色
        {
            resultID = 2;
            resultColor = Color.Lerp(orgin, Palette[2], ColorCorrection);
        }
        else
        {
            for (int i = 3; i < 8; i++)
            {
                float colorDis = Vector3.SqrMagnitude(new Vector3(orgin.r - Palette[i].r, orgin.g - Palette[i].g, orgin.b - Palette[i].b));
                //float paletteH, paletteS, paletteV;
                //Color.RGBToHSV(Palette[i], out paletteH, out paletteS, out paletteV);
                //float colorDis = (paletteH - H) * (paletteH - H);
                if (colorDis < minColorDis)
                {
                    minColorDis = colorDis;
                    resultID = i;
                }
            }
            resultColor = Color.Lerp(orgin, Palette[resultID], ColorCorrection);
        }

        return resultID;
    }

    public Texture2D DownSample(Texture2D texture, int width, int height)
    {
        Texture2D result = new Texture2D(width, height, TextureFormat.ARGB32, true, true);
        float scaleW = (float)texture.width / width;
        float scaleH = (float)texture.height / height;
        for (int x = 0; x < result.width; x++)
        {
            for (int y = 0; y < result.height; y++)
            {
                result.SetPixel(x, y, texture.GetPixel(Mathf.Min(texture.width - 1, (int)(x * scaleW)), Mathf.Min(texture.height - 1, (int)(y * scaleH))));
            }
        }
        return result;
    }
    public Texture2D BlurTexture(Texture2D texture, int iterator, int blurDistance)
    {
        for (int i = 0; i < iterator; i++)
        {
            texture = BlurTexture(texture);
        }
        return texture;
    }
    Texture2D BlurTexture(Texture2D texture)
    {
        Texture2D result = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, true, true);

        for (int x = 0; x < result.width; x++)
        {
            for (int y = 0; y < result.height; y++)
            {
                Color color = new Color(0, 0, 0, 0);
                color += texture.GetPixel((int)Mathf.Max(x - Random.Range(1, 2.0f) * BlurDistance, 0), y) * 0.0545f;
                color += texture.GetPixel((int)Mathf.Max(x - Random.Range(0, 1.0f) * BlurDistance, 0), y) * 0.2442f;
                color += texture.GetPixel(x, y) * 0.4026f;
                color += texture.GetPixel((int)Mathf.Min(x + Random.Range(0, 1.0f) * BlurDistance, texture.width - 1), y) * 0.2442f;
                color += texture.GetPixel((int)Mathf.Min(x + Random.Range(1, 2.0f) * BlurDistance, texture.width - 1), y) * 0.0545f;
                result.SetPixel(x, y, color);
            }
        }
        for (int x = 0; x < result.width; x++)
        {
            for (int y = 0; y < result.height; y++)
            {
                Color color = new Color(0, 0, 0, 0);
                color += result.GetPixel(x, (int)Mathf.Max(y - Random.Range(1, 2.0f) * BlurDistance, 0)) * 0.0545f;
                color += result.GetPixel(x, (int)Mathf.Max(y - Random.Range(0, 1.0f) * BlurDistance, 0)) * 0.2442f;
                color += result.GetPixel(x, y) * 0.4026f;
                color += result.GetPixel(x, (int)Mathf.Min(y + Random.Range(0, 1.0f) * BlurDistance, texture.height - 1)) * 0.2442f;
                color += result.GetPixel(x, (int)Mathf.Min(y + Random.Range(1, 2.0f) * BlurDistance, texture.height - 2)) * 0.0545f;         
                result.SetPixel(x, y, color);
            }
        }
        result.Apply();
        return result;
    }
    public bool SaveRenderTextureToPNG(Texture2D texture, string path)
    {
        byte[] _bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(path, _bytes);

        Debug.LogError("Save: " + path);
        return true;

    }
}
