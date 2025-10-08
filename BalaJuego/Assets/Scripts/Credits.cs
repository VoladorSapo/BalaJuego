using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        TimePoints time = ServiceLocator.Instance.Get<ITimer>().getSeconds();
        text.text = getlanguage(settingManager.Instance.getLanguage()) + ": " + string.Format("{0:00}:{1:00}:{2:00}", time.hours, time.minutes, time.seconds);
    }
    string getlanguage(Language language)
    {
        switch (language)
        {
            case Language.Spanish:
                return "Tiempo Total";
                break;
            case Language.English:
                return "Total Time";
                break;
            case Language.Catalan:
                return "Temps Total";
                break;
        }
        return "";
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void goToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
