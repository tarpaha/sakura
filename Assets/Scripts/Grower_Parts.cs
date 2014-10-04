using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grower_Parts : Grower
{
    #region exposed

    public int Seed = 0;

    public float L = 2.0f;
    public float L_delta = 0.02f;

    public float MaxAngleDeviationLeft = 19.0f;
    public float MaxAngleDeviationRight = 18.0f;

    public int PartsCount = 4;
    public float PartLengthDeviation = 0.1f;

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

        Random.seed = Seed;
        List<float> lengts = GetPartLengths(L, PartsCount, PartLengthDeviation);

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


    private static List<float> GetPartLengths(float L, int partsCount, float deviationFactor)
    {
        List<float> result = new List<float>();

        float idealPartLength = L / partsCount;
        float maxDeviationLength = idealPartLength * deviationFactor;

        float remainingLength = L;
        for(int i = 0; i < partsCount; i++)
        {
            float partLength = idealPartLength + Random.Range(-maxDeviationLength, +maxDeviationLength);
            result.Add(partLength);
            remainingLength -= partLength;
        }

        result[partsCount - 1] += remainingLength;

        return result;
    }

    #endregion
}
