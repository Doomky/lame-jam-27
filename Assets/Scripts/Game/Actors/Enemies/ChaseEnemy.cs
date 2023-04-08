using UnityEngine;

namespace Game
{
    public class ChaseEnemy : Enemy
    {
        [SerializeField]
        private Player _player = null;

        [SerializeField]
        private AnimationCurve _movementSpeedCurve = null;

        [SerializeField]
        private float _randomOffset = 1;

        private int _indexOffset = 0;

        protected override void Awake()
        {
            base.Awake();
            this._indexOffset = UnityEngine.Random.Range(0, 10);
            this._player = FindObjectOfType<Player>();
        }

        public void FixedUpdate()
        {
            if (this._player == null || this._isDead)
            {
                return;
            }

            Vector2 direction = (this._randomOffset * Random.insideUnitCircle.normalized + (Vector2)this._player.transform.position) - (Vector2)this.transform.position;
            direction.Normalize();
            direction *= this._movementSpeedCurve.Evaluate(Time.time + this._indexOffset * 0.8f);

            //Vector2 rotationDirection = (Vector2)this._player.transform.position - (Vector2)this.transform.position;
            //this.transform.rotation = Quaternion.FromToRotation(Vector2.right, rotationDirection);

            this.Move(direction);
        }
    }
}
