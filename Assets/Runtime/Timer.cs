using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime
{
    public class Timer : MonoBehaviour
    {
        public event Action<SideType> EndTimerHandle;

        private float _timeToRememberForOneCard;
        private float _targetTime;
        private bool _timerIsActive;
        private SideType _sideType;

        public void Init(float time) =>
            _timeToRememberForOneCard = time;

        public void StartTimer(SideType sideType, int totalCards)
        {
            _targetTime = Time.time + (_timeToRememberForOneCard * totalCards);
            _timerIsActive = true;
            _sideType = sideType;
        }

        private void Update()
        {
            if (!_timerIsActive || !(Time.time >= _targetTime))
                return;

            _timerIsActive = false;
            EndTimerHandle?.Invoke(_sideType);
        }
    }
}