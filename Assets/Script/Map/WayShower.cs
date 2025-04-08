using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayShower : SingletonMonoBehaviour<WayShower>
{        
    [Header("MapOrigin")]
    [SerializeField] private GridCoordinate MapOrigin;

    [Header("Controller_Bool")]
    [SerializeField] private bool WayShowing=false;
    [SerializeField] private bool EndingInput=false;
    [SerializeField] private bool BeginShowing=false;

    [SerializeField] private bool SavePrincess=false;

    [Header("Audio Source")]
    [SerializeField] private AudioSource audioSource;

    [Header("AudioClips")]
    [SerializeField] private AudioClip EnterClip;
    [SerializeField] private AudioClip BeginClip;
    
    [Space(10)]
    [Range(0.1f,2f)]
    [SerializeField] private float stepWaitSeconds;

    [SerializeField] private MapManager mapManager;

    [SerializeField] GridCoordinate[] direct=new GridCoordinate[4];


    //Will Get Below from external Class
    [SerializeField] private List<GridCoordinate> JeeseCoordinateList;

    private Dictionary<Vector3Int,tileType> tileTypeDictionaryByCoordinate;
    // private Dictionary<Vector3Int,bool> tileFlagByCoordinate;

    [Header("Algorithm")]
    [SerializeField] AStar aStar;
    private Stack<Vector2Int> shortestWayStack;

    private void OnEnable() 
    {
        EventHandler.AfterEnterMapEvent+=FiringWayShower;
    }

    private void OnDisable() 
    {
        EventHandler.AfterEnterMapEvent-=FiringWayShower;
    }

    private void FiringWayShower() 
    {
        EndingInput=true;
    }

    protected override void Awake()
    {   
        base.Awake();
    }

    private void Start() 
    {
        mapManager=GameObject.FindObjectOfType<MapManager>();
        audioSource=this.GetComponent<AudioSource>();
    }
    private void Update() 
    {   
        if(WayShowing==false&&EndingInput==true)    
        {
            if(BeginShowing==false)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {   
                    PlayClip(audioSource,EnterClip);
                    BeginShowing=true;
                }
            }    
            else
            {   
                if(Input.GetKeyDown(KeyCode.Alpha1))
                {
                    WayShowing=true;
                    JeeseCoordinateList=MapManager.Instance.JesseGridCoordinateList;
                    tileTypeDictionaryByCoordinate=MapManager.Instance.tileTypeDictionaryByCoordinate;
                    EventHandler.CallBeforeWayShowerEvent();
                    ShowWay();
                }
                else if(Input.GetKeyDown(KeyCode.Alpha2))
                {
                    WayShowing=true;
                    JeeseCoordinateList=MapManager.Instance.JesseGridCoordinateList;
                    tileTypeDictionaryByCoordinate=MapManager.Instance.tileTypeDictionaryByCoordinate;
                    EventHandler.CallBeforeWayShowerEvent();
                    StartCoroutine(ShowWayByStep());
                }
                else if(Input.GetKeyDown(KeyCode.A))
                {
                    WayShowing=true;
                    tileTypeDictionaryByCoordinate=MapManager.Instance.tileTypeDictionaryByCoordinate;
                    EventHandler.CallBeforeWayShowerEvent();
                    shortestWayStack=new Stack<Vector2Int>();
                    aStar.tileTypeDictionary=tileTypeDictionaryByCoordinate;
                    aStar.BuildPath(out shortestWayStack);
                    setTileByWayStack();
                }
                else if(Input.GetKeyDown(KeyCode.S))
                {
                    WayShowing=true;
                    tileTypeDictionaryByCoordinate=MapManager.Instance.tileTypeDictionaryByCoordinate;
                    EventHandler.CallBeforeWayShowerEvent();
                    shortestWayStack=new Stack<Vector2Int>();
                    aStar.tileTypeDictionary=tileTypeDictionaryByCoordinate;
                    aStar.BuildPathByStep();
                }
            } 
        }
    }   

    private void ShowWay()
    {   
        foreach(GridCoordinate JeeseCoordinate in JeeseCoordinateList)
        {
            FindJeeseWay(JeeseCoordinate);
        }
        Debug.Log("EndFind");
    }

    private void FindJeeseWay(GridCoordinate jeeseCoordinate)
    {   
        if(SavePrincess)
            return;
        foreach(GridCoordinate dirCor in direct)
        {
            GridCoordinate newGridCoordinate=jeeseCoordinate+dirCor;
            
            if(JudgeGrid(newGridCoordinate))
            {   
                if(tileTypeDictionaryByCoordinate[newGridCoordinate]==tileType.princess)
                {
                    Debug.Log("FInd Princess!!!");
                    SavePrincess=true;
                    return;
                }
                ChangeDataAndUITile(jeeseCoordinate,tileType.usedWay);
                
                ChangeDataAndUITile(newGridCoordinate,tileType.jeese);
                FindJeeseWay(newGridCoordinate);
                if(SavePrincess)
                    return;
                ChangeDataAndUITile(jeeseCoordinate,tileType.way);
            }
        }
        return;
    }   

    private IEnumerator ShowWayByStep()
    {
        foreach(GridCoordinate JeeseCoordinate in JeeseCoordinateList)
        {
            yield return FindJeeseWayByStep(JeeseCoordinate);
        }
        Debug.Log("EndFind");
    }

    private IEnumerator FindJeeseWayByStep(GridCoordinate jeeseCoordinate)
    {
        if(SavePrincess)
            StopAllCoroutines();
        foreach(GridCoordinate dirCor in direct)
        {
            GridCoordinate newGridCoordinate=jeeseCoordinate+dirCor;
            
            if(JudgeGrid(newGridCoordinate))
            {   
                if(tileTypeDictionaryByCoordinate[newGridCoordinate]==tileType.princess)
                {
                    Debug.Log("FInd Princess!!!");
                    audioSource.clip=EnterClip;
                    audioSource.Play();
                    SavePrincess=true;
                    StopAllCoroutines();
                }
                yield return OneStep();
                ChangeDataAndUITile(jeeseCoordinate,tileType.usedWay);
                ChangeDataAndUITile(newGridCoordinate,tileType.jeese);

                yield return FindJeeseWayByStep(newGridCoordinate);
                
                if(SavePrincess)
                    StopAllCoroutines();
                yield return OneStep();
                ChangeDataAndUITile(jeeseCoordinate,tileType.way);
            }
        }
    }   

    private IEnumerator OneStep() 
    {
        yield return new WaitForSeconds(stepWaitSeconds);
        audioSource.Play();
    }

    private bool JudgeGrid(Vector3Int vector3Int) 
    {
        if(mapManager.canBePlaced(vector3Int))
        {
            if(tileTypeDictionaryByCoordinate[vector3Int]!=tileType.prohitbitedDefender)
            {   
                if(tileTypeDictionaryByCoordinate[vector3Int]!=tileType.usedWay)
                {
                    return true;
                }
            }
        }

        return false;
    }   


    private void setTileByWayStack()
    {   
        
        while(shortestWayStack.Count>0)
        {   
            Debug.Log(shortestWayStack.Count);
            Vector2Int wayPos=shortestWayStack.Pop();
            ChangeDataAndUITile(new Vector3Int(wayPos.x+MapOrigin.x,wayPos.y+MapOrigin.y,0),tileType.usedWay);
        }
    }

    public void ChangeDataAndUITile(Vector3Int vector3Int,tileType tileType)
    {   
        //Change Data
        tileTypeDictionaryByCoordinate[vector3Int]=tileType;

        //Change UI
        EventHandler.CallMapAdvancedEvent(vector3Int,tileType);
    }
    
    private void PlayClip(AudioSource audioSource,AudioClip audioClip)
    {
        audioSource.clip=audioClip;
        audioSource.Play();
    }

}
