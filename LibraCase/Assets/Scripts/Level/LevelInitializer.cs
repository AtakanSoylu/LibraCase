using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LevelInitializer : MonoBehaviour
{
    [SerializeField] private GameObject _blockPrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private int[,] _levelMatris;
    private string _levelText;
    public void Start()
    {
        _levelMatris = new int[,]{{ 0, 0, 0, 0 },
                                  { 0, 0, 0, 1 },
                                  { 0, 0, 0, 0 },
                                  { 1, 0, 1, 1 }};

        //(Duzelt)upper bondlar yerine sayiyi al
        for (int i = 0; i < _levelMatris.GetUpperBound(0)+1; i++)
        {
            for (int x = 0; x < _levelMatris.GetUpperBound(1)+1; x++)
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
