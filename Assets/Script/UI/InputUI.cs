using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField widthInput;
    [SerializeField] private TMP_InputField heightInput;

    [Space(10)]
    [Header("MapManager")]
    [SerializeField] private MapManager mapManager;


    private void OnEnable()
    {
        EventHandler.AccessInputEvent+=TransformInputToManager;
    }
    
    private void OnDisable() 
    {
        EventHandler.AccessInputEvent-=TransformInputToManager;
    }

    private void TransformInputToManager()
    {   
        MapManager.Instance.width=stringToInt(widthInput.text);
        MapManager.Instance.height=stringToInt(heightInput.text);       
        Debug.Log("width "+MapManager.Instance.width+"  Height "+MapManager.Instance.height);
        MapManager.Instance.ProcessInput();
        Debug.Log( "This is Process Input  "+"width "+MapManager.Instance.width+"  Height "+MapManager.Instance.height);
    }

    private int stringToInt(string str) 
    {
        int i=str.Length-1;
        int multiple=1;
        int finalInt=0;
        while(i>=0)
        {   
            if(str[i]<='0'&&str[i]>='9')
            {
                Debug.Log("Error Input");
                return 0;
            }
            finalInt+=multiple*(str[i]-'0');
            multiple*=10;
            i--;
        }
        
        return finalInt;
    }

}
