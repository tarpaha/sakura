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

    public override TreeData Grow(Vector2 pos, Vector2 dir)
    {
        if(L_delta < 0.01f)
        {
            return new TreeData();
        }

        DebugDrawBounds();

        TreeData.Branch branch = new TreeData.Branch();

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

    /// <summary>
    /// Create fan of possible directions, assign probability to each and select random
    /// </summary>
    private Vector2 GetNextDir(Vector2 pos, Vector2 dir)
    {
        List<GrowRecord> records = CreateRecords(pos, dir, L_delta, AngleDeviation, LOOKING_STEPS);

        ApplyLeftBound(records, Side, SideGap);
        ApplyRightBound(records, Side, SideGap);
        ApplyDirectionUp(records);

        float randomSlice = Random.value;
        List<GrowRecord> passedRecords = records.FindAll(r => r.prob > randomSlice);

        GrowRecord result = null;

        if(passedRecords.Count > 0)
        {
            result = passedRecords[Random.Range(0, passedRecords.Count)];
        }
        else
        {
            Vector2 v = pos.x > 0 ? new Vector2(-1.0f, 1.0f) : new Vector2(1.0f, 1.0f);
            result = GetBestDirectionIndex(records, v);
        }

        return result.dir;
    }

    private static List<GrowRecord> CreateRecords(Vector2 pos, Vector2 dir, float delta, float angleDeviation, float steps)
    {
        List<GrowRecord> records = new List<GrowRecord>();
        for(int i = 0; i < steps; i++)
        {
            float a = -angleDeviation + 2.0f * angleDeviation * i / (steps - 1);
            GrowRecord record = new GrowRecord();
            record.dir = Quaternion.AngleAxis(a, Vector3.forward) * dir;
            record.pos = pos + record.dir * delta;
            record.prob = 1.0f;
            records.Add(record);
        }
        return records;
    }

    private static void ApplyLeftBound(IEnumerable<GrowRecord> records, float side, float sideGap)
    {
        foreach(GrowRecord record in records)
        {
            if(record.pos.x <= -side)
            {
                if(record.pos.x >= -side - sideGap)
                {
                    record.prob = 1.0f - (side - record.pos.x) / sideGap;
                }
                else
                {
                    record.prob = 0.0f;
                }
            }
        }
    }

    private static void ApplyRightBound(IEnumerable<GrowRecord> records, float side, float sideGap)
    {
        foreach(GrowRecord record in records)
        {
            if(record.pos.x >= side)
            {
                if(record.pos.x <= side + sideGap)
                {
                    record.prob = 1.0f - (record.pos.x - side) / sideGap;
                }
                else
                {
                    record.prob = 0.0f;
                }
            }
        }
    }

    private void ApplyDirectionUp(IEnumerable<GrowRecord> records)
    {
        foreach(GrowRecord record in records)
        {
            if(record.dir.y < 0)
            {
                record.prob = 0.0f;
            }
        }
    }

    private void DebugDrawBounds()
    {
        Debug.DrawLine(new Vector2(Side, 0.0f), new Vector2(Side, 2.0f));
        Debug.DrawLine(new Vector2(Side+SideGap, 0.0f), new Vector2(Side+SideGap, 2.0f));
        
        Debug.DrawLine(new Vector2(-Side, 0.0f), new Vector2(-Side, 2.0f));
        Debug.DrawLine(new Vector2(-Side-SideGap, 0.0f), new Vector2(-Side-SideGap, 2.0f));
    }

    private static GrowRecord GetBestDirectionIndex(List<GrowRecord> records, Vector2 v)
    {
        GrowRecord bestRecord = null;
        float dot_max = 0;

        foreach(GrowRecord record in records)
        {
            float dot = Vector2.Dot(record.dir, v);
            if((bestRecord == null) || (dot > dot_max))
            {
                bestRecord = record;
                dot_max = dot;
            }
        }

        return bestRecord;
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region types

    private class GrowRecord
    {
        public Vector2 dir;
        public Vector2 pos;
        public float prob;
    }

    #endregion
    
    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region const

    private const int LOOKING_STEPS = 100;

    #endregion
}
