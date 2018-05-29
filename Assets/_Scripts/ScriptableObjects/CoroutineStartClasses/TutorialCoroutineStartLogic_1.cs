using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Tutorial/StartLogic/Level1")]
public class TutorialCoroutineStartLogic_1 : TutorialCoroutineStartLogic
{
    public override IEnumerator TutorialStartCoroutine()
    {
        TutorialUtility.SetSpawnFreeze(true);

        yield return new WaitForEndOfFrame();

        TutorialUtility.SetBedSanitationUIActive(false);
        TutorialUtility.SetSpawnFreeze(true);
        TutorialUtility.SetTimeFreeze(true);
        TutorialUtility.SetActionablesActive(false);
        TutorialUtility.SetTimerUIAsActive(false);
    }
}
