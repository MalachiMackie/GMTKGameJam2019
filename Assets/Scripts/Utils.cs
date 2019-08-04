using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Utils
    {
        public static IEnumerator DoAfterSeconds(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action.Invoke();
        }
    }
}
