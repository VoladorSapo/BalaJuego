using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class settingManager : MonoBehaviour
{
 [SerializeField]   Language currentLanguage;

    public static settingManager Instance;
    [field:SerializeField] public bool skipCutscenes { get; private set; }
    [field: SerializeField] public bool skipTutorial { get; private set; }
    [field:SerializeField] public bool activatedTimer{ get; private set; }
    public event EventHandler<Language> changeLanguageEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void changeCutsceneSetting(bool speedRun)
    {
        skipCutscenes = speedRun;
    }
    public void activateTimer(bool timerOn)
    {
        activatedTimer = timerOn;
    }
    public void changeTutorialSetting(bool on)
    {
        skipTutorial = on;
    }
    public Language getLanguage() => currentLanguage;

    public void changeLanguage(Language language)
    {
        print("me cambiaron el idioma a" + language);
        currentLanguage =language;
        changeLanguageEvent.Invoke(this,currentLanguage);
    }
    public void subscribeToStateChange(EventHandler<Language> response)
    {
        changeLanguageEvent += response;

    }
}
public class menuSettingChanger:MonoBehaviour
{
    [SerializeField] Toggle modeToggle;
    [SerializeField] Toggle timerToggle;
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            modeToggle.onValueChanged.AddListener(changeModeToggle);
            timerToggle.onValueChanged.AddListener(changeTimerToggle);
        }
    }
    public void changeModeToggle(bool speedRun)
    {
       settingManager.Instance.changeCutsceneSetting(speedRun);
        if (settingManager.Instance.skipCutscenes)
        {
            timerToggle.isOn = true;
        }
    }
    public void changeTimerToggle(bool timerOn)
    {
        settingManager.Instance.activateTimer(timerOn);

    }
}
[System.Serializable]
public enum Language
{
    Spanish = 0,
    English = 1,
    Catalan = 2
}