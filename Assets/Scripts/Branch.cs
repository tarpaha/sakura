using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Branch : MonoBehaviour
{
    #region creation

    public static Branch Create(Transform parent, Vector2 position, Vector2 direction)
    {
        GameObject go = new GameObject("Branch");
        Branch branch = go.AddComponent<Branch>();
        branch.transform.parent = parent;
        branch.transform.position = position;
        branch._direction = direction;
        return branch;
    } 

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region MonoBehaviour

    private void Start()
    {
        StartCoroutine(Grow());
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////
    
    #region private functions
    
    private IEnumerator Grow()
    {
        float L = 2.0f;

        Vector2 dir = _direction;
        Vector2 pos = transform.position;

        float l = 0.0f;
        float dl = 0.01f;

        float size = 0.05f;
        float ds = -0.01f;

        while(l <= L)
        {
            AddVertex(pos, size);

            dir = (dir + 0.15f * Random.insideUnitCircle).normalized;
            l += dl;

            pos += dir * dl;
            size += dl * ds;
        }

        yield return null;
    }

    private void AddVertex(Vector2 position, float size)
    {
        Vertex vertex = Vertex.Create(transform, position, size);
        _vertices.Add(vertex);
    }
    
    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////
    
    #region data
    
    private Vector2 _direction;
    private List<Vertex> _vertices = new List<Vertex>();

    #endregion
}
