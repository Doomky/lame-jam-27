using Framework.Managers;
using UnityEngine;

namespace Game
{
    public class UI_Victory : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator = null;

        private void Update()
        {
            this._animator.SetBool("IsGameOver", Manager.Get<GameManager>().IsVictory);
        }
    }
}