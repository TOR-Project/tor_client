using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyRowController : MonoBehaviour
{
    [SerializeField]
    Text titleText;
    [SerializeField]
    Text valueText;

    public void updatePropertyData(PropertyData _propertyData)
    {
        titleText.text = CountryManager.instance.getPropertyTitle(_propertyData.propertyCategory);

        switch (_propertyData.propertyType)
        {
            case CountryManager.PROPERTY_TYPE_MINING_INC:
                valueText.text = LanguageManager.instance.getText("ID_MINING_AMOUNT") + string.Format(" +{0:0.0000}%", _propertyData.value);
                break;
            case CountryManager.PROPERTY_TYPE_MINING_DEC:
                valueText.text = LanguageManager.instance.getText("ID_MINING_AMOUNT") + string.Format(" {0:0.0000}%", _propertyData.value);
                break;
            default:
                valueText.text = "";
                break;
        }
    }
}
