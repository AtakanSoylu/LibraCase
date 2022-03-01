using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LibraCase.Manager
{
    public class GameManager : Singleton<GameManager>
    {
        public int _currentLevel;

        public System.Action OnStartedLevel;
        public System.Action OnStopedLevel;

        public void OnStartLevel()
        {
            OnStartedLevel?.Invoke();
        }
        public void OnStopLevel()
        {
            OnStopedLevel?.Invoke();
        }


    }
}
