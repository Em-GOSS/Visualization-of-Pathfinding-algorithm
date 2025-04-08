using UnityEngine;

[System.Serializable]
public class MaterialOrginProperties 
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

    public MaterialOrginProperties(MaterialPropertiesToPopulate customMaterialPropertiesToPopulate)
    {
        propertyName=customMaterialPropertiesToPopulate.propertyName;
        valueType=customMaterialPropertiesToPopulate.valueType;
        floatPropertyPopulateValue=customMaterialPropertiesToPopulate.floatPropertyPopulateValue;
        colorPropertyPopulateValue=customMaterialPropertiesToPopulate.colorPropertyPopulateValue;
        populateSeconds=customMaterialPropertiesToPopulate.populateSeconds;
    }

}
