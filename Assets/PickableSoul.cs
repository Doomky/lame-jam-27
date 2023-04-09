using Framework;
using Game;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PickableSoul : MonoBehaviour
{
    [SerializeField]
    private List<Soul> _soulList;

    [SerializeField]
    private AudioClip _pickUpSFX;

    [ShowInInspector]
    private Soul _selectedSoul;

    public Soul SelectedSoul => _selectedSoul;

    public AudioClip PickupSFX => this._pickUpSFX;
    
    private void Awake()
    {   
        int randomSoul = Random.Range(0, _soulList.Count);

        this._selectedSoul = _soulList[randomSoul];
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PrimaryColor", this._selectedSoul.Color1);
        spriteRenderer.material.SetColor("_SecondaryColor", this._selectedSoul.Color2);
    }
}
