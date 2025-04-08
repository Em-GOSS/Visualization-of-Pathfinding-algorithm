using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridProperties 
{
    [SerializeField] public GridCoordinate coordinate;

    [SerializeField] public tileType tileType;

    [SerializeField] public bool isUsed;
    
    public GridProperties(GridCoordinate coordinate,tileType tileType)
    {
        this.coordinate=coordinate;
        this.tileType=tileType;
        isUsed=false;
    }
}
