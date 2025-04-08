using System;

public class AStarNode:IComparable<AStarNode>
{   
    public int gridX;
    public int gridY;
    public int toStartCost;
    public int toEndCost;

    public bool isObstacle;
    public AStarNode parentNode;

    public int FCost{
        get{
            return toStartCost+toEndCost;
        }
    }

    public AStarNode(int posX,int posY)
    {
        gridX=posX;
        gridY=posY;
        isObstacle=false;
    }

    public int CompareTo(AStarNode toCompareNode)
    {
        int CompareToResult=FCost.CompareTo(toCompareNode.FCost);
        if(CompareToResult==0) 
        {
            CompareToResult=toEndCost.CompareTo(toCompareNode.toEndCost);
        }
        return CompareToResult;
    }
}
