﻿using System.Linq;
using UnityEngine;

namespace Claws.Modifiers
{
    internal class SaberLength
    {
        Saber _leftSaber;
        Saber _rightSaber;

        internal void ApplyToGameCore(GameObject gameCore)
        {
            _leftSaber = null;
            _rightSaber = null;

            Plugin.Log("Setting up length adjustments...");

            var handControllers = gameCore.transform
                .Find("Origin")
                ?.Find("VRGameCore")
                ?.Find("HandControllers");

            if (handControllers == null)
            {
                Plugin.Log("Couldn't find HandControllers, bailing!");
                return;
            }

            var saberManager = handControllers.GetComponent<SaberManager>();

            _leftSaber = saberManager.GetPrivateField<Saber>("_leftSaber");
            _rightSaber = saberManager.GetPrivateField<Saber>("_rightSaber");

            if (_leftSaber is null || _rightSaber is null)
            {
                Plugin.Log("Sabers couldn't be found. Bailing!");
                return;
            }

            Plugin.Log("Length adjustments ready!");
        }

        internal void SetLength(float length)
        {
            Plugin.Log($"Setting sabers length to {length:0.00}m");

            if (_leftSaber != null)
                SetSaberLength(_leftSaber, length);

            if (_rightSaber != null)
                SetSaberLength(_rightSaber, length);
        }

        static void SetSaberLength(Saber saber, float length)
        {
            var saberTop = saber.GetPrivateField<Transform>("_topPos");
            var saberBottom = saber.GetPrivateField<Transform>("_bottomPos");

            saberTop.localPosition = new Vector3(saberTop.localPosition.x, saberTop.localPosition.y, saberBottom.localPosition.z + length);

            var trail = Resources.FindObjectsOfTypeAll<GameCoreInstaller>().FirstOrDefault()
                ?.GetPrivateField<BasicSaberModelController>("_basicSaberModelControllerPrefab")
                ?.GetPrivateField<SaberWeaponTrail>("_saberWeaponTrail");

            if (trail != null)
            {
                var trailTop = trail.GetPrivateField<Transform>("_pointEnd");
                var trailBottom = trail.GetPrivateField<Transform>("_pointStart");

                trailTop.localPosition = new Vector3(trailTop.localPosition.x, trailTop.localPosition.y, trailBottom.localPosition.z + length);
            }
        }
    }
}
