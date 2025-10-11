using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenuLanguageText : MonoBehaviour
{

    [SerializeField] string[] Texts;
   [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();

        settingManager.Instance.subscribeToStateChange(changeLanguage);
        updateLanguage();
    }
    void changeLanguage(object sender, Language language)
    {
        text.text = Texts[(int)language];
    }
    public void updateLanguage()
    {
        text = GetComponent<TMP_Text>();
        
        text.text = Texts[(int)settingManager.Instance.getLanguage()];

    }
    private void OnDestroy()
    {
        settingManager.Instance.unsubscribeToStateChange(changeLanguage);

    }
}