using LibraCase.Manager;
using LibraCase.Player;
using LibraCase.UI;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace LibraCase.Game
{
    public class LevelManager : MonoBehaviour
    {
        #region Singleton
        public static LevelManager Instance;
        public void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }
        #endregion
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private int[,] _levelMatrix;
        [SerializeField] private int[,] _templevelMatrix;
        [SerializeField] private GameScene _gameScene;
        [SerializeField] private GameCanvasController _gameCanvasController;



        public int _wallCount;

        private int M; //Row Count
        private int N; //Column Count
        public void Start()
        {
            AdjustLevelScene();
        }

        private void AdjustLevelScene()
        {
            StartCoroutine(GetLevelDataRemote());
        }

        IEnumerator GetLevelDataRemote()
        {
            string LevelUrl = $"https://engineering-case-study.s3.eu-north-1.amazonaws.com/LS_Case_Level-{GameManager.Instance.CurrentLevel}";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(LevelUrl))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    ReadLevelInResources();
                }
                else
                {
                    ReadLevel(webRequest.downloadHandler.text);
                }
            }
        }

        public void ReadLevelInResources()
        {
            string path = $"Assets/Resources/LS_Case_Level-{GameManager.Instance.CurrentLevel}";
            //Read the text from directly from the test.txt file
            string readedText = File.ReadAllText(path);
            ReadLevel(readedText);
        }

        public void ReadLevel(string levelText)
        {
            for (int i = 0; i < levelText.Length; i++)
            {
                if (levelText[i].Equals('\n'))
                {
                    N = (i + 1) / 2;
                    M = (levelText.Length + 1) / i;
                    break;
                }
            }
            AdjustMatrixAndInstantiateLevel(levelText);

        }


        //Level initialize
        public void AdjustMatrixAndInstantiateLevel(string levelText)
        {
            LevelDesignData levelDesignData = _gameConfiguration.GetLevelDesign(N);
            _levelMatrix = new int[M, N];
            _templevelMatrix = new int[M, N];
            for (int i = 0; i < M; i++)
            {
                for (int x = 0; x < N; x++)
                {
                    _levelMatrix[i, x] = (int)(levelText[i * N * 2 + x * 2] - '0');
                    _templevelMatrix[i, x] = (int)(levelText[i * N * 2 + x * 2] - '0');

                    if (_templevelMatrix[i, x] == 0)
                    {
                        var instantiated = Instantiate(_blockPrefab, transform);
                        instantiated.transform.position = new Vector3(
                            levelDesignData._startXCord + x * levelDesignData._increaseOffset,
                            levelDesignData._startYCord - i * levelDesignData._increaseOffset);
                        instantiated.transform.localScale = Vector3.one * levelDesignData._blockScale;
                        instantiated.GetComponent<BlockController>().SetCord(i, x);
                    }
                    else
                    {
                        var instantiated = Instantiate(_wallPrefab, transform);
                        instantiated.transform.position = new Vector3(
                            levelDesignData._startXCord + x * levelDesignData._increaseOffset,
                            levelDesignData._startYCord - i * levelDesignData._increaseOffset);
                        instantiated.transform.localScale = Vector3.one * levelDesignData._blockScale;
                        instantiated.GetComponent<WallController>().SetCord(i, x);
                        _wallCount++;
                    }
                }
            }
            FindBestWay();

        }

        public void FindBestWay()
        {
            int bestBombCount = 0;

            for (int i = 0; i < M; i++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (_templevelMatrix[i, x] == 1)
                    {
                        if (i + 1 < M)
                        {
                            if (_levelMatrix[i + 1, x] == 0)
                            {
                                if (LookDoubleDown(i, x))
                                {
                                    //Drop bomb down
                                    FakeDropBomb(i + 1, x);
                                    bestBombCount++;
                                    continue;
                                }
                                if (x + 1 < N)
                                {
                                    if (LookRight(i, x))
                                    {
                                        if (LookDown(i, x))
                                        {
                                            if (LookRight(i, x + 2))
                                            {
                                                //Drop bomb down
                                                FakeDropBomb(i + 1, x);
                                                bestBombCount++;
                                                continue;
                                            }
                                            else
                                            {
                                                //Drop bomb right
                                                if (FakeDropBomb(i, x + 1))
                                                {
                                                    bestBombCount++;
                                                    continue;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Drop bomb right
                                            if (FakeDropBomb(i, x + 1))
                                            {
                                                bestBombCount++;
                                                continue;
                                            }
                                        }
                                    }
                                }
                                //Drop bomb down
                                FakeDropBomb(i + 1, x);
                                bestBombCount++;
                                continue;
                                
                            }
                        }
                        if (x > 0)
                        {
                            if (LookLeftDown(i, x))
                            {
                                //Drop bomb left
                                if (_levelMatrix[i, x - 1] == 0)
                                {
                                    FakeDropBomb(i, x - 1);
                                    bestBombCount++;
                                    continue;
                                }
                            }
                        }
                        if (x + 1 < N)
                        {
                            if (_levelMatrix[i, x + 1] == 0)
                            {
                                //Drop bomb right
                                FakeDropBomb(i, x + 1);
                                bestBombCount++;
                                continue;
                            }
                        }
                        if (x > 0)
                        {
                            if (_levelMatrix[i, x - 1] == 0)
                            {
                                //Drop bomb left
                                FakeDropBomb(i, x - 1);
                                bestBombCount++;
                                continue;
                            }
                        }
                        if (i > 0)
                        {
                            if (_levelMatrix[i - 1, x] == 0)
                            {
                                //Drop bomb up
                                FakeDropBomb(i - 1, x);
                                bestBombCount++;
                                continue;
                            }
                        }
                    }
                }
            }
            _gameScene.AdjustGameConfiguration(bestBombCount);
        }

        public void DropBomb(int i,int x)
        {
            if (x > 0)
            {
                if(_levelMatrix[i, x - 1] == 1)
                {
                    _levelMatrix[i, x - 1] = 0;
                    _wallCount--;
                }
            }
            if (x + 1 < N)
            {
                if (_levelMatrix[i, x + 1] == 1)
                {
                    _levelMatrix[i, x + 1] = 0;
                    _wallCount--;
                }
            }
            if (i + 1 < M)
            {
                if (_levelMatrix[i + 1, x] == 1)
                {
                    _levelMatrix[i + 1, x] = 0;
                    _wallCount--;
                }
            }

            if (i > 0) 
            {
                if (_levelMatrix[i - 1, x] == 1)
                {
                    _levelMatrix[i - 1, x] = 0;
                    _wallCount --;
                } 
            }
            ControlGameEnd();
        }

        public void ControlGameEnd()
        {
            if(_wallCount == 0)
            {
                _gameCanvasController.OpenWinUI();
            }
            else if(_gameScene._leftBombCount == 0)
            {
                Debug.Log("Fail");
                _gameCanvasController.OpenFailUI();
            }

        }

        //If index of wall return false 
        public bool FakeDropBomb(int i, int x)
        {
            if (_levelMatrix[i, x] == 1) return false;
            if (x > 0) _templevelMatrix[i, x - 1] = 0;
            if (x + 1 < N) _templevelMatrix[i, x + 1] = 0;
            if (i + 1 < M) _templevelMatrix[i + 1, x] = 0;
            if (i > 0) _templevelMatrix[i - 1, x] = 0;
            return true;
        }

        //If it's breaking down two or more walls.
        public bool LookDoubleDown(int i, int x) 
        {
            int downWallCount = 0;

            //Control left-down and right-down
            if (x > 0 && x + 1 < N)
            {
                if (_templevelMatrix[i + 1, x - 1] == 1)
                {
                    downWallCount++;
                }
                if (_templevelMatrix[i + 1, x + 1] == 1)
                {
                    downWallCount++;
                }
            }

            if (i + 2 < M)
            {
                if (_templevelMatrix[i + 2, x] == 1)
                {
                    downWallCount++;
                }
            }

            if (downWallCount > 1)
            {
                return true;
            }
            return false;
        }

        //Look two squares right.
        public bool LookRight(int i, int x)
        {
            if (x + 2 < N)
            {
                if (_templevelMatrix[i, x + 2] == 1)
                {
                    return true;
                }
            }
            return false;
        }

        //Look two squares down.
        public bool LookDown(int i, int x) 
        {
            if (i + 2 < M)
            {
                if (_templevelMatrix[i + 2, x ] == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool LookLeftDown(int i ,int x)
        {
            if (i + 1 < M)
            {
                if (_templevelMatrix[i + 1, x - 1] == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
