using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Drawer_Simple : Drawer
{
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
        foreach(TreeData.Vertex vertex in branch.Vertices)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
            go.transform.parent = transform;
            go.transform.position = vertex.Pos;
            go.transform.localScale = vertex.Width * Vector3.one;
        }
    }

    #endregion
}
