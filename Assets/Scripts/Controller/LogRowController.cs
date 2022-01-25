using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogRowController : MonoBehaviour
{
    [SerializeField]
    Text blockText;
    [SerializeField]
    Text valueText;

    public void updateLogData(LogData _logData, CountryData _data)
    {
        blockText.text = string.Format("({0:#,###})", _logData.blockNum);

        switch(_logData.logType)
        {
            case CountryManager.LOG_TYPE_RESIGNATION:
                valueText.text = string.Format(LanguageManager.instance.getText("ID_LOG_RESIGNATION"), _logData.whoNickName);
                break;
            case CountryManager.LOG_TYPE_INAUGURATION:
                valueText.text = string.Format(LanguageManager.instance.getText("ID_LOG_INAUGURATION"), _logData.whoNickName);
                break;
            case CountryManager.LOG_TYPE_ADJUST_MINING_TAX:
                valueText.text = string.Format(LanguageManager.instance.getText("ID_LOG_ADJUST_MINING_TAX"), float.Parse(_logData.dataInt.ToString()) / 10000);
                break;
            case CountryManager.LOG_TYPE_DONATION:
                valueText.text = string.Format(LanguageManager.instance.getText("ID_LOG_DONATION"), _logData.whoNickName, Utils.convertPebToTorStr(_logData.dataInt));
                break;
            case CountryManager.LOG_TYPE_START_RESEARCH:
                // TODO
                valueText.text = LanguageManager.instance.getText("ID_LOG_START_RESEARCH");
                break;
            case CountryManager.LOG_TYPE_STOP_RESEARCH_STOP:
                // TODO
                valueText.text = LanguageManager.instance.getText("ID_LOG_STOP_RESEARCH_STOP");
                break;
            case CountryManager.LOG_TYPE_END_RESEARCH:
                // TODO
                valueText.text = LanguageManager.instance.getText("ID_LOG_END_RESEARCH");
                break;
            case CountryManager.LOG_TYPE_OCCUR_REBELLION:
                valueText.text = string.Format(LanguageManager.instance.getText("ID_LOG_OCCUR_REBELLION"), _logData.whoNickName);
                break;
            case CountryManager.LOG_TYPE_FAIL_REBELLION:
                valueText.text = LanguageManager.instance.getText("ID_LOG_FAIL_REBELLION");
                break;
            case CountryManager.LOG_TYPE_SUCCESS_REBELLION:
                valueText.text = LanguageManager.instance.getText("ID_LOG_SUCCESS_REBELLION");
                break;
            case CountryManager.LOG_TYPE_RENAME_CASTLE:
                valueText.text = string.Format(LanguageManager.instance.getText("ID_LOG_RENAME_CASTLE"), _logData.dataStr);
                break;
            case CountryManager.LOG_TYPE_ADD_PROPERTY:
                valueText.text = string.Format(LanguageManager.instance.getText("ID_LOG_ADD_PROPERTY"), CountryManager.instance.getPropertyTitle(_data.propertyList[int.Parse(_logData.dataInt.ToString())].propertyCategory));
                break;
            default:
                valueText.text = "";
                break;
        }
}
}
