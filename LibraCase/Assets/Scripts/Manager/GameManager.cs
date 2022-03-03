using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LibraCase.Manager
{
    public class GameManager : Singleton<GameManager>
    {        
        public int CurrentLevel;
        
        public System.Action OnStartedLevel;
        public System.Action OnStopedLevel;

        public override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }
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
