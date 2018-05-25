using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Tutorial/StartLogic/Level1")]
public class TutorialCoroutineStartLogic_1 : TutorialCoroutineStartLogic
{
    public override IEnumerator TutorialStartCoroutine()
    {
        yield return new WaitForEndOfFrame();

        TutorialUtility.SetSpawnFreeze(true);
        TutorialUtility.SetTimeFreeze(true);
        TutorialUtility.SetActionablesActive(false);
        TutorialUtility.SetTimerUIAsActive(false);

        //TutorialUtility.SetPatientHydration(100);
        //TutorialUtility.SetPatientHealth(80);
        //TutorialUtility.SetHydrationFreeze(true);
        //TutorialUtility.SetHealthFreeze(true);
        //TutorialUtility.SetFreezeExcretion(true);
    }
}
