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

    [MenuItem("Tools/�޸�����")]
    public static void GUIDRefReplaceWin()
    {
        Rect wr = new Rect(0, 0, 300, 200);
        // true ��ʾ����ͣ����
        _window = (Editor_ModifyFont)GetWindowWithRect(typeof(Editor_ModifyFont), wr, true, "�޸����� (�� �㧥 ��;)��");
        _window.Show();

    }
    void OnGUI()
    {
        // Ҫ���滻�ģ���Ҫ�Ƴ��ģ�
        GUILayout.Space(20);

        childs = Selection.transforms;

        ModifyFontText = (Font)EditorGUILayout.ObjectField("����", ModifyFontText, typeof(Font), true);

        ModifyFontStyle = (FontStyle)EditorGUILayout.EnumPopup("����", ModifyFontStyle);

        if (childs.Length == 0)
        {
            GUILayout.Label("�޸ĵ�ǰ����ȫ������");
            isThisObject = false;
        }
        else
        {
            string str = "";
            foreach (var item in childs)
            {
                str += " '" + item.name + "' ";
            }
            GUILayout.Label("�޸� " + str + " �����µ���������");
            isThisObject = true;
        }
        if (GUILayout.Button("��ʼ�޸�!"))
        {
            modifyFont();
            GameObject go = new GameObject();
            EditorUtility.SetDirty(go);
            DestroyImmediate(go);//ˢ�³�����������
        }
        //GUILayout.Label ("\t�X�T�T�T�T�T�T�T�T�T�T�T�T�T�T�T�T�[");
        //GUILayout.Label ("\t�U\t�ǵñ��泡��!\t   �U");
        //GUILayout.Label ("\t�U\t�ǵñ��泡��!\t   �U");
        //GUILayout.Label ("\t�^�T�T�T�T�T�T�T�T�T�T�T�T�T�T�T�T�a");
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
            Debug.Log("�޸�Text" + obj.name);
            obj.GetComponent<Text>().font = ModifyFontText;
            obj.GetComponent<Text>().fontStyle = ModifyFontStyle;
        }
        if (obj.GetComponent<TextMesh>())
        {
            Debug.Log("�޸�TextMesh" + obj.name);
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
            //�����ֻ���ȡ������Hierarchy�б����õ����壬��ע���������
            //.Where(x => x != null && !x.activeInHierarchy)
            .Cast<UnityEngine.Object>().ToArray();

        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;

        return selectedTransforms.Select(tr => tr.gameObject).ToList();
    }

}