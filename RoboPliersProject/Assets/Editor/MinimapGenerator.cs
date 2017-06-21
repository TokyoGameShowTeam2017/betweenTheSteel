using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MinimapGenerator : EditorWindow
{
    private GameObject parent;
    private GameObject prefab;
    private Material minimapMaterial;
    private float minimapScale = 1;

    private List<MinimapObject> _minimapObjects;

    [MenuItem("Assets/MinimapGenerator")]
    static void Init()
    {
        EditorWindow.GetWindow<MinimapGenerator>(true, "MinimapGenerator");
    }

    void OnEnable()
    {
        if (Selection.gameObjects.Length > 0) parent = Selection.gameObjects[0];
    }

    void OnSelectionChange()
    {
        if (Selection.gameObjects.Length > 0) prefab = Selection.gameObjects[0];
        Repaint();
    }

    void OnGUI()
    {
        try
        {
            prefab = EditorGUILayout.ObjectField("Minimap", prefab, typeof(GameObject), true) as GameObject;

            minimapMaterial = EditorGUILayout.ObjectField("MinimapMaterial", minimapMaterial, typeof(Material), true) as Material;

            //GUILayout.Label("MinimapScale ", EditorStyles.boldLabel);
            minimapScale = float.Parse(EditorGUILayout.TextField("MinimapScale", minimapScale.ToString()));

            GUILayout.Label("", EditorStyles.boldLabel);
            if (GUILayout.Button("Create")) Create(prefab, minimapScale, minimapMaterial);
        }
        catch (System.FormatException) { }
    }

    public void Create(GameObject parent, float scale, Material material)
    {
        _minimapObjects = new List<MinimapObject>();

        GameObject _newObj = Instantiate(parent);
        _newObj.name = "Minimap";
        _newObj.transform.position = parent.transform.position + Vector3.right * 100;
        _newObj.transform.localScale = parent.transform.localScale * scale;

        List<GameObject> list = GetAll(_newObj);
        List<GameObject> _gavelage = new List<GameObject>();

        foreach (GameObject obj in list)
        {
            //ライトを排除
            Light _l = obj.GetComponent<Light>();

            if (_l != null)
            {
                _gavelage.Add(obj);
            }

            //コライダーを排除
            Collider _c = obj.GetComponent<Collider>();

            if (_c != null)
            {
                DestroyImmediate(_c);
            }

            //カスタムスクリプトを削除
            MonoBehaviour _m = obj.GetComponent<MonoBehaviour>();

            if (_m != null)
            {
                DestroyImmediate(_m);
            }

            //リフレクションプローブを排除
            ReflectionProbe _p = obj.GetComponent<ReflectionProbe>();

            if (_p != null)
            {
                _gavelage.Add(obj);
            }

            Renderer _r = obj.GetComponent<Renderer>();

            if (_r != null)
            {
                Material[] mats = _r.sharedMaterials;

                for (int i = 0; i < mats.Length; i++) 
                {
                    mats[i] = material;
                }

                _r.sharedMaterials = mats;
            }
        }
        Debug.Log(_gavelage.Count);

        for(int i = 0; i < _gavelage.Count; i++)
        {
            DestroyImmediate(_gavelage[i]);
        }


    }

    private List<GameObject> GetAll(GameObject obj)
    {
        List<GameObject> allChildren = new List<GameObject>();
        GetChildren(obj, ref allChildren);
        return allChildren;
    }

    //子要素を取得してリストに追加
    private void GetChildren(GameObject obj, ref List<GameObject> allChildren)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
        if (children.childCount == 0)
        {
            return;
        }
        foreach (Transform ob in children)
        {
            allChildren.Add(ob.gameObject);
            GetChildren(ob.gameObject, ref allChildren);
        }
    }
}
