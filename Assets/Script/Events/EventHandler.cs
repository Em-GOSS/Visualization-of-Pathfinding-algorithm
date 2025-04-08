using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventHandler 
{
    public static event Action EnterUICloseEvent;

    public static void CallEnterUICloseEvent()
    {   
        if(EnterUICloseEvent!=null)
        {
            EnterUICloseEvent();
        }
        else
        {
            Debug.Log("This Event is null");
        }
    }

    public static event Action AccessInputEvent;

    public static void CallAccessInputEvent()
    {
        if(AccessInputEvent!=null)
        {
            AccessInputEvent();
        }
        else
        {
            Debug.Log("This Event is null");
        }
    }

    public static event Action BeforeEnterMapEvent;

    public static void CallBeforeEnterMapEvent()
    {
        if(BeforeEnterMapEvent!=null)
        {
            BeforeEnterMapEvent();
        }
        else
        {
            Debug.Log("This Event is null");
        }
    }

    public static event Action AfterEnterMapEvent;

    public static void CallAfterEnterMapEvent() 
    {
        if(AfterEnterMapEvent!=null) 
        {
            AfterEnterMapEvent();
        }
        else
        {
            Debug.Log("This Event is null");
        }
    }

    public static event Action<Vector3Int,tileType> MapAdvancedEvent;

    public static void CallMapAdvancedEvent(Vector3Int vector3Int,tileType tileType)
    {
        if(BeforeEnterMapEvent!=null)
        {
            MapAdvancedEvent(vector3Int,tileType);
        }
        else
        {
            Debug.Log("This Event is null");
        }
    }

    public static event Action BeforeWayShowerEvent;

    public static void CallBeforeWayShowerEvent() 
    {
        if(BeforeWayShowerEvent!=null)
        {
            BeforeWayShowerEvent();
        }
        else
        {
            Debug.Log("This Event is null");
        }
    }

    public static event Action<tileType> DrawerTypeChangeEvent;

    public static void CallDrawerTypeChangeEvent(tileType tileType)
    {
        if(DrawerTypeChangeEvent!=null)
        {
            DrawerTypeChangeEvent(tileType);
        }
        else
        {
            Debug.Log("This Event is null");
        }
    }
    
    public static event Action<GridCoordinate,GridCoordinate,int,int,int,int> AstarWayShowerBeginEvent;

    public static void CallAstarWayShowerBeginEvent(GridCoordinate startGridCoordinate,GridCoordinate endGridCoordinate,int Width,int Height,int originX,int originY)
    {
        if(AstarWayShowerBeginEvent!=null)
        {
            AstarWayShowerBeginEvent(startGridCoordinate,endGridCoordinate,Width,Height,originX,originY);
        }
        else
        {
            Debug.Log("This Event is null");
        }
    }

}
