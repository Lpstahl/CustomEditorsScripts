using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CustomHierarchyMenu : EditorWindow
{
    [MenuItem("GameObject/Create Custom Header")]
    static void CreateCustomHeader(MenuCommand _menuCommand)
    {
        GameObject obj = new GameObject("Header");
        Undo.RegisterCreatedObjectUndo(obj, "Create Custom Header");

        GameObjectUtility.SetParentAndAlign(obj, _menuCommand.context as GameObject);
        obj.AddComponent<CustomHeaderObjects>();
        Selection.activeObject = obj;
    }
}

// Editor personalizado para CustomHeaderObjects para adicionar caixa de texto no inspetor
[CustomEditor(typeof(CustomHeaderObjects))]
public class CustomHeaderObjectsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CustomHeaderObjects customHeader = (CustomHeaderObjects)target;

        // Caixa de texto para o nome do cabeçalho
        customHeader.headerName = EditorGUILayout.TextField("Header Name", customHeader.headerName);

        // Campos de cor para texto e fundo
        customHeader.textColor = EditorGUILayout.ColorField("Text Color", customHeader.textColor);
        customHeader.backgroundColor = EditorGUILayout.ColorField("Background Color", customHeader.backgroundColor);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(customHeader);
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}

// Editor para desenhar o objeto com cores personalizadas na hierarquia
[InitializeOnLoad]
public static class CustomHierarchyDrawer
{
    static CustomHierarchyDrawer()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj != null)
        {
            CustomHeaderObjects header = obj.GetComponent<CustomHeaderObjects>();
            if (header != null)
            {
                // Desenhar fundo
                EditorGUI.DrawRect(selectionRect, header.backgroundColor);

                // Desenhar texto
                EditorGUI.LabelField(selectionRect, header.headerName, new GUIStyle
                {
                    normal = new GUIStyleState { textColor = header.textColor },
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.MiddleCenter
                });
            }
        }
    }
}