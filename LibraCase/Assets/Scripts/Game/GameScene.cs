using LibraCase.Game;
using LibraCase.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LibraCase.Player
{

    public class GameScene : MonoBehaviour
    {
        [SerializeField] private bool _gameOn;
        [SerializeField] private GameObject _bombPrefab;
        [HideInInspector] private Camera _mainCamera;
        [SerializeField] private LevelManager _levelManager;

        [Header("***Events***")]
        public System.Action<int> OnDroppedBomb;
        public System.Action<int> OnLoadedScene;

        [Header("***Status***")]
        public int _leftBombCount;
        public int _totalWallCount;
        
        private void Start()
        {
            _gameOn = true;
            _mainCamera = Camera.main;
            GameManager.Instance.OnStartedLevel += OnStartedLevel;
            GameManager.Instance.OnStopedLevel += OnStopedLevel;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnStartedLevel -= OnStartedLevel;
            GameManager.Instance.OnStopedLevel -= OnStopedLevel;
        }

        public void GameFinished()
        {
            _gameOn = false;
        }

        public void AdjustGameConfiguration(int bestWay)
        {
            _leftBombCount = bestWay + 2;
            OnLoadedScene?.Invoke(_leftBombCount);
        }

        private void Update()
        {
            if (!_gameOn) return;
            if (Input.GetMouseButtonDown(0))
            {
                if (_leftBombCount > 0) {
                    RaycastHit2D hit = Physics2D.Raycast(_mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.transform != null)
                    {
                        if (hit.collider.CompareTag("Block"))
                        {
                            BlockController block = hit.collider.GetComponent<BlockController>();
                            if (block != null && !block._hasBomb)
                            {
                                var instantiated = Instantiate(_bombPrefab, hit.transform);
                                block._hasBomb = true;
                                _leftBombCount--;
                                OnDroppedBomb?.Invoke(_leftBombCount);
                                _levelManager.DropBomb(block._height, block._width);
                            }
                        }
                    } 
                }
            }
        }
        
        
        
        public void OnStartedLevel()
        {
            _gameOn = true;
        }

        public void OnStopedLevel()
        {
            _gameOn = false;
        }


    }
}
