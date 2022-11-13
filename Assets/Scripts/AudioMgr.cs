using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    [SerializeField] GameObject audioSourcePrefab;
    [SerializeField] AudioSource ambientAudioSource;
    [SerializeField] AudioSource musicEffectSource;
    [SerializeField] AudioSource lightingSource;
    [Header("Audio Sounds")]
    [SerializeField] AudioClip[] footsteps;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip pickup;
    [SerializeField] AudioClip putdown;
    [SerializeField] AudioClip pickSuccess;
    [SerializeField] AudioClip pickFail;
    [SerializeField] AudioClip[] ambientAudioClips;
    [SerializeField] AudioClip waterRise;
    [SerializeField] AudioClip waterFall;
    [SerializeField] AudioClip wordsScramble;
    [SerializeField] AudioClip tomatoHit;
    [SerializeField] AudioClip startBlink;
    [SerializeField] AudioClip endBlink;
    [SerializeField] AudioClip onEyesFullyClosed;

    bool soundsEnabled = true;

    internal static AudioMgr instance;

    private void Awake()
    {
        instance = this;
        ambientAudioSource.clip = ambientAudioClips[0];
        ambientAudioSource.Play();
    }

    int ambientCycle = 0;
    public void CycleThroughAmbientAudio()
    {
        ambientCycle++;
        if (ambientCycle >= ambientAudioClips.Length)
            ambientCycle = 0;
        ambientAudioSource.clip = ambientAudioClips[ambientCycle];
    }

    private void OnEnable()
    {
        SC_FPSController.Footstep += PlayFootstep;
        SC_FPSController.Jumped += PlayJump;
        SC_Interactor.PickedUp += PlayPickUp;
        SC_Interactor.PutDown += PlayPutDown;
        GameMgr.PickSuccess += PlayPickSuccess;
        GameMgr.PickFailed += PlayPickFail;
        EffectsMgr.WaterRisen += PlayWaterRise;
        EffectsMgr.WaterFallen += PlayWaterFall;
        EffectsMgr.EnableScrabledWords += PlayWordsScramble;
        EffectsMgr.StartMusics += EnableMusicsEffect;
        EffectsMgr.EndMusics += DisableMusicsEffect;
        TomatoEffect.TomatoHitEffect += PlayTomatoHit;
        BlinkingScreen.StartBlink += PlayStartBlink;
        BlinkingScreen.EndBlink += PlayEndBlink;
        BlinkingScreen.EyesFullyClosed += PlayEyesFullyClosed;
        EffectsMgr.StartLightning += EnableLightning;
        EffectsMgr.EndLightning += DisableLightning;
    }

    private void PlayAudioAtPoint(AudioClip clip, Vector3 location)
    {
        if (!soundsEnabled)
            return;

        if (clip == null)
            return;

        AudioSource.PlayClipAtPoint(clip, location);
    }

    private void PlayAudioEffect(AudioClip clip)
    {
        if (!soundsEnabled)
            return;

        if (clip == null)
            return;

        GameObject obj = Instantiate(audioSourcePrefab);
        obj.GetComponent<AudioSource>().clip = clip;
        obj.GetComponent<audioSourceTemp>().MyStart();
    }

    private void PlayFootstep(Vector3 location)
    {
        PlayAudioEffect(footsteps[Random.Range(0, footsteps.Length)]);
    }
    private void PlayJump()
    {
        PlayAudioEffect(jump);
    }
    private void PlayPickUp()
    {
        print("pciked");
        PlayAudioEffect(pickup);
    }
    private void PlayPutDown()
    {
        PlayAudioEffect(putdown);
    }
    private void PlayPickSuccess()
    {
        PlayAudioEffect(pickSuccess);
    }
    private void PlayPickFail()
    {
        PlayAudioEffect(pickFail);
    }
    private void PlayWaterRise()
    {
        PlayAudioEffect(waterRise);
    }
    private void PlayWaterFall()
    {
        PlayAudioEffect(waterFall);
    }

    private void PlayWordsScramble()
    {
        PlayAudioEffect(wordsScramble);
    }

    private void EnableMusicsEffect()
    {
        musicEffectSource.enabled = true;
    }

    private void DisableMusicsEffect()
    {
        musicEffectSource.enabled = false;
    }

    private void PlayTomatoHit()
    {
        PlayAudioEffect(tomatoHit);
    }

    private void PlayStartBlink()
    {
        PlayAudioEffect(startBlink);
    }

    private void PlayEndBlink()
    {
        PlayAudioEffect(endBlink);
    }

    private void PlayEyesFullyClosed()
    {
        PlayAudioEffect(onEyesFullyClosed);
    }

    private void EnableLightning()
    {
        lightingSource.enabled = true;
    }

    private void DisableLightning()
    {
        lightingSource.enabled = false;
    }
}
