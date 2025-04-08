using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{   
    [SerializeField] private bool isCursorValid;
    [SerializeField] private Sprite gridSprite_abled;
    [SerializeField] private Sprite gridSprite_disabled;

    [Space(10)]
    [Header("Managers")]
    [SerializeField] private MapManager mapManager;
    [SerializeField] private DrawerManager drawerManager;

    [SerializeField] private RectTransform gridCursorRectTransform;

    [Space(10)]
    [SerializeField] private Canvas canvas; 

    [Space(10)]
    [SerializeField] private Camera mainCamera; 

    [Space(10)]
    [SerializeField] private Grid grid;

    [Space(10)]
    [SerializeField] private Image cursorImage;


    private void OnEnable() 
    {
        EventHandler.BeforeWayShowerEvent+=CloseGridCursor;
    }

    private void OnDisable() 
    {
        EventHandler.BeforeWayShowerEvent-=CloseGridCursor;
    }


    private void CloseGridCursor() 
    {
        setGridCursor(false,false);
    }

    private void Awake() 
    {
        this.gameObject.SetActive(false);
        mainCamera=Camera.main;
    }   

    private void Update()
    {   
        Vector3Int mouseGridPosition=GetMouseGridPosition();
        ProcessPosition(mouseGridPosition);
        ProcessCursorValidity(mouseGridPosition);
        ProcessClick(mouseGridPosition);
    }

    private void ProcessPosition(Vector3Int mouseGridPosition) 
    {
        gridCursorRectTransform.position=GetGridRectTransformPosition(mouseGridPosition);
    }  

    private void ProcessCursorValidity(Vector3Int mouseGridPosition)
    {
        if(isCursorValid!=MapManager.Instance.canBePlaced(mouseGridPosition))
        {
            setValidity(!isCursorValid);
        }
    }

    private void setValidity(bool validity)
    {   
        if(!validity)
            cursorImage.sprite=gridSprite_disabled;
        else
            cursorImage.sprite=gridSprite_abled;


        isCursorValid=validity;
    }

    private void ProcessClick(Vector3Int mouseGridPosition)
    {   
        //Left-Click
        if(Input.GetMouseButtonDown(0))
        {
            if(isCursorValid)
            {
                PlaceTargetTile(mouseGridPosition);
            }
        }
        //Right-Click
        else if(Input.GetMouseButtonDown(1))
        {
            if(isCursorValid)
            {   
                RemoveTargetTile(mouseGridPosition);
            }
        }
    }

    private void PlaceTargetTile(Vector3Int mouseGridPosition)
    {   
        tileType tileType=DrawerManager.Instance.getDrawerTileType();
        //Make targetTile to drawerModetype-tile
        EventHandler.CallMapAdvancedEvent(mouseGridPosition,tileType);
    }

    private void RemoveTargetTile(Vector3Int mouseGridPosition)
    {
        //Make targetTile to WayTile
        EventHandler.CallMapAdvancedEvent(mouseGridPosition,tileType.way);
    }

    private Vector3Int GetMouseGridPosition()
    {
        Vector3 mouseScreenPosition=Input.mousePosition;
        
        Vector3 mouseWorldPosition=mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        return grid.WorldToCell(mouseWorldPosition);
    } 

    private Vector2 GetGridRectTransformPosition(Vector3Int gridPosition) 
    {
        Vector3 gridWorldPosition=grid.CellToWorld(gridPosition);

        Vector3 gridScreenPosition=mainCamera.WorldToScreenPoint(gridWorldPosition);

        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition,gridCursorRectTransform,canvas);
    }


    public void setGridCursor(bool ifActive,bool ifAbled=true)
    {   
        this.gameObject.SetActive(ifActive);

        if(ifAbled) 
            cursorImage.sprite=gridSprite_abled;
        else
            cursorImage.sprite=gridSprite_disabled;
        
        isCursorValid=ifAbled;

    }
}
