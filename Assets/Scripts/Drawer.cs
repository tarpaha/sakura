using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Drawer : MonoBehaviour
{
    #region public

    public abstract void Draw(TreeData treeData);

    #endregion
}
