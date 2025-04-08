using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{    
    [Header("MapOrigin")]
    [SerializeField] private GridCoordinate MapOrigin;

    public GridCoordinate startGridCoordinate;
    public GridCoordinate endCoordinate;
    public int Width;
    public int Height;
    public Stack<Vector2Int> ShortestWayStack;

    private AStarNode startNode;
    private AStarNode endNode;
    
    private AStarNode currentNode;  
    [SerializeField] private AStarNode[,] AStarNodeArray;
    [SerializeField] private List<AStarNode> openList=new List<AStarNode>();
    [SerializeField] private HashSet<AStarNode> closedList=new HashSet<AStarNode>();

    [Space(10)]
    [SerializeField] private bool pathFound=false;

    [Header("Get from WayShower")]
    public Dictionary<Vector3Int,tileType> tileTypeDictionary;

    [Header("IEnumerator Step Seconds")]
    [Range(0,2)]
    [SerializeField] private float AstarStepSeconds;
    
    public void BuildPath(out Stack<Vector2Int> wayStack) 
    {
        Initialize();
        UseDictionaryToPopulateArray();
        if(FindTheShortestWay())
        {       
            UpdateWayStack();
            Debug.Log("FindAStarWay");
        }
        wayStack=ShortestWayStack;
    }

    public void BuildPathByStep() 
    {
        Initialize();
        UseDictionaryToPopulateArray();
        StartCoroutine(BuildPathCoroutine());
    }

   

    private void Initialize()
    {   
        GetWidthAndHeightFromMapManager();
        ShortestWayStack=new Stack<Vector2Int>();
        AStarNodeArray=new AStarNode[Width,Height];
        for(int i=0;i<Width;i++) 
        {
            for(int j=0;j<Height;j++) 
            {
                AStarNodeArray[i,j]=new AStarNode(i,j);
            }
        }
        // startNode=AStarNodeArray[startGridCoordinate.x,startGridCoordinate.y];

        // endNode=AStarNodeArray[endCoordinate.x,endCoordinate.y];
    }   

    private void UseDictionaryToPopulateArray()
    {
        for(int i=0;i<Width;i++)
        {
            for(int j=0;j<Height;j++) 
            {
                switch(tileTypeDictionary[new Vector3Int(i+MapOrigin.x,j+MapOrigin.y,0)])
                {
                    case tileType.jeese:
                        startNode=AStarNodeArray[i,j];
                    break;

                    case tileType.princess:
                        endNode=AStarNodeArray[i,j];
                    break;

                    case tileType.prohitbitedDefender:
                        AStarNodeArray[i,j].isObstacle=true;
                    break;
                }
            }
        }
    }

    private void GetWidthAndHeightFromMapManager()
    {
        Width=MapManager.Instance.width;
        Height=MapManager.Instance.height;
    }

    private bool FindTheShortestWay()
    {   
        openList.Add(startNode);
        while(openList.Count>0) 
        {   
            Debug.Log("FindLoopOnce");
            openList.Sort();

            currentNode=openList[0];
            openList.RemoveAt(0);

            closedList.Add(currentNode);

            if(currentNode==endNode)
            {
                pathFound=true;
                return true;
            }

            EvaluateNeighbourNodes();
        }
        return pathFound;
    }

    private void EvaluateNeighbourNodes()
    {
        for(int i=-1;i<=1;i++) 
        {
            for(int j=-1;j<=1;j++) 
            {
                if(i==0&&j==0) 
                    continue;
                if(GetValidAStarNode(currentNode.gridX+i,currentNode.gridY+j,out AStarNode validAstarNode))
                {
                    if(validAstarNode!=null) 
                    {
                        int newCost=currentNode.toStartCost+GetDistance(currentNode,validAstarNode);

                        bool isNodeInOpenList=openList.Contains(validAstarNode);

                        if(newCost<validAstarNode.toStartCost||!isNodeInOpenList)
                        {
                            validAstarNode.toStartCost=newCost;

                            validAstarNode.toEndCost=GetDistance(validAstarNode,endNode);

                            validAstarNode.parentNode=currentNode;

                            if(!isNodeInOpenList)
                            {
                                openList.Add(validAstarNode);
                            }
                        }   
                    }
                }
            }
        }
    }   

     private IEnumerator BuildPathCoroutine()
    {
        openList.Add(startNode);
        while(openList.Count>0)
        {
            openList.Sort();

            currentNode=openList[0];

            openList.RemoveAt(0);

            WayShower.Instance.ChangeDataAndUITile(new Vector3Int(currentNode.gridX+MapOrigin.x,currentNode.gridY+MapOrigin.y,0),tileType.astarTile);
            closedList.Add(currentNode);

            if(currentNode==endNode)
            {
                pathFound=true;
                break;
            }
            yield return new WaitForSeconds(AstarStepSeconds);
            EvaluateNeighbourNodesWithTileChange();
        }
    }

    private void EvaluateNeighbourNodesWithTileChange()
    {
        for(int i=-1;i<=1;i++) 
        {
            for(int j=-1;j<=1;j++) 
            {
                if(i==0&&j==0) 
                    continue;
                if(GetValidAStarNode(currentNode.gridX+i,currentNode.gridY+j,out AStarNode validAstarNode))
                {
                    if(validAstarNode!=null) 
                    {
                        int newCost=currentNode.toStartCost+GetDistance(currentNode,validAstarNode);

                        bool isNodeInOpenList=openList.Contains(validAstarNode);

                        if(newCost<validAstarNode.toStartCost||!isNodeInOpenList)
                        {
                            validAstarNode.toStartCost=newCost;

                            validAstarNode.toEndCost=GetDistance(validAstarNode,endNode);

                            validAstarNode.parentNode=currentNode;

                            if(!isNodeInOpenList)
                            {
                                openList.Add(validAstarNode);
                            }
                            WayShower.Instance.ChangeDataAndUITile(new Vector3Int(validAstarNode.gridX+MapOrigin.x,validAstarNode.gridY+MapOrigin.y,0),tileType.usedWay);
                        }   
                    }
                }
            }
        }
    }

    private bool GetValidAStarNode(int posX,int posY,out AStarNode validAstarNode)
    {   
        if(posX>=0&&posX<Width&&posY>=0&&posY<Height)
        {   
            validAstarNode=AStarNodeArray[posX,posY];
            if(!closedList.Contains(validAstarNode))
            {   
                if(validAstarNode.isObstacle==false)
                {
                    return true;
                }
            }
        }
        validAstarNode=null;
        return false;
    }

    private int GetDistance(AStarNode node1,AStarNode node2)
    {
        int dstX=Mathf.Abs(node1.gridX-node2.gridX);
        int dstY=Mathf.Abs(node1.gridY-node2.gridY);

        if(dstX<dstY) 
        {
            return 14*dstX+10*(dstY-dstX);
        }
        else
        {
            return 14*dstY+10*(dstX-dstY);
        }
    }   

    private void UpdateWayStack()
    {
        if(pathFound)
        {   
            AStarNode lastnode=endNode;

            while(lastnode!=null)
            {
                ShortestWayStack.Push(new Vector2Int(lastnode.gridX,lastnode.gridY));

                lastnode=lastnode.parentNode;
            }
        }
    }

}
