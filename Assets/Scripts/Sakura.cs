using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sakura : MonoBehaviour
{
    #region creation

    public static Sakura Create(Vector2 position)
    {
        GameObject go = new GameObject("Sakura");
        Sakura sakura = go.AddComponent<Sakura>();
        sakura.transform.position = position;
        return sakura;
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region MonoBehaviour

    private void Start()
    {
        Grow();
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region private functions

    private void Grow()
    {
        Branch.Create(transform, transform.position, Vector2.up);
    }

    #endregion
}
