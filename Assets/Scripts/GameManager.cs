using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tymski;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public SceneReference MainMenu;
    public SceneReference Level_1;

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

    public void LoadLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(Level_1);
    }

    public void LoadMainMenu()
    {
        SettingChanged = null;
        Time.timeScale = 1;
        SceneManager.LoadScene(MainMenu);
    }

    public float MusicVolume;
    public float SoundVolume;
    public void MusicSetting(bool isOn)
    {
        MusicVolume = isOn ? 1 : 0;
    }

    public void SoundSetting(bool isOn)
    {
        SoundVolume = isOn ? 1 : 0;
    }

    public Action SettingChanged;
    public bool VFX_Active;
    public void VFX_Setting(bool isOn)
    {
        VFX_Active = isOn;

        SettingChanged?.Invoke();
    }

    public bool JoystickOn;
    public void JoystickSetting(bool isOn)
    {
        JoystickOn = isOn;
        SettingChanged?.Invoke();
    }

    public void PauseGame(bool isTrue)
    {
        Time.timeScale = isTrue ? 0 : 1;
    }
}
