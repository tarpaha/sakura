using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Grower : MonoBehaviour
{
    #region public

    public abstract TreeData Grow(Vector2 pos, Vector2 dir);

    #endregion
}
