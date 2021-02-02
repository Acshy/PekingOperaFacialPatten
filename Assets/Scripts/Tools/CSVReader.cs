using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CSVReader : EditorWindow
{
    TextAsset CsvFile;
    string IconImagePath = "";
    string TextureImagePath = "";
    string MaskImagePath = "";
    string SavePath = "";



    //利用构造函数来设置窗口名称
    CSVReader()
    {
        this.titleContent = new GUIContent("脸谱信息读取");
    }

    //添加菜单栏用于打开窗口
    [MenuItem("Windows/脸谱信息读取工具")]
    static void showWindow()
    {
        EditorWindow.GetWindow(typeof(CSVReader));
    }
    void OnGUI()
    {
        GUILayout.BeginVertical();

        //绘制标题
        GUILayout.Space(10);
        GUILayout.Label("读取csv脸谱信息文件转存为scriptObject");
        //CSV文件
        GUILayout.Space(10);
        CsvFile = (TextAsset)EditorGUILayout.ObjectField("Buggy Game Object", CsvFile, typeof(TextAsset), true);
        //路径
        GUILayout.Space(10);
        IconImagePath = EditorGUILayout.TextField("脸谱缩略图路径", IconImagePath);
        TextureImagePath = EditorGUILayout.TextField("脸谱贴图路径", TextureImagePath);
        MaskImagePath = EditorGUILayout.TextField("脸谱蒙版路径", MaskImagePath);
        GUILayout.Space(10);
        SavePath = EditorGUILayout.TextField("保存路径", SavePath);




        EditorGUILayout.Space();

        //添加名为"Save Bug"按钮，用于调用SaveBug()函数
        if (GUILayout.Button("Save Facial Patten"))
        {
            ReadCSV();
        }


        GUILayout.EndVertical();
    }

    //用于保存当前信息
    void ReadCSV()
    {
        TextAsset data = CsvFile;
        FacialPattenScriptableObject[] objs = CSVSerializer.Deserialize<FacialPattenScriptableObject>(data.text);
        
        for(int i=0;i<objs.Length;i++)
        {
            string path = SavePath + "FacialPatten_"+objs[i].FileName + ".asset";
            FacialPattenScriptableObject file = AssetDatabase.LoadAssetAtPath<FacialPattenScriptableObject>(path);
            if (file == null)
            {
                file = new FacialPattenScriptableObject();
                AssetDatabase.CreateAsset(file, path);
            }
            file.CopyValue(objs[i],IconImagePath,TextureImagePath,MaskImagePath);
            EditorUtility.SetDirty(file);
            Debug.LogError("保存文件："+objs[i].FileName);
        }
        AssetDatabase.SaveAssets();
    }



}