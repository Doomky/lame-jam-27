using Framework.StateMachine;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;

namespace Game
{
    public abstract class Enemy : Actor, IEnemy
    {
        public static List<Enemy> Enemies = new();

        protected override void Awake()
        {
            base.Awake();

            Enemies.Add(this);
        }

        protected void OnDestroy()
        {
            Enemies.Remove(this);
        }
    }
}
