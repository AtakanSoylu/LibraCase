using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private int[,] _levelMatris;
    private string _levelText;


    private int M;
    private int N;

    public void Start()
    {
        _levelMatris = new int[,]{{ 0, 1, 0, 1,0,1 },
                                  { 1, 0, 1, 0,0,0 },
                                  { 0, 0, 0, 0,0,1 },
                                  { 1, 0, 0, 1 ,0,1},
        };
        M = _levelMatris.GetLength(0);
        N = _levelMatris.GetLength(1);

        for (int i = 0; i < M; i++)
        {
            for (int x = 0; x < N; x++)
            {

                if (_levelMatris[i,x] == 0)
                {
                    var instantiated = Instantiate(_blockPrefab, transform);
                    instantiated.transform.localPosition = new Vector3(-1 + x * 0.7f, 2.8f + 0.7f * i);
                }
                else
                {
                    var instantiated = Instantiate(_wallPrefab, transform);
                    instantiated.transform.localPosition = new Vector3(-1 + x * 0.7f, 2.8f + 0.7f * i);
                }
            }
        }
        AdjustLevelScene();
        FindBestWay();
    }

    //get lenghtleri duzelt
    public void FindBestWay()
    {
        int bestBombCount = 0;

        for (int i = 0; i < M ; i++)
        {
            for (int x = 0; x < N ; x++)
            {   
                if (_levelMatris[i, x] == 1)
                {
                    if(i+1 < M)
                    {
                        if(_levelMatris[i+1,x] == 0)
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
                    if(x+1 < N)
                    {
                        if (_levelMatris[i, x + 1] == 0)
                        {
                            //sag koy
                            DropBomb(i, x + 1);
                            bestBombCount++;
                            continue;
                        }
                    }
                    if(x > 0)
                    {
                        if(_levelMatris[i,x-1] == 0)
                        {
                            //sola koy
                            DropBomb(i, x - 1);
                            bestBombCount++;
                            continue;
                        }
                    }
                    if(i > 0)
                    {
                        if(_levelMatris[i-1,x] == 0)
                        {
                            //uste koy
                            DropBomb(i-1, x);
                            bestBombCount++;
                            continue;
                        }
                    }

                }
            }
        }
        Debug.Log("Best Way = " + bestBombCount);
    }


    public void DropBomb(int i,int x)
    {
        if (x > 0) _levelMatris[i, x - 1] = 0;        
        if (x+1 < N) _levelMatris[i, x + 1] = 0;
        if (i+1 < M) _levelMatris[i + 1, x] = 0;
        if (i > 0) _levelMatris[i - 1, x] = 0;
    }

    public bool LookDoubleDown(int i,int x) // Alt satira koydugumuzda iki yada daha fazla duvar yikiyormu // Alpe cevirt
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
        
        if(downWallCount > 1)
        {
            return true;
        }
        return false;
    }

    public bool LookRight(int i,int x) // 2 kare sag bak // Alpcevirt
    {
        if(x + 2 < N)
        {
            if(_levelMatris[i,x+2] == 1)
            {
                return true;
            }
        }
        return false;
    }



    private void AdjustLevelScene()
    {
        StartCoroutine(GetRequest());
    }

    IEnumerator GetRequest()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://engineering-case-study.s3.eu-north-1.amazonaws.com/LS_Case_Level-1"))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if(webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("Error");
            }
            else
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
