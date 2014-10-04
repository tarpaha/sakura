using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grower_Simple : Grower
{
    #region exposed

    public float L;
    public float L_delta;

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region Grower

    public override TreeData Grow()
    {
        TreeData.Branch branch = new TreeData.Branch();

        Vector2 dir = Vector2.up;
        Vector2 pos = Vector3.zero;
        
        float l = 0.0f;

        float witdh = 0.05f;
        float dw = -0.01f;
        
        while(l <= L)
        {
            branch.AddVertex(new TreeData.Vertex(pos, dir, witdh));

            dir = (dir + 0.15f * Random.insideUnitCircle).normalized;
            l += L_delta;
            
            pos += dir * L_delta;
            witdh += L_delta * dw;
        }

        TreeData treeData = new TreeData();
        treeData.AddBranch(branch);

        return treeData;
    }

    #endregion
}
