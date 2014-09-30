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
        public Vector3 Pos;
        public float Size;

        public Vertex(Vector2 pos, float size)
        {
            Pos = pos;
            Size = size;
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
