using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Editor_ModifyFont : EditorWindow
{
    private static Editor_ModifyFont _window;

    public Font ModifyFontText;

    public FontStyle ModifyFontStyle;

    Transform[] childs = null;

    bool isThisObject = true;

    [MenuItem("Tools/修改字体")]
    public static void GUIDRefReplaceWin()
    {
        Rect wr = new Rect(0, 0, 300, 200);
        // true 表示不能停靠的
        _window = (Editor_ModifyFont)GetWindowWithRect(typeof(Editor_ModifyFont), wr, true, "修改字体 (っ °Д °;)っ");
        _window.Show();

    }
    void OnGUI()
    {
        // 要被替换的（需要移除的）
        GUILayout.Space(20);

        childs = Selection.transforms;

        ModifyFontText = (Font)EditorGUILayout.ObjectField("字体", ModifyFontText, typeof(Font), true);

        ModifyFontStyle = (FontStyle)EditorGUILayout.EnumPopup("字形", ModifyFontStyle);

        if (childs.Length == 0)
        {
            GUILayout.Label("修改当前场景全部字体");
            isThisObject = false;
        }
        else
        {
            string str = "";
            foreach (var item in childs)
            {
                str += " '" + item.name + "' ";
            }
            GUILayout.Label("修改 " + str + " 物体下的所有字体");
            isThisObject = true;
        }
        if (GUILayout.Button("开始修改!"))
        {
            modifyFont();
            GameObject go = new GameObject();
            EditorUtility.SetDirty(go);
            DestroyImmediate(go);//刷新场景更新字体
        }
        //GUILayout.Label ("\t╔════════════════╗");
        //GUILayout.Label ("\t║\t记得保存场景!\t   ║");
        //GUILayout.Label ("\t║\t记得保存场景!\t   ║");
        //GUILayout.Label ("\t╚════════════════╝");
    }
    public void modifyFont()
    {
        if (ModifyFontText)
        {
            if (isThisObject)
            {
                foreach (Transform transforms in childs)
                {
                    foreach (Transform childs in transforms.GetComponentsInChildren<Transform>(true))
                    {
                        modifyText(childs.gameObject);
                    }
                }
            }
            else
            {
                foreach (GameObject item in GetAllSceneObjectsWithInactive())
                {
                    modifyText(item);
                }
            }
        }
    }
    void modifyText(GameObject obj)
    {
        if (obj.GetComponent<Text>())
        {
            Debug.Log("修改Text" + obj.name);
            obj.GetComponent<Text>().font = ModifyFontText;
            obj.GetComponent<Text>().fontStyle = ModifyFontStyle;
        }
        if (obj.GetComponent<TextMesh>())
        {
            Debug.Log("修改TextMesh" + obj.name);
            obj.GetComponent<TextMesh>().font = ModifyFontText;
            obj.GetComponent<TextMesh>().fontStyle = ModifyFontStyle;
        }
    }

    private static List<GameObject> GetAllSceneObjectsWithInactive()
    {
        var allTransforms = Resources.FindObjectsOfTypeAll(typeof(Transform));
        var previousSelection = Selection.objects;
        Selection.objects = allTransforms.Cast<Transform>()
            .Where(x => x != null)
            .Select(x => x.gameObject)
            //如果你只想获取所有在Hierarchy中被禁用的物体，反注释下面代码
            //.Where(x => x != null && !x.activeInHierarchy)
            .Cast<UnityEngine.Object>().ToArray();

        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;

        return selectedTransforms.Select(tr => tr.gameObject).ToList();
    }

}