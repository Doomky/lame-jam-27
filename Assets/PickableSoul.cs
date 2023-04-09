using Framework;
using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PickableSoul : MonoBehaviour
{
    private static HashSet<Soul> _spawnedSouls = new();

    public static HashSet<Soul> SpawnedSouls => _spawnedSouls;

    [SerializeField]
    private List<Soul> _soulList;

    [SerializeField]
    private AudioClip _pickUpSFX;

    [ShowInInspector]
    private Soul _selectedSoul;

    [SerializeField]
    private Timer _lifetime = new(10f);

    public Soul SelectedSoul => _selectedSoul;

    public AudioClip PickupSFX => this._pickUpSFX;
    
    private void Awake()
    {
        foreach (Soul soul in _spawnedSouls)
        {
            this._soulList.Remove(soul);
        }
        
        if (this._soulList.Count == 0)
        {
            GameObject.Destroy(this.gameObject);
            return;
        }
        
        int randomSoul = Random.Range(0, _soulList.Count);

        this._selectedSoul = _soulList[randomSoul];
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PrimaryColor", this._selectedSoul.Color1);
        spriteRenderer.material.SetColor("_SecondaryColor", this._selectedSoul.Color2);

        _spawnedSouls.Add(this._selectedSoul);

        this._lifetime.Reset();
    }

    public void FixedUpdate()
    {
        if (this._lifetime.IsTriggered())
        {
            _spawnedSouls.Remove(this._selectedSoul);
            GameObject.Destroy(this.gameObject);
        }
    }
}
