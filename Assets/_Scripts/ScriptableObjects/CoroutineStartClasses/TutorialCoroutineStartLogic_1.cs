using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Tutorial/StartLogic/Level1")]
public class TutorialCoroutineStartLogic_1 : TutorialCoroutineStartLogic
{
    public override IEnumerator TutorialStartCoroutine()
    {
        TutorialUtility.SetTimeFreeze(true);
        TutorialUtility.SetTimerUIAsActive(false);

        yield return new WaitForSeconds(0.1f);

        TutorialUtility.SetSpawnFreeze(true);
        TutorialUtility.SetPatientHydration(100);
        TutorialUtility.SetHydrationFreeze(true);
        TutorialUtility.SetHealthFreeze(true);
        TutorialUtility.SetFreezeExcretion(true);
    }
}
