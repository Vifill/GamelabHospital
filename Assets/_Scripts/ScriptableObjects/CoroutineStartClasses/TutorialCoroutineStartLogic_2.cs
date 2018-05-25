using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Tutorial/StartLogic/Level2")]
public class TutorialCoroutineStartLogic_2 : TutorialCoroutineStartLogic
{
    public override IEnumerator TutorialStartCoroutine()
    {
        yield return new WaitForEndOfFrame();
        TutorialUtility.SetTimeFreeze(true);
        TutorialUtility.SetTimerUIAsActive(false);

        yield return new WaitForEndOfFrame();

        TutorialUtility.SetBedSanitationFreeze(false);
        TutorialUtility.SetSpawnFreeze(true);
        //TutorialUtility.SetPatientHydration(50);
        //TutorialUtility.SetPatientHealth(50);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(true);
        TutorialUtility.SetFreezeExcretion(true);
    }
}
