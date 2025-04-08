using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName ="ScriptableObject/UI/SoTileTypeDetails",fileName = "SoTileTypeDetails")]
public class SoTileTypeDetails : ScriptableObject
{
    public tileType tileType;
    
    public Sprite tileSprite;

    public string DrawerText;

    public Color textColor;
}
