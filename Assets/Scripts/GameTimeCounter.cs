using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimeCounter : MonoBehaviour
{
    public static GameTimeCounter Instance;

    public int CurrentPlayTime;

    [SerializeField]
    private TMP_Text CurrentPlayTimeText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1);

        CurrentPlayTime++;

        int min = CurrentPlayTime / 60;
        int sec = CurrentPlayTime - min * 60;
        if (sec < 10)
            CurrentPlayTimeText.text = min.ToString() + " : 0" + sec.ToString();
        else
            CurrentPlayTimeText.text = min.ToString() + " : " + sec.ToString();

        StartCoroutine(StartTimer());
    }
}
