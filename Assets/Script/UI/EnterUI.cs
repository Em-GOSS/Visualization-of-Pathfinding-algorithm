using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EnterUI : MonoBehaviour
{   
    [SerializeField] private CanvasGroup EnterUICanvasGroup;
    [SerializeField] private Image EnterBackGround;
    [SerializeField] private TextMeshProUGUI ProductorName;


    [Header("SecondsToSet")]
    [SerializeField] private float beforeFadeBeginSeconds=0f;
    [SerializeField] private float UIBackGroudFadeInSeconds=0f;
    [SerializeField] private float twoFadeInPadding=0f;
    [SerializeField] private float nameFadeInSeconds=0f;
    [SerializeField] private float beforeCloseEnterUISeconds=0f;
    [SerializeField] private float closeEnterUISecond=0f;
    [SerializeField] private float LightAngleFadeSeconds=0f;

    [Header("Text shakes")]
    [SerializeField] 
    private bool isShake;

    [Range(0,1)]
    [SerializeField] 
    private float shakeAmplitude=0f;

    [Range(0,1)]
    [SerializeField]
    private float shakeDuration=0f;

    [SerializeField] 
    private float shakeNum=0f;


    [Header("Audio")]
    [SerializeField] 
    private AudioSource EnterUIAudioSource;

    [SerializeField]
    private AudioClip subtitudeAudioClip;

    [Header("Font")]
    [SerializeField]
    private Material FontMaterial;

    
    [Range(0.0f, 6.2831853f)]
    [SerializeField]
    private float targetLightAngle;


    [Header("Fader")]
    [SerializeField] private Fader Fader;


    private void Start() 
    {   
        ResetMaterial(FontMaterial,"_LightAngle",0.0f); 
        StartCoroutine(UIEnterAndCallOut());
    }


    private IEnumerator UIEnterAndCallOut() 
    {   
        yield return new WaitForSeconds(beforeFadeBeginSeconds);

        yield return Fader.Instance.Fade(EnterBackGround,UIBackGroudFadeInSeconds,1f);

        yield return new WaitForSeconds(twoFadeInPadding);

        yield return Fader.Instance.Fade(ProductorName,nameFadeInSeconds,1f);

        if(isShake)
            yield return StartCoroutine(ShakeText());           
        StartCoroutine(UIOut());
        
    }

    private IEnumerator UIOut() 
    {   
        yield return new WaitForSeconds(beforeCloseEnterUISeconds);

        yield return Fader.Instance.Fade(EnterUICanvasGroup,closeEnterUISecond,0f);

        this.gameObject.SetActive(false);

        EventHandler.CallEnterUICloseEvent();
        //归位
        ResetMaterial(FontMaterial,"_LightAngle",0.0f); 
    }

    private IEnumerator ShakeText()
    {
        float shakeLowBound=1f-shakeAmplitude;

        yield return FadeLightAngle();
        EnterUIAudioSource.clip=subtitudeAudioClip;
        EnterUIAudioSource.Play();
        for(int count=0;count<shakeNum;count++)
        {
            yield return Fader.Instance.Fade(ProductorName,shakeDuration,shakeLowBound);

            yield return Fader.Instance.Fade(ProductorName,shakeDuration,1f);
        }
    }

    private IEnumerator FadeLightAngle()
    {      
        ResetMaterial(FontMaterial,"_LightAngle",0.0f);
        yield return Fader.Instance.FadeMaterialFloat(FontMaterial,"_LightAngle",LightAngleFadeSeconds,targetLightAngle);
    }

    private void ResetMaterial(Material materialToReset,string propertyName,float value) 
    {
        int ResetID=Shader.PropertyToID(propertyName);
        materialToReset.SetFloat(ResetID,value);
    }
}
