using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Tutorial/StartLogic/Level2")]
public class TutorialCoroutineStartLogic_2 : TutorialCoroutineStartLogic
{
    public override IEnumerator TutorialStartCoroutine()
    {
        TutorialUtility.SetTimeFreeze(true);
        TutorialUtility.SetTimerUIAsActive(false);

        yield return new WaitForEndOfFrame();

        TutorialUtility.SetBedSanitationFreeze(false);
        TutorialUtility.SetSpawnFreeze(true);
        TutorialUtility.SetPatientHydration(100);
        TutorialUtility.SetPatientHealth(60);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(false);
        TutorialUtility.SetFreezeExcretion(true);
    }
}
