using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingletonMonoBehaviour<UIManager>
{    
    [Header("Canvas Group Set In the UI")]
    [SerializeField] private CanvasGroup MenuPanelCanvasGroup;
    [SerializeField] private CanvasGroup InputUICanvasGroup;
    [SerializeField] private CanvasGroup BeginUICanvasGroup;

    [Space(10)]
    [SerializeField] GridCursor gridCursor;


    [Header("FadeSeconds")]
    [SerializeField]
    private float CanvasGroupFadeSeconds=0f;
 
    [SerializeField] private Fader Fader;

    protected override void Awake() 
    {
        EventHandler.EnterUICloseEvent+=OpenMenuPanelCanvasGroup;
    }   

    private void OpenMenuPanelCanvasGroup()
    {   
        StartCoroutine(FadeInAndActiveCanvasGroup(MenuPanelCanvasGroup));
    }

    public IEnumerator FadeInAndActiveCanvasGroup(CanvasGroup targetCanvasGroup)
    {   
        targetCanvasGroup.gameObject.SetActive(true);

        yield return Fader.Instance.Fade(targetCanvasGroup,CanvasGroupFadeSeconds,1f);

        targetCanvasGroup.blocksRaycasts=true;
    }

    public IEnumerator FadeOutAndInactiveCanvasGroup(CanvasGroup targetCanvasGroup)
    {   
        yield return Fader.Instance.Fade(targetCanvasGroup,CanvasGroupFadeSeconds,0f);

        targetCanvasGroup.blocksRaycasts=false;

        targetCanvasGroup.gameObject.SetActive(false);
    }

    public IEnumerator SwapUI(CanvasGroup orginCanvasGroup,CanvasGroup targetCanvasGroup)
    {
        yield return FadeOutAndInactiveCanvasGroup(orginCanvasGroup);

        yield return FadeInAndActiveCanvasGroup(targetCanvasGroup);
    }


    //ButtonClcikMethods

    public void SwapBeginUIToInputUI()
    {
        StartCoroutine(SwapUI(BeginUICanvasGroup,InputUICanvasGroup));
    }
    
    public void CloseInputUI()
    {
        StartCoroutine(CloseInputUICoroutine());
    }

    public IEnumerator CloseInputUICoroutine()
    {   
        EventHandler.CallAccessInputEvent();
        EventHandler.CallBeforeEnterMapEvent();
        EventHandler.CallAfterEnterMapEvent();

        yield return FadeOutAndInactiveCanvasGroup(MenuPanelCanvasGroup);

        gridCursor.setGridCursor(true,true);

    }

    

}
