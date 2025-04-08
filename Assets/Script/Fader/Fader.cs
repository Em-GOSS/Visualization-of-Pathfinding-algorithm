using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Fader:SingletonMonoBehaviour<Fader>
{
    public IEnumerator Fade(TextMeshProUGUI fadeTextMeshProUGUI,float fadeSeconds,float targetAlpha)
    {
        Color fadeTextMeshProUGUIColor=fadeTextMeshProUGUI.color;
        float fadeSpeed=Mathf.Abs(targetAlpha-fadeTextMeshProUGUIColor.a)/fadeSeconds;

        while(!Mathf.Approximately(fadeTextMeshProUGUIColor.a,targetAlpha))
        {
            fadeTextMeshProUGUIColor.a=Mathf.MoveTowards(fadeTextMeshProUGUIColor.a,targetAlpha,fadeSpeed*Time.deltaTime);
            fadeTextMeshProUGUI.color=fadeTextMeshProUGUIColor;
            yield return null;
        }
    }

    public IEnumerator Fade(Image fadeImage,float fadeSeconds,float targetAlpha)
    {
        Color fadeImageColor=fadeImage.color;
        float fadeSpeed=Mathf.Abs(targetAlpha-fadeImageColor.a)/fadeSeconds;

        while(!Mathf.Approximately(fadeImageColor.a,targetAlpha))
        {
            fadeImageColor.a=Mathf.MoveTowards(fadeImageColor.a,targetAlpha,fadeSpeed*Time.deltaTime);
            fadeImage.color=fadeImageColor;
            yield return null;
        }
    }

    public IEnumerator Fade(CanvasGroup fadeCanvasGroup,float fadeSeconds,float targetAlpha)
    {
        float fadeSpeed=Mathf.Abs(targetAlpha-fadeCanvasGroup.alpha)/fadeSeconds;

        while(!Mathf.Approximately(fadeCanvasGroup.alpha,targetAlpha))
        {
            fadeCanvasGroup.alpha=Mathf.MoveTowards(fadeCanvasGroup.alpha,targetAlpha,fadeSpeed*Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator FadeMaterialFloat(Material fadeMaterial,string toFadeProperty,float fadeSeconds,float targetValue)
    {
        int propertyToID=Shader.PropertyToID(toFadeProperty);

        float currentPropertyValue=fadeMaterial.GetFloat(propertyToID);
        float fadeSpeed=Mathf.Abs(targetValue-currentPropertyValue)/fadeSeconds;

        while(!Mathf.Approximately(currentPropertyValue,targetValue))
        {
            currentPropertyValue=Mathf.MoveTowards(currentPropertyValue,targetValue,fadeSpeed*Time.deltaTime);
            fadeMaterial.SetFloat(propertyToID,currentPropertyValue);
            yield return null;
        }
    }

    public IEnumerator FadeMaterialColor(Material fadeMaterial,string toFadeProperty,float fadeSeconds,Color targetColor)
    {
        int propertyToID=Shader.PropertyToID(toFadeProperty);
        
        Color currentPropertyColor=fadeMaterial.GetColor(toFadeProperty);

        Color ColorDeltaAbs=currentPropertyColor;
        
        ColorDeltaAbs.a=Mathf.Abs(targetColor.a-ColorDeltaAbs.a);
        ColorDeltaAbs.r=Mathf.Abs(targetColor.r-ColorDeltaAbs.r);
        ColorDeltaAbs.g=Mathf.Abs(targetColor.g-ColorDeltaAbs.g);
        ColorDeltaAbs.b=Mathf.Abs(targetColor.b-ColorDeltaAbs.b);

        Color colorFadeSpeed=(ColorDeltaAbs)/fadeSeconds;

        while(!Mathf.Approximately(currentPropertyColor.a,targetColor.a))
        {
            currentPropertyColor.a=Mathf.MoveTowards(currentPropertyColor.a,targetColor.a,colorFadeSpeed.a);
            currentPropertyColor.r=Mathf.MoveTowards(currentPropertyColor.r,targetColor.r,colorFadeSpeed.r);
            currentPropertyColor.g=Mathf.MoveTowards(currentPropertyColor.g,targetColor.g,colorFadeSpeed.g);
            currentPropertyColor.b=Mathf.MoveTowards(currentPropertyColor.b,targetColor.b,colorFadeSpeed.b);

            fadeMaterial.SetColor(propertyToID,currentPropertyColor);
            yield return null;
        }

    }

    public IEnumerator FadeFont(TextMeshProUGUI fadeTextMeshProUGUI,float fadeSeconds,float fontTargetSize)
    {
        float currentTextFontSize=fadeTextMeshProUGUI.fontSize;

        float fadeSpeed=Mathf.Abs(fontTargetSize-currentTextFontSize)/fadeSeconds;

        while(!Mathf.Approximately(currentTextFontSize,fontTargetSize))
        {   
            currentTextFontSize=Mathf.MoveTowards(currentTextFontSize,fontTargetSize,fadeSpeed*Time.deltaTime);
            fadeTextMeshProUGUI.fontSize=currentTextFontSize;
            yield return null;
        }
    }
}
