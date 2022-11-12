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
    [SerializeField] GameObject waterEffectPlane;
    [SerializeField] ParticleSystem psKnifesSpoon;
    [SerializeField] GameObject lightingParticles;

    internal static EffectsMgr instance;
    internal static System.Action EnableScrabledWords;
    internal static System.Action DisableScrabledWords;

    List<GameObject> activeCupMeshes = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        transform.position = playerController.transform.position;
    }

    public void EnableEffect(PropType prop)
    {
        switch (prop)
        {
            case PropType.Lightbulb:
                if (globalVolume.profile.TryGet<Bloom>(out Bloom tempDof))
                {
                    tempDof.intensity.value = 3f;
                    tempDof.threshold.value = 0.1f;
                }
                break;
            case PropType.Lamp:
                if (globalVolume.profile.TryGet<Bloom>(out tempDof))
                {
                    tempDof.intensity.value = 3f;
                    tempDof.threshold.value = 0.1f;
                }
                break;
            case PropType.Headphones:
                psMusic.gameObject.SetActive(true);
                break;
            case PropType.Shoes:
                playerController.walkingSpeed = 2f;
                playerController.walkingSpeed = 2.5f;
                break;
            case PropType.Cap:
                for (int i = 0; i < GameMgr.instance.activeProps.Count; i++)
                {
                    Transform cupPoint = GameMgr.instance.activeProps[i].GetComponent<Prop>().cupPoint;
                    GameObject obj = (Instantiate(cupMeshPrefab, cupPoint.position, cupPoint.rotation));
                    obj.transform.localScale = cupPoint.localScale;
                    activeCupMeshes.Add(obj);
                }
                break;
            case PropType.Cup:
                waterEffectPlane.SetActive(true);
                waterEffectPlane.GetComponent<Animator>().SetInteger("state", 0);
                break;
            case PropType.DrinkCan:
                waterEffectPlane.SetActive(true);
                waterEffectPlane.GetComponent<Animator>().SetInteger("state", 0);
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
                break;
            case PropType.Shoes:
                yield return new WaitForSeconds(sec);
                playerController.walkingSpeed = 4f;
                playerController.walkingSpeed = 5f;
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
                break;
            case PropType.DrinkCan:
                yield return new WaitForSeconds(sec);
                waterEffectPlane.GetComponent<Animator>().SetInteger("state", 1);
                break;
            case PropType.KnifeForkSpoon:
                yield return new WaitForSeconds(sec);
                psKnifesSpoon.gameObject.SetActive(true);
                break;
            case PropType.Book:
                yield return new WaitForSeconds(sec);
                EnableScrabledWords?.Invoke();
                break;
            case PropType.Medicine:
                yield return new WaitForSeconds(sec);
                if (globalVolume.profile.TryGet<LensDistortion>(out LensDistortion tempDist))
                {
                    tempDist.active = true;
                }
                break;
            case PropType.Hammer:
                yield return new WaitForSeconds(0);
                lightingParticles.SetActive(false);
                break;
        }
    }
}
