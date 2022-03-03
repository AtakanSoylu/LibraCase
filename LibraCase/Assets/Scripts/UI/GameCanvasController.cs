using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LibraCase.Player;
using LibraCase.Manager;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace LibraCase.UI
{
    public class GameCanvasController : MonoBehaviour
    {
        [SerializeField] private GameScene _gameScene;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private TMP_Text _leftBombCountText;

        [Header("For Final UI")]
        [SerializeField] private GameObject _finalUIObject;
        [SerializeField] private TMP_Text _headerText;
        [SerializeField] private Color _starOpenColor;
        [SerializeField] private Image[] _starsArray;

        public void Start()
        {
            _levelText.text ="LEVEL-" + GameManager.Instance.CurrentLevel;
            _gameScene.OnLoadedScene += OnLoadedScene;
            _gameScene.OnDroppedBomb += OnDroppedBomb;

        }
        public void OnLoadedScene(int leftBomb)
        {
            _leftBombCountText.text = "" + leftBomb;
        }
        public void OnDroppedBomb(int leftBomb)
        {
            _leftBombCountText.text = "" + leftBomb;
        }

        public void OnClickLevelsButton()
        {
            SceneManager.LoadScene("LevelsScene");
        }

        public void OnClickPlayAgainButton()
        {
            SceneManager.LoadScene("GameScene");

        }







        public void OpenWinUI()
        {
            _gameScene.GameFinished();
            if (_gameScene._leftBombCount == 2)
            {
                for (int i = 0; i < _starsArray.Length; i++)
                {
                    _starsArray[i].color = _starOpenColor;
                    AdjustPrefs(3);
                }
            }
            else if(_gameScene._leftBombCount == 1)
            {
                for (int i = 0; i < _starsArray.Length - 1; i++)
                {
                    _starsArray[i].color = _starOpenColor;
                    AdjustPrefs(2);
                }
            }
            else if (_gameScene._leftBombCount == 0)
            {
                for (int i = 0; i < _starsArray.Length - 2; i++)
                {
                    _starsArray[i].color = _starOpenColor;
                    AdjustPrefs(1);
                }
            }

            _headerText.text = "CONGRATS!";
            _finalUIObject.SetActive(true);
            _finalUIObject.GetComponent<RectTransform>().DOLocalMoveY(-Screen.height, .6f).From().SetEase(Ease.InOutBack);
            _finalUIObject.GetComponent<RectTransform>().DOScale(Vector3.zero, .8f).From().SetEase(Ease.InOutBack);
        }

        public void AdjustPrefs(int starsCount)
        {
            int currentLevel = GameManager.Instance.CurrentLevel;
            int prevStartCount = PlayerPrefs.GetInt($"Level{currentLevel}Stars");
            if (starsCount > prevStartCount)
            {
                PlayerPrefs.SetInt($"Level{currentLevel}Stars", starsCount);
                PlayerPrefs.SetInt("TotalStars", PlayerPrefs.GetInt("TotalStars") + (starsCount - prevStartCount));
            }
        }

        public void OpenFailUI()
        {
            _headerText.text = "GAMEOVER!";
            _finalUIObject.SetActive(true);
            _finalUIObject.GetComponent<RectTransform>().DOLocalMoveY(-Screen.height, .6f).From().SetEase(Ease.InOutBack);
            _finalUIObject.GetComponent<RectTransform>().DOScale(Vector3.zero, .8f).From().SetEase(Ease.InOutBack);
        }

    }
}
