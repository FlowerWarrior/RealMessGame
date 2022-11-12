using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMgr : MonoBehaviour
{
    [SerializeField] GameObject audioSourcePrefab;
    [SerializeField] AudioSource ambientAudioSource;
    [SerializeField] AudioSource musicEffectSource;
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
        ambientAudioSource.clip = ambientAudioClips[ambientCycle];
    }

    private void OnEnable()
    {
        SC_FPSController.Footstep += PlayFootstep;
        SC_FPSController.Jumped += PlayJump;
        SC_FPSController.PickedUp += PlayPickUp;
        SC_FPSController.PutDown += PlayPutDown;
        GameMgr.PickSuccess += PlayPickSuccess;
        GameMgr.PickFailed += PlayPickFail;
        EffectsMgr.WaterRisen += PlayWaterRise;
        EffectsMgr.WaterFallen += PlayWaterFall;
        EffectsMgr.EnableScrabledWords += PlayWordsScramble;
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
        PlayAudioAtPoint(footsteps[Random.Range(0, footsteps.Length)], location);
    }
    private void PlayJump()
    {
        PlayAudioEffect(jump);
    }
    private void PlayPickUp()
    {
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
}
