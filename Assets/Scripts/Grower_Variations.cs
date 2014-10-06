using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grower_Variations : Grower
{
    #region exposed

    public int Seed = 0;

    public float L = 2.0f;
    public float L_delta = 0.02f;

    public float MaxAngleDeviationLeft = 25.0f;
    public float MaxAngleDeviationRight = 24.0f;

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region Grower

    public override TreeData Grow()
    {
        if(L_delta < 0.01f)
        {
            return new TreeData();
        }

        TreeData.Branch branch = new TreeData.Branch();

        Vector2 dir = Vector2.up;
        Vector2 pos = Vector3.zero;
        
        float l = 0.0f;

        float width = 0.05f;
        float dw = -0.01f;

        Random.seed = Seed;
        while(l <= L)
        {
            branch.AddVertex(new TreeData.Vertex(pos, dir, width));

            float angle = Random.Range(-MaxAngleDeviationRight, MaxAngleDeviationLeft);

            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            dir = rot * dir;

            l += L_delta;           

            pos += dir * L_delta;
            width += L_delta * dw;
        }

        TreeData treeData = new TreeData();
        treeData.AddBranch(branch);

        return treeData;
    }

    #endregion
}
