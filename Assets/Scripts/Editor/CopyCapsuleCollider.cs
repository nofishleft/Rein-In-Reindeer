using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class CopyCapsuleCollider : EditorWindow
{
    public List<GameObject> obs;
    public CapsuleCollider m;

    [MenuItem("Tools/Replace Collider/Capsule")]
    static void OpenMaterialReplacer()
    {
        List<GameObject> obs = new List<GameObject>();

        foreach (GameObject o in Selection.gameObjects)
        {
            obs.Add(o);
        }

        CopyCapsuleCollider window = (CopyCapsuleCollider)EditorWindow.GetWindow(typeof(CopyCapsuleCollider));

        window.obs = obs;

        window.Show();
    }

    void OnGUI()
    {
        m = (CapsuleCollider)EditorGUILayout.ObjectField("Collider", m, typeof(CapsuleCollider), false);

        if (GUILayout.Button("Replace"))
        {
            foreach (GameObject o in obs)
            {
                CapsuleCollider c = o.AddComponent<CapsuleCollider>();

                c.center = m.center;
                c.direction = m.direction;
                c.height = m.height;
                c.radius = m.radius;
            }
        }
    }
}