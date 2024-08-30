using UnityEditor;
using UnityEngine;

public class CustomHeaderObjects : MonoBehaviour
{
    public Color textColor = Color.white;
    public Color backgroundColor = Color.red;
    public string headerName = "Header";

    private void OnValidate()
    {
        EditorApplication.RepaintHierarchyWindow();
    }
}