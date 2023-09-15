using System;
using System.Collections.Generic;
using UnityEngine;

public static class scr_Models
{
    #region

    [Serializable]
    public class PlayerSettingModel 
    {
        [Header("Settings")]
        public float ViewXSensitivity;
        public float ViewYSensitivity;

        public bool ViewXInveted;
        public bool ViewYInveted;

        [Header("Movement")]
        public float WalkinForwardSpeed;
        public float WalkinStafeSpeed;
        public float WalkinBackwardSpeed;
    }

    #endregion
}
