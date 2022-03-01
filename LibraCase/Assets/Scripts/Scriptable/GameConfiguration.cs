using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/GameConfiguration",fileName = "GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
    public LevelDesignData[] _levelDesignArray;
    public LevelDesignData GetLevelDesign(int rows)
    {
        for (int i = 0; i < _levelDesignArray.Length; i++)
        {
            if(rows == _levelDesignArray[i]._rowsCount)
            {
                return _levelDesignArray[i];
            }
        }
        return null;
    }
}

[System.Serializable]
public class LevelDesignData
{
    public int _rowsCount;
    public float _startXCord;
    public float _startYCord;
    public float _increaseOffset;
    public float _blockScale;
}