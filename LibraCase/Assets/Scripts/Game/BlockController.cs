using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LibraCase.Game
{
    public class BlockController : MonoBehaviour
    {
        public bool _hasBomb;
        public int _height;
        public int _width;

        public void SetCord(int height, int width)
        {
            _height = height;
            _width = width;
        }
    }
}
