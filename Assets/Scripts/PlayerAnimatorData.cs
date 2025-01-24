using UnityEngine;

public static class PlayerAnimatorData
{
    public static class Params
    {
        public static readonly int isRun = Animator.StringToHash(nameof(isRun));
        public static readonly int isJump = Animator.StringToHash(nameof(isJump));
    }
}
