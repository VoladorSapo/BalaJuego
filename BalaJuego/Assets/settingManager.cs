using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class settingManager : MonoBehaviour
{
    public static settingManager Instance;
    [field:SerializeField] public bool modeSpeedRun { get; private set; }
    [field:SerializeField] public bool activatedTimer{ get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void changeMode(bool speedRun)
    {
        modeSpeedRun = speedRun;
    }
    public void activateTimer(bool timerOn)
    {
        activatedTimer = timerOn;
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
       settingManager.Instance.changeMode(speedRun);
        if (settingManager.Instance.modeSpeedRun)
        {
            timerToggle.isOn = true;
        }
    }
    public void changeTimerToggle(bool timerOn)
    {
        settingManager.Instance.activateTimer(timerOn);

    }
}
