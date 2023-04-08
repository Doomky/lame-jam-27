using Framework;
using Framework.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Android;

namespace Game
{
    public class Game : Game<Game.StateMachine, Game.Action>
    {
        [SerializeField]
        private GameObject _enemyPrefab;

        [SerializeField]
        private GameObject _playerPrefab;

        [SerializeField]
        private List<GameObject> _enemyList;

        [SerializeField]
        private float _survivalTimerInMinutes;

        [SerializeField]
        private float _enemySpawnTime;

        private float _elapsedTime = 0f;

        [SerializeField]
        private Camera _camera;

        private StateMachine _machine;

        public enum Action
        {
            EnterInGame     = 1 << 0,
            ExitFromGame    = 1 << 1,
        }

        public new class StateMachine : GameStateMachine<Action>
        {
            private GameState _inGameState = new();
            
            private GameState _outGameState = new();

            public StateMachine() : base(null)
            {
            }

            protected override bool TryGetNextState(Action action, out GameState nextState)
            {
                switch (action)
                {
                    case Action.EnterInGame:
                        {
                            nextState = _inGameState;
                            return true;
                        }
                        
                    case Action.ExitFromGame:
                        {
                            nextState = _outGameState;
                            return true;
                        }

                    default:
                        {
                            nextState = null;
                            return false;
                        }
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            this._machine = new StateMachine();
            Instantiate(this._playerPrefab, Vector3.zero, Quaternion.identity);
        }

        public void Update()
        {
            this._survivalTimerInMinutes -= Time.deltaTime;
            if (this._survivalTimerInMinutes <= 0)
            {
                Debug.Log("t'as gagné bogoss");
                // TODO: Game Over
            }
            this._elapsedTime += Time.deltaTime;
            if (this._elapsedTime > this._enemySpawnTime)
            {
                Vector3 position = this.GetEdgeRandomScreenPosition();
                this.spawnEnemy(position);
                this._elapsedTime = 0f;
            }
        }

        public void spawnEnemy(Vector3 position)
        {
            GameObject enemy = Instantiate(this._enemyPrefab, position, Quaternion.identity);
            _enemyList.Add(enemy);
        }

        public Vector3 GetEdgeRandomScreenPosition()
        {
            int random = UnityEngine.Random.Range(0, 4);

            // 0 = left, 1 = top, 2 = right, 3 = bottom
            switch (random)
            {
                case 0:
                    return new Vector3(-_camera.orthographicSize * _camera.aspect, UnityEngine.Random.Range(-_camera.orthographicSize, _camera.orthographicSize), 0);
                case 1:
                    return new Vector3(UnityEngine.Random.Range(-_camera.orthographicSize * _camera.aspect, _camera.orthographicSize * _camera.aspect), _camera.orthographicSize, 0);
                case 2:
                    return new Vector3(_camera.orthographicSize * _camera.aspect, UnityEngine.Random.Range(-_camera.orthographicSize, _camera.orthographicSize), 0);
                case 3:
                    return new Vector3(UnityEngine.Random.Range(-_camera.orthographicSize * _camera.aspect, _camera.orthographicSize * _camera.aspect), -_camera.orthographicSize, 0);
                default:
                    return Vector3.zero;
            }
        }
    }
}
