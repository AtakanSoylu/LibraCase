using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBarController : MonoBehaviour
{
    [SerializeField] private int _levelIndex;
    [SerializeField] private Sprite[] _starsArray;
    [SerializeField] private GameObject _activeButton;
    [SerializeField] private GameObject _pasiveButton;

    public void Start()
    {
        if(PlayerPrefs.GetInt($"Level{_levelIndex}Stars") > 0)
        {
            _activeButton.SetActive(true);
            _pasiveButton.SetActive(false);
        }
        else
        {
            _activeButton.SetActive(false);
            _pasiveButton.SetActive(true);
        }
    }
}
