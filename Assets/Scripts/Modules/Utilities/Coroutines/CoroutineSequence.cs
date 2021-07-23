using System;
using System.Collections;
using UnityEngine;

namespace GameUtilities.CoroutineHelper
{
    public class CoroutineSequence
    {
        public bool IsRunning { get; private set; }
        public Action EndRoutineCallback;

        private IEnumerator[] sequencedCoroutines = new IEnumerator[0];
        private Coroutine semaphoreRoutine;

        public CoroutineSequence(Action callback = null)
        {
            this.EndRoutineCallback = callback;
        }

        public void End()
        {
            if (IsRunning)
            {
                CoroutineHost.Host.StopCoroutine(semaphoreRoutine);
                EndRoutineCallback?.Invoke();
            }
        }

        public void Run(params IEnumerator[] coroutines)
        {
            sequencedCoroutines = coroutines;
            semaphoreRoutine = CoroutineHost.Host.StartCoroutine(SemaphoreCoroutine(EndRoutineCallback));
        }

        public void Stop()
        {
            if (IsRunning)
            {
                CoroutineHost.Host.StopCoroutine(semaphoreRoutine);
            }
        }

        private IEnumerator SemaphoreCoroutine(Action endCallback)
        {
            IsRunning = true;

            foreach (IEnumerator routine in sequencedCoroutines)
            {
                yield return routine;
            }

            IsRunning = false;

            endCallback?.Invoke();
        }
    }
}