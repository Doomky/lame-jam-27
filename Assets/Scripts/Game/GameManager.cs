using Framework.Managers.Audio;
using Game;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Managers
{
    public class GameManager : Manager
    {
        [SerializeField]
        private List<GameObject> _enemyPrefab;

        [SerializeField]
        private GameObject _bossPrefab;

        [SerializeField]
        private GameObject _playerPrefab;

        [SerializeField]
        private GameObject _spawnSoulPrefab;

        private Player _player;

        private List<GameObject> _enemyList;

        [SerializeField]
        private List<Soul> _soulListToDrop = null;

        [SerializeField]
        private float _survivalTimerInMinutes;

        public float remainingTimeInSeconds;

        [SerializeField]
        private float _enemySpawnTime;

        [SerializeField]
        private float _bossSpawnTime;

        [SerializeField]
        private float _soulSpawnTime;

        private float _elapsedTime = 0f;

        private float _bossElapsedTime = 0f;

        private float _soulElapsedTime = 0f;

        [SerializeField]
        private AnimationCurve _enemySpawnAnimationCurve;

        [SerializeField]
        private float _maxNumberOfEnemyToSpawn;

        [SerializeField]
        private Camera _camera;

        [BoxGroup("Misc")]
        private AudioClip _gameOverSFX = null;

        public bool isGameOver = false;

        public bool IsGameOver => this.isGameOver;

        public override void Bind()
        {
            this._player = Instantiate(this._playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
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
            this._bossElapsedTime += Time.deltaTime;
            this._soulElapsedTime += Time.deltaTime;

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
            
            if (this._bossElapsedTime > this._bossSpawnTime)
            {
                Vector3 position = this.GetEdgeRandomScreenPosition();
                this.spawnBoss(position);
                this._bossElapsedTime = 0f;
            }

            if (this._soulElapsedTime > this._soulSpawnTime && this._player.HasAnyEmptySoul())
            {
                float spawnY = Random.Range
                (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
                float spawnX = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

                Vector2 spawnPosition = new Vector2(spawnX, spawnY);
                this.spawnSoul(spawnPosition);
                this._soulElapsedTime = 0f;
            }
        }

        public void spawnEnemy(Vector3 position)
        {
            int random = Random.Range(0, this._enemyPrefab.Count);
            GameObject enemy = Instantiate(this._enemyPrefab[random], position, Quaternion.identity);
        }

        public void spawnBoss(Vector3 position)
        {
            GameObject enemy = Instantiate(this._bossPrefab, position, Quaternion.identity);
        }

        public void spawnSoul(Vector3 position)
        {
            GameObject soul = Instantiate(this._spawnSoulPrefab, position, Quaternion.identity);
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

        public void GameOver()
        {
            this.StartCoroutine(this.GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
        {
            SFXManager sfxManager = Manager.Get<SFXManager>();

            this.isGameOver = true;
            sfxManager.PlayGlobalSFX(this._gameOverSFX);           

            yield return new WaitForSeconds(2);

            SceneManager.LoadScene("OutGame", LoadSceneMode.Single);
        }
    }
}
