﻿using System;
using System.Threading;

namespace Burrow.Internal
{
    public class DefaultRetryPolicy : IRetryPolicy
    {
        private readonly int _maxDelayTime;
        private const int DelayGap = 1000;
        private volatile bool _iswaiting;
        private volatile int _delayTime;

        public DefaultRetryPolicy(int maxDelayTime = 5 * 60 * 1000 /* 5 minutes */)
        {
            _maxDelayTime = maxDelayTime;
            _delayTime = 0;
        }

        public int DelayTime { get { return _delayTime; } }

        public bool IsWaiting
        {
            get { return _iswaiting; }
        }

        public void WaitForNextRetry(Action retryingAction)
        {
            _iswaiting = true;
            var timer = new Timer(state =>
            {
                _delayTime = _delayTime == 0
                           ? DelayGap
                           : _delayTime * 2;

                if (_delayTime > _maxDelayTime)
                {
                    _delayTime = _maxDelayTime;
                }
                _iswaiting = false;
                retryingAction();
            });

            timer.Change(_delayTime, Timeout.Infinite);
        }

        public void Reset()
        {
            _delayTime = 0;
            _iswaiting = false;
        }
    }
}
