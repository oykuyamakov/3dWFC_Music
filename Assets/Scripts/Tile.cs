using System;
using Sirenix.OdinInspector;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public PointTypes LeftFits;
    public PointTypes RightFits;
    public PointTypes UpFits;
    public PointTypes DownFits;
    public PointTypes ForwardFits;
    public PointTypes BackFits;
    
    public PointTypes Self; 
    
    public bool OnGround;
    public bool IsCeiling;

    public bool CanFit(PointTypes constraints)
    {
        return (Self & constraints) != 0 || constraints == PointTypes.All;
    }
    
}

[EnumToggleButtons][Flags]
public enum PointTypes
{
    Empty = 1,
    All = 2,
    Top = 8,
    Bottom = 16,
    BridgeLR = 32,
}

