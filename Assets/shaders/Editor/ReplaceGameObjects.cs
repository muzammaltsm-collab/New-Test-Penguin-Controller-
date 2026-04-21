using UnityEngine;
using UnityEditor;
// CopyComponents - by Michael L. Croswell for Colorado Game Coders, LLC
// March 2010
//Modified by Kristian Helle Jespersen
//June 2011
//Modified by Connor Cadellin McKee for Excamedia
//April 2015
//Modified by Fernando Medina (fermmmm)
//April 2015
//Modified  by Ryan Mitchell (RyanMitchellGames.WordPress.com)
//January 2019

public class ReplaceGameObjects : ScriptableWizard
{
    public GameObject[] ObjectsToReplace;
    Object obj = null;

    [MenuItem("Tools/Replace GameObjects")]
    static void CreateWizard()
    {
        ReplaceGameObjects window = ScriptableObject.CreateInstance(typeof(ReplaceGameObjects)) as ReplaceGameObjects;
        window.ShowUtility();
    }

    void Apply()
    {
        Object[] gos = Selection.objects;
        foreach (Object go in gos)
        {
            GameObject oldObject = go as GameObject;
            GameObject newObjectHold = obj as GameObject;
            if (oldObject &&
                newObjectHold)
            {
                GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(newObjectHold);

                newObject.transform.SetParent(oldObject.transform.parent, true);
                newObject.transform.localPosition = oldObject.transform.localPosition;
                newObject.transform.localRotation = oldObject.transform.localRotation;
                newObject.transform.localScale = oldObject.transform.localScale;
                newObject.transform.name = oldObject.transform.name;
                DestroyImmediate(go);
            }
        }
    }

    void OnGUI()
    {
        obj = EditorGUILayout.ObjectField("Label:", obj, typeof(GameObject), true);

        string selectedName = "Selected:\r\n";
        Object[] gos = Selection.objects;
        foreach (Object go in gos)
        {
            selectedName += go.name + "\r\n";
        }
        EditorGUILayout.TextArea(selectedName);

        if (GUILayout.Button("Apply"))
            Apply();

        if (GUILayout.Button("Close"))
            Close();

    }

    void OnInspectorUpdate()
    {
        Repaint();
    }
}