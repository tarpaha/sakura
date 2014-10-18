using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Grower_Branches : Grower
{
    #region exposed

    public Grower Trunk;
    public Grower[] Branches;

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region Grower

    public override TreeData Grow(Vector2 pos, Vector2 dir)
    {
        TreeData tree = Trunk.Grow(pos, dir);
        AddBranches(tree);
        return tree;
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////
     
    #region private functions

    private void AddBranches(TreeData tree)
    {
        TreeData.Branch trunk = tree.Branches.First();
        TreeData.Vertex[] branchesRoots = GetBranchesRoots(trunk, Branches.Length);
        for(int i = 0; i < Branches.Length; i++)
        {
            AddBranch(tree, Branches[i], branchesRoots[i], (i % 2) == 0);
        }
    }

    private static TreeData.Vertex[] GetBranchesRoots(TreeData.Branch branch, int branchesCount)
    {
        float trunkLength = GetBranchLength(branch);
        float trunkPartLength = trunkLength / (branchesCount + 1);

        List<TreeData.Vertex> result = new List<TreeData.Vertex>();

        float length = 0;
        TreeData.Vertex prev_v = null;
        foreach(TreeData.Vertex current_v in branch.Vertices)
        {
            if(prev_v != null)
            {
                length += Vector2.Distance(prev_v.Pos, current_v.Pos);
                if(length >= trunkPartLength)
                {
                    length -= trunkPartLength;
                    TreeData.Vertex branchRoot = new TreeData.Vertex(
                        current_v.Pos,
                        current_v.Dir,
                        current_v.Width);
                    result.Add(branchRoot);
                }
            }
            prev_v = current_v;
        }

        return result.ToArray();
    }

    private static void AddBranch(TreeData tree, Grower branchGrower, TreeData.Vertex branchRoot, bool left)
    {
        TreeData branch = branchGrower.Grow(
            branchRoot.Pos,
            Quaternion.AngleAxis(left ? 90.0f : -90.0f, Vector3.forward) * branchRoot.Dir);
        foreach(TreeData.Branch subBranch in branch.Branches)
        {
            tree.AddBranch(subBranch);
        }
    }

    private static float GetBranchLength(TreeData.Branch branch)
    {
        float length = 0;
        TreeData.Vertex prev_v = null;
        foreach(TreeData.Vertex current_v in branch.Vertices)
        {
            if(prev_v != null)
            {
                length += Vector2.Distance(prev_v.Pos, current_v.Pos);
            }
            prev_v = current_v;
        }
        return length;
    }

    #endregion
}