namespace Game
{
    public static class ActorAnimatorParamName
    {
        public enum AnimationType
        {
            Idle,
            Moving,
            GettingHit,
            Dying,
            Attacking
        }

        public const string IS_MOVING = "IsMoving";
        public const string IS_GETTING_HIT = "IsGettingHit";
        public const string IS_DEAD = "IsDead";
        public const string IS_ATTACKING = "IsAttacking";

        public const string ATTACK_SPEED = "AttackSpeed";
    }
}
