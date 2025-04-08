using UnityEngine;

[System.Serializable]
public class MaterialPropertiesToPopulate 
{       

    [SerializeField]
    public string propertyName;

    [SerializeField] 
    public MaterialPropertyValueType valueType;
    
    [SerializeField]
    public float floatPropertyPopulateValue;

    [SerializeField]
    public Color colorPropertyPopulateValue;

    [SerializeField] public float populateSeconds;

    [SerializeField] public float fontTargetSize;

    public MaterialPropertiesToPopulate(MaterialPropertiesToPopulate customMaterialOrginProperties)
    {
        propertyName=customMaterialOrginProperties.propertyName;
        valueType=customMaterialOrginProperties.valueType;
        floatPropertyPopulateValue=customMaterialOrginProperties.floatPropertyPopulateValue;
        colorPropertyPopulateValue=customMaterialOrginProperties.colorPropertyPopulateValue;
        populateSeconds=customMaterialOrginProperties.populateSeconds;
    }

}
