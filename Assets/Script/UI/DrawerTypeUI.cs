using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DrawerTypeUI : MonoBehaviour
{    
    [Header("Custom ScriptableObject")]
    [SerializeField]
    private List<SoTileTypeDetails> tileTypeDetailsList=new List<SoTileTypeDetails>();
    
    [Space(10)]
    [SerializeField] private Fader fader;

    [Space(10)]
    [Header("Component section")]
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private Image tileTypeImage;


    [Header("Fade details")]
    [SerializeField] private CanvasGroup canvasGroup;
    [Range(0,1)]
    [SerializeField] private float maxAlpha;
    [Range(0,1)]
    [SerializeField] private float fadeSeconds;
    
    [Range(0,2)]
    [SerializeField] private float UIEmergedSeconds;

    private void OnEnable() 
    {
        EventHandler.DrawerTypeChangeEvent+=ActiveTileTypeUI;
    }

    private void Ondisable() 
    {
        EventHandler.DrawerTypeChangeEvent-=ActiveTileTypeUI;
    }

    private void RefreshUI(tileType tileType) 
    {
        foreach(SoTileTypeDetails soTileTypeDetails in tileTypeDetailsList)
        {
            if(tileType==soTileTypeDetails.tileType)
            {
                textMeshProUGUI.text=soTileTypeDetails.DrawerText;
                textMeshProUGUI.color=soTileTypeDetails.textColor;
                tileTypeImage.sprite=soTileTypeDetails.tileSprite;
                return;
            }
        }
        Debug.Log("Error");
    }

    private void ActiveTileTypeUI(tileType tileType)
    {   
        StopAllCoroutines();
        StartCoroutine(ActiveTileTypeUICoroutine(tileType));
    }
    private IEnumerator ActiveTileTypeUICoroutine(tileType tileType)
    {   
        RefreshUI(tileType);
        yield return Fader.Instance.Fade(canvasGroup,fadeSeconds,maxAlpha);
        
        yield return new WaitForSeconds(UIEmergedSeconds);
        yield return Fader.Instance.Fade(canvasGroup,fadeSeconds,0f);
    }

}
