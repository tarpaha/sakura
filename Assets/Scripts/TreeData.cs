using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreeData
{
    public IEnumerable<Branch> Branches { get { return _branches; } }

    public void AddBranch(Branch branch)
    {
        _branches.Add(branch);
    }

    public class Vertex
    {
        public Vector2 Pos;
        public Vector2 Dir;
        public float Width;

        public Vertex(Vector2 pos, Vector2 dir, float width)
        {
            Pos = pos;
            Dir = dir;
            Width = width;
        }
    }

    public class Branch
    {
        public IEnumerable<Vertex> Vertices { get { return _vertices; } }

        public void AddVertex(Vertex vertex)
        {
            _vertices.Add(vertex);
        }

        public List<Vertex> _vertices = new List<Vertex>();
    }

    private List<Branch> _branches = new List<Branch>();
}
