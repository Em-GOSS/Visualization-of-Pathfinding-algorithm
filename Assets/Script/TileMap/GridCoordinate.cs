using UnityEngine;

[System.Serializable]
public class GridCoordinate
{
    public int x;
    public int y;

    public GridCoordinate(int p1,int p2)
    {
        x=p1;
        y=p2;
    }

    public static GridCoordinate operator+(GridCoordinate gridCoordinate1,GridCoordinate gridCoordinate2)
        =>new GridCoordinate(gridCoordinate1.x+gridCoordinate2.x,gridCoordinate1.y+gridCoordinate2.y);

    public static implicit operator Vector3Int(GridCoordinate girdCoordinate)
    {
        return new Vector3Int(girdCoordinate.x,girdCoordinate.y,0);
    }
}

 