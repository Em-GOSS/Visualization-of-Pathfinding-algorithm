using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class ClickedTextMeshProUGUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{   
    [SerializeField] private Fader Fader;

    [SerializeField] private List<MaterialPropertiesToPopulate> propertyPopulateList=new List<MaterialPropertiesToPopulate>();
    [SerializeField] private List<MaterialPropertiesToPopulate> propertyOrginList=new List<MaterialPropertiesToPopulate>();
    [SerializeField] private Material textMeterial;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;




    //Coroutines 
    private Coroutine textPopulateCoroutine;
    private Coroutine textResetCoroutine;

    public void OnDisable() 
    {
        ResetTextProperty();
    }

    private void Awake() 
    {
        textMeterial=this.GetComponent<TextMeshProUGUI>().materialForRendering; 

        InitializeResetList();
    }
    


    private void InitializeResetList()
    {   
        for(int i=0;i<propertyPopulateList.Count;i++)
        {
            propertyOrginList.Add(new MaterialPropertiesToPopulate(propertyPopulateList[i]));
        }
        
        for(int i=0;i<propertyOrginList.Count;i++)
        {
            MaterialPropertiesToPopulate OrginProperty=propertyOrginList[i];
            MaterialPropertyValueType valueType=propertyOrginList[i].valueType;

            switch(valueType)
            {
                case MaterialPropertyValueType._Color:
                    propertyOrginList[i].colorPropertyPopulateValue=GetCurrentPropertyColorValue(propertyOrginList[i].propertyName);
                break;

                case MaterialPropertyValueType._Float:
                    propertyOrginList[i].floatPropertyPopulateValue=GetCurrentPropertyFloatValue(propertyOrginList[i].propertyName);
                break;

                case MaterialPropertyValueType._Font:
                    propertyOrginList[i].fontTargetSize=GetCurrentPropertyFontSize();
                break;
                default:
                break;
            }
            
        }
    }

    
    public void OnPointerClick(PointerEventData eventData)
    {
        ResetTextProperty();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {   
        // Debug.Log("OnPointerEnter");
        StopAllCoroutines();
        TextPopulate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {   
        // Debug.Log("OnPointerExit");
        StopAllCoroutines();
        TextReset();
    }

    public void TextPopulate()
    {
        for(int i=0;i<propertyPopulateList.Count;i++)
        {
            FadeTextProperties(propertyPopulateList[i]);
        }
    }

    public void TextReset()
    {
        for(int i=0;i<propertyOrginList.Count;i++)
        {
            FadeTextProperties(propertyOrginList[i]);
        }
    }

    private void FadeTextProperties(MaterialPropertiesToPopulate populatedProperty) 
    {
        switch(populatedProperty.valueType)
        {
            case MaterialPropertyValueType._Color:
                StartCoroutine(Fader.FadeMaterialColor(textMeterial,populatedProperty.propertyName,populatedProperty.populateSeconds,populatedProperty.colorPropertyPopulateValue));
            break;

            case MaterialPropertyValueType._Float:
                StartCoroutine(Fader.FadeMaterialFloat(textMeterial,populatedProperty.propertyName,populatedProperty.populateSeconds,populatedProperty.floatPropertyPopulateValue));
            break;

            case MaterialPropertyValueType._Font:
                StartCoroutine(Fader.FadeFont(textMeshProUGUI,populatedProperty.populateSeconds,populatedProperty.fontTargetSize));
            break;

            default:
            break;
        }
    }


    public Color GetCurrentPropertyColorValue(string propertyName)
    {
        int propertyToID=Shader.PropertyToID(propertyName);
        return textMeterial.GetColor(propertyToID);
    }

    public float GetCurrentPropertyFloatValue(string propertyName)
    {
        int propertyToID=Shader.PropertyToID(propertyName);
        return textMeterial.GetFloat(propertyToID);
    }
    
    public float GetCurrentPropertyFontSize()
    {
        float fontSize=textMeshProUGUI.fontSize;
        return fontSize;
    }



    //Reset data
    public void ResetTextProperty()
    {
        for(int i=0;i<propertyOrginList.Count;i++)
        {
            MaterialPropertiesToPopulate propertyToReset=propertyOrginList[i];

            switch(propertyToReset.valueType)
            {
                case MaterialPropertyValueType._Color:
                    SetCurrentPropertyColorValue(propertyToReset.propertyName,propertyToReset.colorPropertyPopulateValue);
                break;

                case MaterialPropertyValueType._Float:
                    SetCurrentPropertyFloatValue(propertyToReset.propertyName,propertyToReset.floatPropertyPopulateValue);
                break;  

                case MaterialPropertyValueType._Font:
                    SetCurrentPropertyFontSize(propertyToReset.fontTargetSize);
                break;

                default:
                break;
            }
        }
    } 

    public void SetCurrentPropertyFloatValue(string propertyName,float setFloat)
    {
        int propertyToID=Shader.PropertyToID(propertyName);
        textMeterial.SetFloat(propertyName,setFloat);
    }

    public void SetCurrentPropertyColorValue(string propertyName,Color setColor)
    {
        int propertyToID=Shader.PropertyToID(propertyName);
        textMeterial.SetColor(propertyName,setColor);
    }

    public void SetCurrentPropertyFontSize(float setFontSize)
    {
        textMeshProUGUI.fontSize=setFontSize;
    }

}
    

