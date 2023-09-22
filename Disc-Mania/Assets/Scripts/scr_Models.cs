using System;
using System.Collections.Generic;
using UnityEngine;

public static class scr_Models
{
    #region - Player - 

    public enum PlayerStance
    {
        Stand,
        Crouch,
        Prone
    }

    [Serializable]
    public class PlayerSettingModel 
    {
        [Header("View Settings")]
        public float ViewXSensitivity;
        public float ViewYSensitivity;

        public bool ViewXInveted;
        public bool ViewYInveted;

        [Header("Movement")]
        public float WalkinForwardSpeed;
        public float WalkinStafeSpeed;
        public float WalkinBackwardSpeed;

        [Header("Jumping")]
        public float JumpingHeigt;
        public float JumpingFalloff;
    }

    #endregion
}
