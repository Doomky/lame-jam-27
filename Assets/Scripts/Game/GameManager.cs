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

        [SerializeField]
        private float _soulPlayerDistance = 4f;

        [SerializeField]
        private AudioClip _bgm = null;

        private float _elapsedTime = 0f;

        private float _bossElapsedTime = 0f;

        private float _soulElapsedTime = 0f;

        [SerializeField]
        private AnimationCurve _enemySpawnAnimationCurve;

        [SerializeField]
        private float _maxNumberOfEnemyToSpawn;

        private int _bossSpawnCount = 0;

        [SerializeField]
        private Camera _camera;

        [BoxGroup("Misc")]
        [SerializeField]
        private AudioClip _gameOverSFX = null;

        [BoxGroup("Misc")]
        [SerializeField]
        private AudioClip _victorySFX = null;

        public bool isGameOver = false;
        
        public bool isVictory = false;
        private bool _victoryCoroutineHasStarted;

        public bool IsGameOver => this.isGameOver;

        public bool IsVictory => this.isVictory;

        public override void Bind()
        {
            this._player = Instantiate(this._playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
            this.remainingTimeInSeconds = this._survivalTimerInMinutes * 60;

            Manager.Get<BGMManager>().PlayBGM(this._bgm, true, 0.5f);
        }

        public override void Unbind()
        {
        }

        public void Update()
        {
            if (this.remainingTimeInSeconds <= 0)
            {
                if (!this._victoryCoroutineHasStarted)
                {
                    this.Victory();
                }

                return;
            }

            this.remainingTimeInSeconds -= Time.deltaTime;
            this.remainingTimeInSeconds = Mathf.Max(0, this.remainingTimeInSeconds);

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
                this._bossSpawnCount++;
                for (int i = 0; i < this._bossSpawnCount; i++)
                {
                    Vector3 position = this.GetEdgeRandomScreenPosition();
                    this.spawnBoss(position);
                }

                this._bossElapsedTime = 0f;
            }

            if (this._soulElapsedTime > this._soulSpawnTime && (this._player?.HasAnyEmptySoul() ?? false))
            {
                Vector2 playerPosition = this._player.transform.position;

                Vector2 soulPosition;

                do
                {
                    soulPosition = playerPosition + this._soulPlayerDistance * Random.insideUnitCircle.normalized;
                } while (!this.IsWorldPositionInScreen(soulPosition));
                
                this.spawnSoul(soulPosition);
                this._soulElapsedTime = 0f;
            }
        }

        public void spawnEnemy(Vector3 position)
        {
            int random = 0; 
            if (remainingTimeInSeconds < 240)
            {
                random = UnityEngine.Random.Range(0, 2);
            }
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

        public bool IsWorldPositionInScreen(Vector2 position)
        {
            Vector2 topLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
            Vector2 rightBottom = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            return 
                position.x > topLeft.x && position.x < rightBottom.x &&
                position.y > topLeft.y && position.y < rightBottom.y;
        }

        public void GameOver()
        {
            this.StartCoroutine(this.GameOverCoroutine());
        }

        public void Victory()
        {
            this._victoryCoroutineHasStarted = true;
            this.StartCoroutine(this.VictoryCoroutine());
        }

        private IEnumerator VictoryCoroutine()
        {
            SFXManager sfxManager = Manager.Get<SFXManager>();

            List<Enemy> enemies = Enemy.Enemies;
            int enemiesCount = enemies.Count;
            
            Damage damage = new(999, Color.white);
            
            for (int i = enemiesCount - 1; i >= 0; i--)
            {
                enemies[i].TakeDamage(damage);
                yield return new WaitForSeconds(0.1f);
                if (this._player == null)
                {
                    yield break;
                }
            }

            if (this._player != null)
            {
                this.isVictory = true;
                sfxManager.PlayGlobalSFX(this._victorySFX);

                yield return new WaitForSeconds(3f);

                SceneManager.LoadScene("OutGame", LoadSceneMode.Single);
            }
        }

        private IEnumerator GameOverCoroutine()
        {
            SFXManager sfxManager = Manager.Get<SFXManager>();

            this.isGameOver = true;
            sfxManager.PlayGlobalSFX(this._gameOverSFX);           

            yield return new WaitForSeconds(3);

            SceneManager.LoadScene("OutGame", LoadSceneMode.Single);
        }
    }
}
