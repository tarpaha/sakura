using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grower_Parts : Grower
{
    #region exposed

    public int Seed;

    public float L;
    public float L_delta;

    public int PartsCount;
    public float PartLengthDeviation;

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region Grower

    public override TreeData Grow()
    {
        Random.seed = Seed;
        List<float> lengts = GetPartLengths(L, PartsCount, PartLengthDeviation);

        TreeData.Branch branch = new TreeData.Branch();

        Vector2 dir = Vector2.up;
        Vector2 pos = Vector3.zero;
        
        float l = 0.0f;

        float size = 0.05f;
        float ds = -0.01f;
        
        Random.seed = Seed;
        while(l <= L)
        {
            branch.AddVertex(new TreeData.Vertex(pos, dir, size));

            dir = (dir + 0.15f * Random.insideUnitCircle).normalized;
            l += L_delta;
            
            pos += dir * L_delta;
            size += L_delta * ds;
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
