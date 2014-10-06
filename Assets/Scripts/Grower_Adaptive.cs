using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grower_Adaptive : Grower
{
    #region exposed

    public int Seed = 0;

    public float L = 2.0f;
    public float L_delta = 0.02f;

    public float AngleDeviation = 25.0f;

    public float Side    = 0.4f;
    public float SideGap = 0.2f;

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

        Debug.DrawLine(new Vector2(Side, 0.0f), new Vector2(Side, 2.0f));
        Debug.DrawLine(new Vector2(Side+SideGap, 0.0f), new Vector2(Side+SideGap, 2.0f));

        Debug.DrawLine(new Vector2(-Side, 0.0f), new Vector2(-Side, 2.0f));
        Debug.DrawLine(new Vector2(-Side-SideGap, 0.0f), new Vector2(-Side-SideGap, 2.0f));

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

            dir = GetNextDir(pos, dir);

            l += L_delta;           

            pos += dir * L_delta;
            width += L_delta * dw;
        }

        TreeData treeData = new TreeData();
        treeData.AddBranch(branch);

        return treeData;
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region private functions

    private Vector2 GetNextDir(Vector2 pos, Vector2 dir)
    {
        int steps = 100;
        Vector2[] dirs = new Vector2[steps];
        Vector2[] poss = new Vector2[steps];
        float[] pr = new float[steps];
        for(int i = 0; i < steps; i++)
        {
            float a = -AngleDeviation + 2.0f * AngleDeviation * i / (steps - 1);
            dirs[i] = Quaternion.AngleAxis(a, Vector3.forward) * dir;
            poss[i] = pos + dirs[i] * L_delta;
            pr[i] = 1.0f;
        }


        for(int i = 0; i < steps; i++)
        {
            if(poss[i].x >= Side)
            {
                if(poss[i].x <= Side + SideGap)
                {
                    pr[i] = 1.0f - (poss[i].x - Side) / SideGap;
                }
                else
                {
                    pr[i] = 0.0f;
                }
            }

            if(poss[i].x <= -Side)
            {
                if(poss[i].x >= -Side - SideGap)
                {
                    pr[i] = 1.0f - (Side - poss[i].x) / SideGap;
                }
                else
                {
                    pr[i] = 0.0f;
                }
            }

            if(dirs[i].y < 0)
            {
                pr[i] = 0.0f;
            }
        }


        float d = Random.value;
        List<int> ids = new List<int>();
        for(int i = 0; i < steps; i++)
        {
            if(pr[i] > d)
            {
                ids.Add(i);
            }
        }

        int index = Random.Range(0, steps);

        if(ids.Count > 0)
        {
            index = ids[Random.Range(0, ids.Count)];
        }
        else
        {
            Vector2 v = pos.x > 0 ? new Vector2(-1.0f, 1.0f) : new Vector2(1.0f, 1.0f);

            float t_max = Vector2.Dot(dirs[0], v);
            index = 0;
            for(int i = 1; i < steps; i++)
            {
                float t = Vector2.Dot(dirs[i], v);
                if(t > t_max)
                {
                    t_max = t;
                    index = i;
                }
            }
        }

        return dirs[index];
    }

    #endregion
}
