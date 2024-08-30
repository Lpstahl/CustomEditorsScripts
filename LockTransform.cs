using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LockTransform : MonoBehaviour
{
    public bool isLocked = false;

    private void Update()
    {
        if (!Application.isPlaying)
        {
            if (isLocked)
            {
                // Impede que as transformações sejam alteradas no Editor
                Transform transform = GetComponent<Transform>();
                transform.hideFlags = HideFlags.NotEditable;
            }
            else
            {
                // Permite que as transformações sejam alteradas no Editor
                Transform transform = GetComponent<Transform>();
                transform.hideFlags = HideFlags.None;
            }
        }
    }
}

[CustomEditor(typeof(LockTransform))]
public class LockTransformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LockTransform lockTransform = (LockTransform)target;

        // Botão para habilitar/desabilitar o travamento
        if (GUILayout.Button(lockTransform.isLocked ? "Unlock Transform" : "Lock Transform"))
        {
            lockTransform.isLocked = !lockTransform.isLocked;
            SceneView.RepaintAll(); // Atualiza a cena para refletir o novo estado de bloqueio
        }

        // Atualiza o estado do bloqueio
        if (lockTransform.isLocked)
        {
            EditorGUILayout.HelpBox("Transform is locked.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("Transform is unlocked.", MessageType.Warning);
        }

        // Impede que o script seja removido acidentalmente
        GUI.enabled = false;
        base.OnInspectorGUI();
        GUI.enabled = true;
    }
}

// Classe para impedir a modificação do transform na cena
[InitializeOnLoad]
public static class LockTransformHandler
{
    static LockTransformHandler()
    {
        // Adiciona o callback para verificar a modificação do transform na cena
        SceneView.duringSceneGui += OnScene;
    }

    private static void OnScene(SceneView sceneView)
    {
        // Impede a movimentação dos objetos bloqueados
        if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
        {
            foreach (var obj in Selection.transforms)
            {
                LockTransform lockTransform = obj.GetComponent<LockTransform>();
                if (lockTransform != null && lockTransform.isLocked)
                {
                    Event.current.Use();
                }
            }
        }
    }
}
