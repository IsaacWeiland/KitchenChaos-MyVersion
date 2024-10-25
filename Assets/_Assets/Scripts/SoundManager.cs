using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SFX_VOLUME = "SoundEffectsVolume";
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefSO audioClipRefSO;
    private float volume = 1f;
    private void Awake()
    {
        Instance = this;
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SFX_VOLUME, 1f);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFail += DeliveryManager_OnRecipeFail;
        CuttingCounter.AnyCut += CuttingCounter_ONAnyCut;
        Player.Instance.OnPickup += InstanceOnOnPickup;
        BaseCounter.AnyPlace += BaseCounterOnAnyPlace;
        TrashCounter.AnyTrash += TrashCounterOnAnyTrash;
    }

    private void TrashCounterOnAnyTrash(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefSO.trash, trashCounter.transform.position);
    }

    private void BaseCounterOnAnyPlace(object sender, EventArgs e)
    {
       BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefSO.objectDrop, baseCounter.transform.position);
        
    }

    private void InstanceOnOnPickup(object sender, EventArgs e)
    {
        PlaySound(audioClipRefSO.pickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_ONAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFail(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefSO.deliveryFail, deliveryCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
    {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefSO.deliverySuc, deliveryCounter.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier);
    } 
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefSO.footSteps, position, volume);
    }
    public void PlayCountdownSound()
    {
        PlaySound(audioClipRefSO.warning, Vector3.zero);
    }

    public void PlayWarningSound(Vector3 position)
    {
        PlaySound(audioClipRefSO.warning, position);
    }

    public void ChangeVolume()
    {
        volume += .1f;
        if (volume > 1f)
        {
            volume = 0f;
        }
        
        PlayerPrefs.SetFloat(PLAYER_PREFS_SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
