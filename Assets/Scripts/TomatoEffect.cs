using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoEffect : MonoBehaviour
{
    [SerializeField] GameObject tomatoPrefab;
    [SerializeField] float frequency;

    internal static System.Action TomatoHitEffect;

    public void EnableTomatoes()
    {
        StartCoroutine(SpawnTomato());
    }

    public void DisableTomatoes()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnTomato()
    {
        yield return new WaitForSeconds(1f / frequency);
        Instantiate(tomatoPrefab, transform);
        float x = Random.Range(-572.88f, 572.88f);
        float y = Random.Range(-284.72f, 284.72f);
        tomatoPrefab.GetComponent<RectTransform>().localPosition = new Vector2(x, y);
        StartCoroutine(SpawnTomato());
    }
}
