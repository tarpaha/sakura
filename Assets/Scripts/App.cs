using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : MonoBehaviour
{
    #region MonoBehaviour

    private void Start()
    {
        CreateSakura();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CreateSakura();
        }
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region private functions

    private void CreateSakura()
    {
        if(_sakura != null)
        {
            Destroy(_sakura.gameObject);
        }

        _sakura = Sakura.Create(new Vector2(0.0f, -1.0f));
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region data

    private Sakura _sakura;

    #endregion
}
