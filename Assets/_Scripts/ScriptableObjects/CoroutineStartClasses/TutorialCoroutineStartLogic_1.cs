using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Tutorial/StartLogic/Level1")]
public class TutorialCoroutineStartLogic_1 : TutorialCoroutineStartLogic
{
    public override IEnumerator TutorialStartCoroutine()
    {
        TutorialUtility.SetTimeFreeze(true);

        yield return new WaitForSeconds(0.1f);

        TutorialUtility.SetActionablesActive(false);
        TutorialUtility.SetTimerUIAsActive(false);
        TutorialUtility.SetSpawnFreeze(true);
        TutorialUtility.SetPatientHydration(100);
        TutorialUtility.SetPatientHealth(80);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(true);
        TutorialUtility.SetFreezeExcretion(true);
    }
}
