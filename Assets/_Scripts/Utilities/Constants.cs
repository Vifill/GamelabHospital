using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Utilities
{
    public static class Constants
    {

        public static string GuidePoints = "Navmesh Guidepoints";
        public static string SceneNamePrefix = "Level";
        public static string MainMenu = "MainMenu";
        public static string ArrowPlacementTag = "ArrowPlacement";
        public static string Highlightable = "Highlightable";

        public class AnimationParameters
        {
            // Character / Patient
            public const string CharacterHoldingBandage = "HoldingBandage";
            public const string CharacterHoldingSyringe = "HoldingSyringe";
            public const string CharacterHoldingSaw = "HoldingSaw";
            public const string CharacterIsWalking = "IsWalking";
            public const string CharacterHoldingWater = "HoldingWater";
            public const string CharacterHoldingIVBag = "HoldingIVBag";
            public const string CharacterHoldingBucket = "HoldingBucket";
            public const string CharacterIsActioning = "Actioning";
            public const string IsPatient = "IsPatient";
            public const string PatientPuke = "PatientPuke";
            public const string PatientDrinkWater = "PatientDrink";
            public const string PatientIVBag = "PatientIVBag";
            public const string PatientPukeWarning = "PatientPukeWarning";
            public const string CharacterDash = "IsDashing";

            // UI
            public const string IsPulsatingUI = "IsPulsatingUI";
            public const string IsPulsatingHydration = "IsPulsatingHydration";
            public const string IsPulsatingHP = "IsPulsatingHP";
            public const string IsPulsating = "IsPulsating";
            public const string GivingPoints = "GivingPoints";

            // Floats
            public const string Speed = "Speed";

            // Environment
            public const string DoorOpen = "DoorOpen";
        }

        public class Colors
        {
            public static Color Blue => GetColor("#004D7DFF");
            public static Color Brown => GetColor("#6C5300FF");
            public static Color Red => GetColor("#B22222");
            public static Color Green => GetColor("#006400");

            public static Color GetColor(string pHexString)
            {
                Color col = Color.magenta;
                ColorUtility.TryParseHtmlString(pHexString, out col);
                return col;
            }
        }
    }
}