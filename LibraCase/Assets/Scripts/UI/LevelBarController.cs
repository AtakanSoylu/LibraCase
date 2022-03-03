using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LibraCase.Manager;
using UnityEngine.SceneManagement;

public class LevelBarController : MonoBehaviour
{
    [SerializeField] private int _levelIndex;

    [Header("Stars Veriable")]
    [SerializeField] private GameObject _starsParent;
    [SerializeField] private Image[] _starsArray;
    [SerializeField] private Color _starsActiveColor;

    [Header("Butons Veriable")]
    [SerializeField] private GameObject _activeButton;
    [SerializeField] private GameObject _pasiveButton;

    [Header("Level Veriable")]
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private Slider _levelProgresSlider;
    [SerializeField] private TMP_Text _levelProgressText;

    private int _currentStarCount;
    private int _totalStarCount;

    public void Start()
    {
        _levelIndex = transform.GetSiblingIndex() + 1;
        _levelText.text = $"LEVEL-{_levelIndex}";
        AdjustButtonStatus();
        AdjustStars();
    }


    public void AdjustButtonStatus()
    {
        _currentStarCount = PlayerPrefs.GetInt($"Level{_levelIndex}Stars");
        if (_levelIndex == 1)
        {
            OpenButton();
            return;
        }
        _totalStarCount = PlayerPrefs.GetInt("TotalStars");
   

        if (_currentStarCount > 0)
        {
            OpenButton();
        }
        else
        {
            if (PlayerPrefs.GetInt($"Level{_levelIndex - 1}Stars") > 0)
            {
                if (_levelIndex % 5 == 0)
                {
                    if (_totalStarCount >= (_levelIndex - 1) * 2)
                    {
                        OpenButton();
                    }
                    else
                    {
                        CloseButton();
                        AdjustProgressBar();
                    }
                }
                else
                {
                    OpenButton();
                }
            }
            else
            {
                CloseButton();
            }
        }
    }

    public void AdjustProgressBar()
    {
        _starsParent.SetActive(false);
        _levelProgresSlider.gameObject.SetActive(true);
        int maxValue = (_levelIndex - 1) * 2;
        _levelProgresSlider.maxValue = maxValue;
        _levelProgresSlider.value = _totalStarCount;
        _levelProgressText.text = _totalStarCount + "/" + maxValue;
    }


    public void AdjustStars()
    {
        for (int i = 0; i < _currentStarCount; i++)
        {
            _starsArray[i].color = _starsActiveColor;
        }
    }


    public void OpenButton()
    {
        _activeButton.SetActive(true);
        _pasiveButton.SetActive(false);
    }

    public void CloseButton()
    {
        _activeButton.SetActive(false);
        _pasiveButton.SetActive(true);
    }

    public void OnClickedPlayButton()
    {
        GameManager.Instance.CurrentLevel = _levelIndex;
        SceneManager.LoadScene("GameScene");
    }
}
