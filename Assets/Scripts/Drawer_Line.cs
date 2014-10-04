using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawer_Line : Drawer
{
    #region exposed

    public Material LineMaterial;
    public Color ColorStart = Color.black;
    public Color ColorEnd = Color.black;

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
        List<TreeData.Vertex> vertices = new List<TreeData.Vertex>(branch.Vertices);

        GameObject go = new GameObject("Branch");
        go.transform.parent = transform;

        LineRenderer lr = go.AddComponent<LineRenderer>();
        lr.SetVertexCount(vertices.Count);
        lr.SetWidth(vertices[0].Width, vertices[vertices.Count-1].Width);
        lr.material = LineMaterial;
        lr.SetColors(ColorStart, ColorEnd);

        int index = 0;
        foreach(TreeData.Vertex vertex in vertices)
        {
            lr.SetPosition(index, vertex.Pos);
            index += 1;
        }
    }

    #endregion
}
