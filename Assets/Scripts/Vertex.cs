using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vertex : MonoBehaviour
{
    #region creation

    public static Vertex Create(Transform parent, Vector2 position, float size)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Quad);
        go.name = "Vertex";
        go.transform.parent = parent;
        go.transform.position = position;
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = size * Vector3.one;
        return go.AddComponent<Vertex>();
    }

    #endregion
}
