using UnityEngine;
using System.Collections;

public class TutorialCoroutineStartLogic : ScriptableObject
{
    public virtual IEnumerator TutorialStartCoroutine() { yield return null; }
}
