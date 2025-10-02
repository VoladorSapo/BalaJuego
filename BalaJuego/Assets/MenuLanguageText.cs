using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenuLanguageText : MonoBehaviour
{

    [SerializeField] string[] Texts;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();

        settingManager.Instance.subscribeToStateChange(changeLanguage);
    }
    void changeLanguage(object sender, Language language)
    {
        text.text = Texts[(int)language];
    }
}
