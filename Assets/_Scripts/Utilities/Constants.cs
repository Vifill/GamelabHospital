using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Utilities
{
    public static class Constants
    {
        public static string GuidePoints = "Navmesh Guidepoints";
        public static string SceneNamePrefix = "Midterms_Level";
        public static string MainMenu = "MainMenu";
        public static string ArrowPlacementTag = "ArrowPlacement";

        public static AnimationParameters AnimationParameters;
    }

    public class AnimationParameters
    {
        public const string CharacterIsWalking = "IsWalking";
        public const string CharacterHoldingWater = "HoldingWater";
        public const string CharacterHoldingIVBag = "HoldingIVBag";
        public const string IsPatient = "IsPatient";
        public const string PatientPuke = "PatientPuke";
        public const string PatientDrinkWater = "PatientDrink";
        public const string PatientIVBag = "PatientIVBag";
    }
}
