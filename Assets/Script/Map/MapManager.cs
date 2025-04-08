using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager:SingletonMonoBehaviour<MapManager>
{       
    [SerializeField]
    private int MaxWidth;

    [SerializeField]
    private int MaxHeight;

    public int width;
    public int height;

    [Space(10)]
    [Header("Jeese&&Princess CoordinateList")]
    [SerializeField] public List<GridCoordinate> JesseGridCoordinateList;

    [SerializeField] public List<GridCoordinate> PrincessGridCoordinateList;

    [Space(10)]
    //used left bottom as the basePoint;
    [SerializeField] private GridCoordinate MapOrign; 

    [SerializeField] private GridCoordinate MapSymmetryOrgin;

    [Space(10)]
    [SerializeField] private Grid grid;
    [SerializeField] private Tilemap tilemap;

    [Space(10)]
    [SerializeField] private Camera mainCamera;

    [Space(10)]
    [Header("Tile Should Use")] 

    [SerializeField] private List<Tile> tileStoreList=new List<Tile>();

    public Dictionary<Vector3Int,tileType> tileTypeDictionaryByCoordinate=new Dictionary<Vector3Int, tileType>();
    private GridProperties[,] mapGridPropertiesArray=new GridProperties[30,16];

    Dictionary<tileType,Tile> tileDictionary=new Dictionary<tileType, Tile>();


    private void OnEnable() 
    {
        EventHandler.BeforeEnterMapEvent+=BeforeEnterMap;
        EventHandler.MapAdvancedEvent+=MapAdvanced;
    }

    private void OnDisable() 
    {
        EventHandler.BeforeEnterMapEvent-=BeforeEnterMap;
        EventHandler.MapAdvancedEvent-=MapAdvanced;
    }

    private void BeforeEnterMap()
    {
        DrawNewEmptyMap();
    }

    protected override void Awake()
    {   
        mainCamera=Camera.main;
        tileTypeDictionaryByCoordinate=new Dictionary<Vector3Int, tileType>();
        CreateMapSymmtryOrgin();
        InitTileDictionary();
        base.Awake();
    }
    
    private void CreateMapSymmtryOrgin()
    {
        MapSymmetryOrgin.x=MapOrign.x+MaxWidth-1;
        MapSymmetryOrgin.y=MapOrign.y+MaxHeight-1;
    }

    private void InitTileDictionary() 
    {
        for(int i=0;i<(int)tileType.count;i++)
        {
            tileDictionary.Add((tileType)i,tileStoreList[i]);
        }
    }


    public void ProcessInput()
    {
        if(width==0||height==0||width>MaxWidth||height>MaxHeight)
        {   
            width=MaxWidth;
            height=MaxHeight;
        }
    }   

    private void MapAdvanced(Vector3Int vector3Int,tileType tileType) 
    {   
        PopulateList(vector3Int,tileType);
        // PopulateGridPropertiesArray(vector3Int,tileType); 
        SetTileInGrid(vector3Int,tileType);
    }

    private void PopulateList(Vector3Int vector3Int,tileType newtileType) 
    {      
        tileType orginTileType=tileTypeDictionaryByCoordinate[vector3Int];
        
        if(orginTileType==newtileType)
            return;

        switch(newtileType) 
        {
            case tileType.jeese:
                JesseGridCoordinateList.Add(new GridCoordinate(vector3Int.x,vector3Int.y));
                
            break;

            case tileType.princess:
                PrincessGridCoordinateList.Add(new GridCoordinate(vector3Int.x,vector3Int.y));
            break;

            default:
            break;
        }

        if(orginTileType==tileType.jeese)
        {   
            RemoveValueInGridCoordinateList(JesseGridCoordinateList,vector3Int);
        }
        else if(orginTileType==tileType.princess)
        {   
            RemoveValueInGridCoordinateList(PrincessGridCoordinateList,vector3Int);
        }       

        tileTypeDictionaryByCoordinate[vector3Int]=newtileType;
    }   

    private void RemoveValueInGridCoordinateList(List<GridCoordinate> list,Vector3Int removeValue)
    {
        foreach(GridCoordinate gridCoordinate in list) 
        {
            if(gridCoordinate.x==removeValue.x&&gridCoordinate.y==removeValue.y)
            {
                list.Remove(gridCoordinate);
                break;
            }
        }
    }

    private void PopulateGridPropertiesArray(Vector3Int vector3Int,tileType tileType)
    {
        int gridX=vector3Int.x-MapOrign.x;
        int gridY=vector3Int.y-MapOrign.y;

        mapGridPropertiesArray[gridX,gridY].tileType=tileType;
        // Debug.Log(tileType);
    }

    private void DrawNewEmptyMap()
    {      
        tileTypeDictionaryByCoordinate.Clear();
        for(int x=0;x<width;x++)
        {
            for(int y=0;y<height;y++)
            {   
                //Populate GridProperties[,]
                FillingGridPropertiesArray(x,y,tileType.way);

                //SetDictionary
                tileTypeDictionaryByCoordinate.Add(new Vector3Int(x+MapOrign.x,y+MapOrign.y,0),tileType.way);

                //Create new tile Base in the scene
                SetTileInGrid(new Vector3Int(x+MapOrign.x,y+MapOrign.y,0),tileType.way);    
            }
        }
    }

    private void FillingGridPropertiesArray(int x,int y,tileType tileType)
    {
        if(mapGridPropertiesArray[x,y]!=null)
        {
            mapGridPropertiesArray[x,y].tileType=tileType;
        }
        else
        {
            mapGridPropertiesArray[x,y]=new GridProperties(new GridCoordinate(x+MapOrign.x,y+MapOrign.y),tileType);
        }
    }
    
    private void SetTileInGrid(Vector3Int vector3Int,tileType tileType)
    {
        tilemap.SetTile(vector3Int,tileDictionary[tileType]);
        //SetDictionary
        tileTypeDictionaryByCoordinate[vector3Int]=tileType;
    }


    //Judge Methods used by other class

    public bool canBePlaced(Vector3Int placedPoint)
    {
        if(placedPoint.x>=MapOrign.x&&placedPoint.y>=MapOrign.y)
        {
            if(placedPoint.x<=MapSymmetryOrgin.x&&placedPoint.y<=MapSymmetryOrgin.y)
            {
                if(placedPoint.x<=MapOrign.x+width-1&&placedPoint.y<=MapOrign.y+height-1)
                {   
                    
                    return true;
                }
            }
        }
        
        return false;      
    }


    //ToDo: Remove 

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            PrintDictionary(width,height);
        }
    }
    private void PrintDictionary(int width,int height) 
    {   
        for(int i=0;i<width;i++)
        {
            for(int j=0;j<height;j++)
            {   
                int x=i+MapOrign.x;
                int y=j+MapOrign.y;
                Debug.Log(x+","+y);
                Debug.Log(tileTypeDictionaryByCoordinate[new Vector3Int(x,y,0)]);
            }
        }
    }
}
