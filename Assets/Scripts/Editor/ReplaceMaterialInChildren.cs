using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceMaterialInChildren : EditorWindow
{
    public List<MeshRenderer> r;
    public Material m;

    [MenuItem("Tools/Replace Materials")]
    static void OpenMaterialReplacer()
    {
        List<MeshRenderer> r = new List<MeshRenderer>();

        foreach (GameObject g in Selection.gameObjects)
        {
            MeshRenderer[] rs = g.GetComponentsInChildren<MeshRenderer>();

            if (rs == null || rs.Length == 0) continue;

            foreach (MeshRenderer ren in rs)
            {
                r.Add(ren);
            }
        }

        ReplaceMaterialInChildren window = (ReplaceMaterialInChildren)EditorWindow.GetWindow(typeof(ReplaceMaterialInChildren));

        window.r = r;

        window.Show();
    }

    void OnGUI()
    {
        m = (Material) EditorGUILayout.ObjectField("Material", m, typeof(Material), false);

        if (GUILayout.Button("Replace"))
        {
            foreach (MeshRenderer re in r)
            {
                re.material = m;
            }
        }
    }
}
