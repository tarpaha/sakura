using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sakura : MonoBehaviour
{
    #region exposed

    public Grower Grower;
    public Drawer Drawer;

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region MonoBehaviour

    private void Start()
    {
        GrowAndDraw();
    }

    private void FixedUpdate()
    {
        //if(Input.GetMouseButtonDown(0))
        {
            GrowAndDraw();
        }
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region private functions

    private void GrowAndDraw()
    {
        _treeData = Grower.Grow();
        Drawer.Draw(_treeData);
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////

    #region data

    private TreeData _treeData;

    #endregion
}
