using System.Collections;
using UnityEngine;

namespace GameUtilities.CoroutineHelper
{
    [System.Obsolete("Use CoroutineSequence instead.")]
    public class CoroutinePlayable
    {
        public bool IsPlaying { get; private set; }

        private MonoBehaviour coroutineInvoker;
        private Coroutine currentCoroutine;

        public CoroutinePlayable(MonoBehaviour coroutineInvoker)
        {
            this.coroutineInvoker = coroutineInvoker;
        }

        public void Play(params IEnumerator[] coroutines)
        {
            currentCoroutine = coroutineInvoker.StartCoroutine(SemaphoreCoroutine(coroutines));
        }

        public void Stop()
        {
            if (IsPlaying)
            {
                coroutineInvoker.StopCoroutine(currentCoroutine);
            }

            IsPlaying = false;
        }

        private IEnumerator SemaphoreCoroutine(IEnumerator[] coroutines)
        {
            IsPlaying = true;

            foreach (IEnumerator routine in coroutines)
            {
                yield return routine;
            }

            IsPlaying = false;
        }
    }
}