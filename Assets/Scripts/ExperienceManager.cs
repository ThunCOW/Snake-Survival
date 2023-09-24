using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static ExperienceManager Instance;

    [SerializeField]
    private int currentLVL;
    [SerializeField]
    private int _currentEXP;
    public int currentEXP
    {
        get { return _currentEXP; }
        set
        {
            _currentEXP = value;

            if (currentEXP >= experienceReq[currentLVL])
            {
                currentLVL++;
                currentEXP = 0;
                LevelUp?.Invoke();
            }
            TEMPAUDIOSOURCE.PlayOneShot(TEMPCLIP, 0.3f * GameManager.Instance.SoundVolume);

            EXP_Bar.transform.localScale = new Vector3(currentEXP / (float)experienceReq[currentLVL], 1, 1);
        }
    }
    [SerializeField]
    private List<int> experienceReq;

    [Space]
    [SerializeField]
    private RectTransform EXP_Bar;

    public Action LevelUp;

    public AudioSource TEMPAUDIOSOURCE;
    public AudioClip TEMPCLIP;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //currentEXP = 0;    
    }
}
