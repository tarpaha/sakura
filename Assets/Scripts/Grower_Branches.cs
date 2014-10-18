using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Grower_Branches : Grower
{
    #region exposed

	public int Seed;
    public Grower_Adaptive TrunkGrower;
	public Grower_Adaptive BranchGrower;
	public int BranchesCount;
	public float BranchWidthCoeff;
	public float BranchLengthCoeff;

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region Grower

    public override TreeData Grow(Vector2 pos, Vector2 dir)
    {
		TrunkGrower.Seed = Seed;
        TreeData tree = TrunkGrower.Grow(pos, dir);

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
        TreeData.Vertex[] branchesRoots = GetBranchesRoots(trunk, BranchesCount);
        for(int i = 0; i < BranchesCount; i++)
        {
			BranchGrower.Seed = Seed + i;
			BranchGrower.L = (1.0f - branchesRoots[i].T) * TrunkGrower.L * BranchLengthCoeff;
			BranchGrower.Width01 = BranchWidthCoeff * branchesRoots[i].Width;

			AddBranch(tree, BranchGrower, branchesRoots[i], Random.Range(0, 2) == 0);
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
                        current_v.Width,
						current_v.T);
                    result.Add(branchRoot);
                }
            }
            prev_v = current_v;
        }

        return result.ToArray();
    }

    private static void AddBranch(TreeData tree, Grower_Adaptive branchGrower, TreeData.Vertex branchRoot, bool left)
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
