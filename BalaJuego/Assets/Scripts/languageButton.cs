using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class languageButton : MonoBehaviour
{
  [SerializeField]  Language language;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { settingManager.Instance.changeLanguage(language); });
        
        settingManager.Instance.subscribeToStateChange(changeLanguage);
        changeLanguage(this,settingManager.Instance.getLanguage());
    }
    void changeLanguage(object sender, Language language)
    {
        transform.GetChild(0).gameObject.SetActive(language == this.language);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        settingManager.Instance.unsubscribeToStateChange(changeLanguage);

    }
}
