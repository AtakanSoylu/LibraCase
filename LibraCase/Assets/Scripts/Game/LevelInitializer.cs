using LibraCase.Manager;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace LibraCase.Game
{
    public class LevelInitializer : MonoBehaviour
    {
        [SerializeField] private GameConfiguration _gameConfiguration;
        [SerializeField] private GameObject _blockPrefab;
        [SerializeField] private GameObject _wallPrefab;
        [SerializeField] private int[,] _levelMatris;
        private string _levelText;


        private int M; //Row Count
        private int N; //Column Count

        public void Start()
        {
            AdjustLevelScene();
        }

        private void AdjustLevelScene()
        {
            StartCoroutine(GetLevelTextRemote());
        }

        IEnumerator GetLevelTextRemote()
        {
            string LevelUrl = $"https://engineering-case-study.s3.eu-north-1.amazonaws.com/LS_Case_Level-{GameManager.Instance._currentLevel}";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(LevelUrl))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.Log("Error");
                    //Filedan cek
                }
                else
                {
                    ReadLevel(webRequest.downloadHandler.text);
                }
            }
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



        public void AdjustMatrixAndInstantiateLevel(string levelText)
        {
            LevelDesignData levelDesignData = _gameConfiguration.GetLevelDesign(N);
            _levelMatris = new int[M, N];
            for (int i = 0; i < M; i++)
            {
                for (int x = 0; x < N; x++)
                {
                    _levelMatris[i, x] = (int)(levelText[i * N * 2 + x * 2] - '0');

                    if (_levelMatris[i, x] == 0)
                    {
                        var instantiated = Instantiate(_blockPrefab, transform);
                        instantiated.transform.position = new Vector3(
                            levelDesignData._startXCord + x * levelDesignData._increaseOffset,
                            levelDesignData._startYCord - i * levelDesignData._increaseOffset);
                        instantiated.transform.localScale = Vector3.one * levelDesignData._blockScale;
                    }
                    else
                    {
                        var instantiated = Instantiate(_wallPrefab, transform);
                        instantiated.transform.position = new Vector3(
                            levelDesignData._startXCord + x * levelDesignData._increaseOffset,
                            levelDesignData._startYCord - i * levelDesignData._increaseOffset);
                        instantiated.transform.localScale = Vector3.one * levelDesignData._blockScale;
                    }
                }
            }
            FindBestWay();

        }

        //get lenghtleri duzelt
        public void FindBestWay()
        {
            int bestBombCount = 0;

            for (int i = 0; i < M; i++)
            {
                for (int x = 0; x < N; x++)
                {
                    if (_levelMatris[i, x] == 1)
                    {
                        if (i + 1 < M)
                        {
                            if (_levelMatris[i + 1, x] == 0)
                            {
                                if (LookDoubleDown(i, x))
                                {
                                    //assagı koy
                                    DropBomb(i + 1, x);
                                    bestBombCount++;
                                    continue;
                                }
                                else if (LookRight(i, x))
                                {
                                    //saga koy
                                    DropBomb(i, x + 1);
                                    bestBombCount++;
                                    continue;
                                }
                                else
                                {
                                    //assagi koy
                                    DropBomb(i + 1, x);
                                    bestBombCount++;
                                    continue;
                                }
                            }
                        }
                        if (x + 1 < N)
                        {
                            if (_levelMatris[i, x + 1] == 0)
                            {
                                //sag koy
                                DropBomb(i, x + 1);
                                bestBombCount++;
                                continue;
                            }
                        }
                        if (x > 0)
                        {
                            if (_levelMatris[i, x - 1] == 0)
                            {
                                //sola koy
                                DropBomb(i, x - 1);
                                bestBombCount++;
                                continue;
                            }
                        }
                        if (i > 0)
                        {
                            if (_levelMatris[i - 1, x] == 0)
                            {
                                //uste koy
                                DropBomb(i - 1, x);
                                bestBombCount++;
                                continue;
                            }
                        }
                    }
                }
            }
            Debug.Log("Best Way = " + bestBombCount);
        }

        public void DropBomb(int i, int x)
        {
            if (x > 0) _levelMatris[i, x - 1] = 0;
            if (x + 1 < N) _levelMatris[i, x + 1] = 0;
            if (i + 1 < M) _levelMatris[i + 1, x] = 0;
            if (i > 0) _levelMatris[i - 1, x] = 0;
        }

        public bool LookDoubleDown(int i, int x) // Alt satira koydugumuzda iki yada daha fazla duvar yikiyormu // Alpe cevirt
        {
            int downWallCount = 0;

            //Sol alt ve sag alt kontrol eder
            if (x > 0 && x + 1 < N)
            {
                if (_levelMatris[i + 1, x - 1] == 1)
                {
                    downWallCount++;
                }
                if (_levelMatris[i + 1, x + 1] == 1)
                {
                    downWallCount++;
                }
            }

            if (i + 2 < M)
            {
                if (_levelMatris[i + 2, x] == 1)
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

        public bool LookRight(int i, int x) // 2 kare sag bak // Alpcevirt
        {
            if (x + 2 < N)
            {
                if (_levelMatris[i, x + 2] == 1)
                {
                    return true;
                }
            }
            return false;
        }



    }
}
