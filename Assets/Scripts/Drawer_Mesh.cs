using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawer_Mesh : Drawer
{
    #region exposed

    public Material Material;

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region Drawer

    public override void Draw(TreeData treeData)
    {
        Clean();
        foreach(TreeData.Branch branch in treeData.Branches)
        {
            DrawBranch(branch);
        }
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region private functions

    private void Clean()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    private void DrawBranch(TreeData.Branch branch)
    {
        GameObject go = new GameObject("Branch");
        go.transform.parent = transform;

        MeshRenderer re = go.AddComponent<MeshRenderer>();
        re.material = Material;

        MeshFilter meshFilter = go.AddComponent<MeshFilter>();
        meshFilter.mesh = CreateFromVerticesList(new List<TreeData.Vertex>(branch.Vertices));
    }

    private Mesh CreateFromVerticesList(List<TreeData.Vertex> list)
    {
        Vector3[] vertices = new Vector3[list.Count * 2];
        for(int i = 0; i < list.Count; i++)
        {
            Vector3 pos = list[i].Pos;
            Vector3 perp = new Vector3(list[i].Dir.y, -list[i].Dir.x);

            perp *= list[i].Size / 2;

            vertices[i * 2 + 0] = pos - perp;
            vertices[i * 2 + 1] = pos + perp;
        }

        int[] triangles = new int[6 * (list.Count - 1)];
        for(int i = 0; i < list.Count - 1; i++)
        {
            triangles[6 * i + 0] = 2 * i + 0;
            triangles[6 * i + 1] = 2 * i + 3;
            triangles[6 * i + 2] = 2 * i + 1;

            triangles[6 * i + 3] = 2 * i + 0;
            triangles[6 * i + 4] = 2 * i + 2;
            triangles[6 * i + 5] = 2 * i + 3;
        }

        Vector2[] uv = new Vector2[list.Count * 2];
        float du = 1.0f / (list.Count - 1);
        for(int i = 0; i < list.Count; i++)
        {
            uv[i * 2 + 0] = new Vector2(0.0f, du * i);
            uv[i * 2 + 1] = new Vector2(1.0f, du * i);
        }
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    #endregion
}
