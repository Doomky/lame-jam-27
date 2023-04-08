using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Framework.Managers
{
    public class GameManager : Manager
    {
        [SerializeField]
        private List<GameObject> _enemyPrefab;

        [SerializeField]
        private GameObject _playerPrefab;

        private List<GameObject> _enemyList;

        [SerializeField]
        private float _survivalTimerInMinutes;

        public float remainingTimeInSeconds;

        [SerializeField]
        private float _enemySpawnTime;

        private float _elapsedTime = 0f;

        [SerializeField]
        private AnimationCurve _enemySpawnAnimationCurve;

        [SerializeField]
        private float _maxNumberOfEnemyToSpawn;

        [SerializeField]
        private Camera _camera;

        public override void Bind()
        {
            Instantiate(this._playerPrefab, Vector3.zero, Quaternion.identity);
            this.remainingTimeInSeconds = this._survivalTimerInMinutes * 60;
        }

        public override void Unbind()
        {
        }

        public void Update()
        {
            this.remainingTimeInSeconds -= Time.deltaTime;

            if (this.remainingTimeInSeconds <= 0)
            {
                Debug.Log("t'as gagn� bogoss");
                // TODO: Game Over
            }

            this._elapsedTime += Time.deltaTime;

            if (this._elapsedTime > this._enemySpawnTime)
            {
                
                float indexOfCurve = (this._survivalTimerInMinutes * 60 - remainingTimeInSeconds) / (this._survivalTimerInMinutes * 60);
                float numberOfEnemyToSpawn = _enemySpawnAnimationCurve.Evaluate(indexOfCurve) * this._maxNumberOfEnemyToSpawn;

                Vector3 position;
                for (int i = 0; i < numberOfEnemyToSpawn; i++)
                {
                    position = this.GetEdgeRandomScreenPosition();
                    this.spawnEnemy(position);
                }

                this._elapsedTime = 0f;
            }
        }

        public void spawnEnemy(Vector3 position)
        {
            int random = UnityEngine.Random.Range(0, this._enemyPrefab.Count);
            GameObject enemy = Instantiate(this._enemyPrefab[random], position, Quaternion.identity);
        }

        public Vector3 GetEdgeRandomScreenPosition()
        {
            int random = UnityEngine.Random.Range(0, 4);
            float dist = (transform.position - Camera.main.transform.position).z;
            // 0 = left, 1 = top, 2 = right, 3 = bottom
            switch (random)
            {   
                case 0:
                    return Camera.main.ViewportToWorldPoint(new Vector3(0, UnityEngine.Random.Range(0.0f, 1.0f), dist));
                case 1:
                    return Camera.main.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), 1, dist));
                case 2:
                    return Camera.main.ViewportToWorldPoint(new Vector3(1, UnityEngine.Random.Range(0.0f, 1.0f), dist));
                case 3:
                    return Camera.main.ViewportToWorldPoint(new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), 0, dist));
                default:
                    return Vector3.zero;
            }
        }
    }
}
