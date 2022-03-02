using LibraCase.Game;
using LibraCase.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LibraCase.Player
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private bool _gameOn;
        [HideInInspector] private Camera _mainCamera;
        [SerializeField] private GameObject _bombPrefab;
        
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

        private void Update()
        {
            if (!_gameOn) return;
            if (Input.GetMouseButtonDown(0))
            {
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
