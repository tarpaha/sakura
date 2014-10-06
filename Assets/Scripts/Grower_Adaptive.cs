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

    public float Width01 = 0.1f;
    public float Width02 = 0.01f;

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

        DebugDrawBounds();

        TreeData.Branch branch = new TreeData.Branch();

        Vector2 dir = Vector2.up;
        Vector2 pos = Vector3.zero;
        
        float l = 0.0f;

        float width = Width01;
        float dw = (Width02 - Width01) / (L / L_delta);

        Random.seed = Seed;
        while(l <= L)
        {
            branch.AddVertex(new TreeData.Vertex(pos, dir, width));

            dir = GetNextDir(pos, dir);

            l += L_delta;           

            pos += dir * L_delta;
            width += dw;
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
        Vector2[] directions = new Vector2[LOOKING_STEPS];
        Vector2[] positions = new Vector2[LOOKING_STEPS];
        float[] probability = new float[LOOKING_STEPS];
        for(int i = 0; i < LOOKING_STEPS; i++)
        {
            float a = -AngleDeviation + 2.0f * AngleDeviation * i / (LOOKING_STEPS - 1);
            directions[i] = Quaternion.AngleAxis(a, Vector3.forward) * dir;
            positions[i] = pos + directions[i] * L_delta;
            probability[i] = 1.0f;
        }

        for(int i = 0; i < LOOKING_STEPS; i++)
        {
            if(positions[i].x >= Side)
            {
                if(positions[i].x <= Side + SideGap)
                {
                    probability[i] = 1.0f - (positions[i].x - Side) / SideGap;
                }
                else
                {
                    probability[i] = 0.0f;
                }
            }

            if(positions[i].x <= -Side)
            {
                if(positions[i].x >= -Side - SideGap)
                {
                    probability[i] = 1.0f - (Side - positions[i].x) / SideGap;
                }
                else
                {
                    probability[i] = 0.0f;
                }
            }

            if(directions[i].y < 0)
            {
                probability[i] = 0.0f;
            }
        }


        float randomSlice = Random.value;
        List<int> possibleIndices = new List<int>();
        for(int i = 0; i < LOOKING_STEPS; i++)
        {
            if(probability[i] > randomSlice)
            {
                possibleIndices.Add(i);
            }
        }

        int index = Random.Range(0, LOOKING_STEPS);

        if(possibleIndices.Count > 0)
        {
            index = possibleIndices[Random.Range(0, possibleIndices.Count)];
        }
        else
        {
            Vector2 v = pos.x > 0 ? new Vector2(-1.0f, 1.0f) : new Vector2(1.0f, 1.0f);
            index = GetBestDirectionIndex(directions, v);
        }

        return directions[index];
    }

    private void DebugDrawBounds()
    {
        Debug.DrawLine(new Vector2(Side, 0.0f), new Vector2(Side, 2.0f));
        Debug.DrawLine(new Vector2(Side+SideGap, 0.0f), new Vector2(Side+SideGap, 2.0f));
        
        Debug.DrawLine(new Vector2(-Side, 0.0f), new Vector2(-Side, 2.0f));
        Debug.DrawLine(new Vector2(-Side-SideGap, 0.0f), new Vector2(-Side-SideGap, 2.0f));
    }

    private static int GetBestDirectionIndex(Vector2[] directions, Vector2 v)
    {
        int index = 0;
        float dot_max = Vector2.Dot(directions[0], v);

        for(int i = 1; i < directions.Length; i++)
        {
            float dot = Vector2.Dot(directions[i], v);
            if(dot > dot_max)
            {
                dot_max = dot;
                index = i;
            }
        }

        return index;
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region const

    private const int LOOKING_STEPS = 100;

    #endregion
}
