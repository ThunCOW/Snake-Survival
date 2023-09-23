using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerReferenceHelper : MonoBehaviour
{
    public void LoadLevel()
    {
        GameManager.Instance.LoadLevel();
    }

    public void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }

    public void MusicSetting(bool isOn)
    {
        GameManager.Instance.MusicSetting(isOn);
    }

    public void SoundSetting(bool isOn)
    {
        GameManager.Instance.SoundSetting(isOn);
    }

    public void VFX_Setting(bool isOn)
    {
        GameManager.Instance.VFX_Setting(isOn);
    }

    public void JoystickSetting(bool isOn)
    {
        GameManager.Instance.JoystickSetting(isOn);
    }

    public void PauseGame(bool isTrue)
    {
        GameManager.Instance.PauseGame(isTrue);
    }
}
