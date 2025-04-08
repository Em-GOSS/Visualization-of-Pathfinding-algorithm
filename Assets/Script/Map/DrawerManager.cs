using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerManager : SingletonMonoBehaviour<DrawerManager>
{   
    [Header("Controller_Bool")]
    [SerializeField] private bool ChangeTypeAccess=false;

    [SerializeField] private DrawerMode drawerMode=DrawerMode.ProhitbitedDrawer;

    private DrawerMode maxDrawerMode=0;

    private void OnEnable() 
    {
        EventHandler.BeforeEnterMapEvent+=FiringDrawerManager;
    } 

    private void Ondisable() 
    {
        EventHandler.BeforeEnterMapEvent-=FiringDrawerManager;
    }

    private void FiringDrawerManager()
    {
        ChangeTypeAccess=true;
    }

    protected override void Awake() 
    {   
        maxDrawerMode=DrawerMode.count-1;
        ChangeTypeAccess=false;
        drawerMode=maxDrawerMode;
        base.Awake();
    }   

    private void Update() 
    {   
        if(ChangeTypeAccess)
            ProcessInput();
    }

    private void ProcessInput() 
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {   
            if(drawerMode==0)
                drawerMode=maxDrawerMode;
            else
                drawerMode-=1;
            EventHandler.CallDrawerTypeChangeEvent(getDrawerTileType());
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {      
            if(drawerMode==maxDrawerMode)
                drawerMode=0;
            else
                drawerMode+=1;
            EventHandler.CallDrawerTypeChangeEvent(getDrawerTileType());
        }
    }

    public tileType getDrawerTileType()
    {
        switch(drawerMode)
        {
            case DrawerMode.WayDrawer:
                return tileType.way;
            case DrawerMode.ProhitbitedDrawer:
                return tileType.prohitbitedDefender;
            case DrawerMode.HeroDrawer:
                return tileType.jeese;
            case DrawerMode.PrincessDrawer:
                return tileType.princess;
            default:
                return tileType.count;  
        }
    }

}
