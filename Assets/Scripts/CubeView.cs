using System;
using UnityEngine;

public class CubeView : MonoBehaviour
{
    public Action<CubeView> OnCubeClick;

    public void InteractWithCube()
    {
        OnCubeClick?.Invoke(this);
    }
}
