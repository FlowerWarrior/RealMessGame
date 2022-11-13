using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsMgr : MonoBehaviour
{
    [SerializeField] SC_FPSController playerController;
    [SerializeField] Volume globalVolume;
    [SerializeField] ParticleSystem psMusic;
    [SerializeField] GameObject cupMeshPrefab;
    [SerializeField] Transform propHolder;
    [SerializeField] GameObject waterEffectPlane;
    [SerializeField] ParticleSystem psKnifesSpoon;
    [SerializeField] GameObject lightingParticles;
    [SerializeField] TomatoEffect tomatoEffectMgr;

    internal static EffectsMgr instance;
    internal static System.Action EnableScrabledWords;
    internal static System.Action DisableScrabledWords;
    internal static System.Action WaterRisen;
    internal static System.Action WaterFallen;
    internal static System.Action StartMusics;
    internal static System.Action EndMusics;
    internal static System.Action StartLightning;
    internal static System.Action EndLightning;

    List<GameObject> activeCupMeshes = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        BlinkingScreen.EyesFullyClosed += DisableAllEffects;
    }

    private void OnDisable()
    {
        BlinkingScreen.EyesFullyClosed -= DisableAllEffects;
    }

    private void Update()
    {
        transform.position = playerController.transform.position;
    }

    public void EnableEffect(PropType prop)
    {
        StopCoroutine(DisableEffectAfter(prop, 3));
        switch (prop)
        {
            case PropType.Lightbulb:
                if (globalVolume.profile.TryGet<Bloom>(out Bloom tempDof))
                {
                    tempDof.intensity.value = 4f;
                    tempDof.threshold.value = 0.1f;
                }
                break;
            case PropType.Lamp:
                if (globalVolume.profile.TryGet<Bloom>(out tempDof))
                {
                    tempDof.intensity.value = 4f;
                    tempDof.threshold.value = 0.1f;
                }
                break;
            case PropType.Headphones:
                psMusic.gameObject.SetActive(true);
                StartMusics?.Invoke();
                break;
            case PropType.Shoes:
                playerController.walkingSpeed = 2f;
                playerController.runningSpeed = 2.5f;
                break;
            case PropType.Cap:
                for (int i = 0; i < GameMgr.instance.activeProps.Count; i++)
                {
                    Transform cupPoint = GameMgr.instance.activeProps[i].GetComponent<Prop>().cupPoint;
                    if (cupPoint.parent != propHolder)
                    {
                        GameObject obj = (Instantiate(cupMeshPrefab, cupPoint.position, cupPoint.rotation));
                        obj.transform.localScale = cupPoint.localScale;
                        activeCupMeshes.Add(obj);
                    }
                }
                break;
            case PropType.Cup:
                waterEffectPlane.SetActive(true);
                waterEffectPlane.GetComponent<Animator>().SetInteger("state", 0);
                WaterRisen?.Invoke();
                break;
            case PropType.DrinkCan:
                waterEffectPlane.SetActive(true);
                waterEffectPlane.GetComponent<Animator>().SetInteger("state", 0);
                WaterRisen?.Invoke();
                break;
            case PropType.KnifeForkSpoon:
                psKnifesSpoon.gameObject.SetActive(true);
                break;
            case PropType.Book:
                EnableScrabledWords?.Invoke();
                break;
            case PropType.Medicine:
                if (globalVolume.profile.TryGet<LensDistortion>(out LensDistortion tempDist))
                {
                    tempDist.active = true;
                }
                break;
            case PropType.Hammer:
                lightingParticles.SetActive(true);
                StartLightning?.Invoke();
                break;
            case PropType.Tomato:
                tomatoEffectMgr.EnableTomatoes();
                break;
        }
    }

    public IEnumerator DisableEffectAfter(PropType prop, float sec)
    {
        switch (prop)
        {
            case PropType.Lightbulb:
                yield return new WaitForSeconds(sec);
                if (globalVolume.profile.TryGet<Bloom>(out Bloom tempDof))
                {
                    tempDof.intensity.value = 1f;
                    tempDof.threshold.value = 1f;
                }
                break;
            case PropType.Lamp:
                yield return new WaitForSeconds(sec);
                if (globalVolume.profile.TryGet<Bloom>(out tempDof))
                {
                    tempDof.intensity.value = 1f;
                    tempDof.threshold.value = 1f;
                }
                break;
            case PropType.Headphones:
                yield return new WaitForSeconds(sec);
                psMusic.gameObject.SetActive(false);
                EndMusics?.Invoke();
                break;
            case PropType.Shoes:
                yield return new WaitForSeconds(sec);
                playerController.walkingSpeed = 4f;
                playerController.runningSpeed = 5f;
                break;
            case PropType.Cap:
                yield return new WaitForSeconds(sec);
                for (int i = activeCupMeshes.Count-1; i >= 0; i--)
                {
                    GameObject temp = activeCupMeshes[i];
                    activeCupMeshes.Remove(temp);
                    Destroy(temp);
                }
                break;
            case PropType.Cup:
                yield return new WaitForSeconds(sec);
                waterEffectPlane.GetComponent<Animator>().SetInteger("state", 1);
                print("water clear");
                WaterFallen?.Invoke();
                break;
            case PropType.DrinkCan:
                yield return new WaitForSeconds(sec);
                print("water clear");
                waterEffectPlane.GetComponent<Animator>().SetInteger("state", 1);
                WaterFallen?.Invoke();
                break;
            case PropType.KnifeForkSpoon:
                yield return new WaitForSeconds(sec);
                psKnifesSpoon.gameObject.SetActive(true);
                break;
            case PropType.Book:
                yield return new WaitForSeconds(sec);
                DisableScrabledWords?.Invoke();
                break;
            case PropType.Medicine:
                yield return new WaitForSeconds(sec);
                if (globalVolume.profile.TryGet<LensDistortion>(out LensDistortion tempDist))
                {
                    tempDist.active = false;
                }
                break;
            case PropType.Hammer:
                yield return new WaitForSeconds(0);
                lightingParticles.SetActive(false);
                EndLightning?.Invoke();
                break;
            case PropType.Tomato:
                tomatoEffectMgr.DisableTomatoes();
                break;
        }
    }

    private void DisableAllEffects()
    {
        print("hello");
        for (int i = 0; i < 12; i++)
        {
            DisableEffectAfter(PropType.Book, 0);
            DisableEffectAfter(PropType.Cap, 0);
            DisableEffectAfter(PropType.Cup, 0);
            DisableEffectAfter(PropType.DrinkCan, 0);
            DisableEffectAfter(PropType.Hammer, 0);
            DisableEffectAfter(PropType.Headphones, 0);
            DisableEffectAfter(PropType.KnifeForkSpoon, 0);
            DisableEffectAfter(PropType.Lamp, 0);
            DisableEffectAfter(PropType.Lightbulb, 0);
            DisableEffectAfter(PropType.Medicine, 0);
            DisableEffectAfter(PropType.Shoes, 0);
            DisableEffectAfter(PropType.Tomato, 0);
            //DisableEffectAfter(PropType.Plant, 0);
        }
    }
}
