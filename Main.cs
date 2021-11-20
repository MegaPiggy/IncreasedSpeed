using HarmonyLib;
using SALT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace IncreasedSpeed
{
    public class Main : ModEntryPoint
    {
        // THE EXECUTING ASSEMBLY
        public static Assembly execAssembly;

        public static float? BaseSpeed;
        public static float? BaseAccel;
        public static float? BaseBounce;

        // Called before MainScript.Awake
        // You want to register new things and enum values here, as well as do all your harmony patching
        public override void PreLoad()
        {
            // Gets the Assembly being executed
            execAssembly = Assembly.GetExecutingAssembly();
            HarmonyInstance.PatchAll(execAssembly);
        }


        // Called before MainScript.Start
        // Used for registering things that require a loaded gamecontext
        public override void Load()
        {
            UserInputService.Instance.InputBegan += InputBegan;
        }

        // Called after all mods Load's have been called
        // Used for editing existing assets in the game, not a registry step
        public override void PostLoad()
        {

        }

        public void InputBegan(UserInputService.InputObject inputObject, bool wasProcessed)
        {
            if (inputObject.keyCode == KeyCode.Comma)
                PlayerScript.player.maxSpeed -= 1;
            else if (inputObject.keyCode == KeyCode.Period)
                PlayerScript.player.maxSpeed += 1;
            else if (inputObject.keyCode == KeyCode.Semicolon)
                PlayerScript.player.acceleration -= 1;
            else if (inputObject.keyCode == KeyCode.Quote)
                PlayerScript.player.acceleration += 1;
            else if (inputObject.keyCode == KeyCode.Alpha9)
                PlayerScript.player.bounceStrength -= 1;
            else if (inputObject.keyCode == KeyCode.Alpha0)
                PlayerScript.player.bounceStrength += 1;
        }

        [HarmonyPatch(typeof(PlayerScript))]
        [HarmonyPatch("Start")]
        private class Patch_SpeedChange
        {
            public static void Postfix(PlayerScript __instance)
            {
                if (!BaseSpeed.HasValue)
                    BaseSpeed = __instance.maxSpeed;
                if (!BaseAccel.HasValue)
                    BaseAccel = __instance.acceleration;
                if (!BaseBounce.HasValue)
                    BaseBounce = __instance.bounceStrength;
                __instance.maxSpeed = BaseSpeed.Value * 2;
                __instance.acceleration = BaseAccel.Value * 2;
                __instance.bounceStrength = BaseBounce.Value * 1.2f;
            }
        }
    }
}