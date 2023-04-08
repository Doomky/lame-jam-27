using Framework.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Timer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _timerLabel;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        this._gameManager = Manager.Get<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        int seconds = (int)this._gameManager.remainingTimeInSeconds;
        int minutes = seconds / 60;
        _timerLabel.text = minutes.ToString("00") + ":" + (seconds % 60).ToString("00");
    }
}
