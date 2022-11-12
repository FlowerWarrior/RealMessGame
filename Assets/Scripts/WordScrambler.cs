using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordScrambler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] texts;
    [SerializeField] string[] scrambledTexts;
    List<string> originalTexts = new List<string>();
    TextMeshProUGUI myTxt;

    private void OnEnable()
    {
        EffectsMgr.EnableScrabledWords += EnableScramble;
        EffectsMgr.EnableScrabledWords += DisableScramble;
    }

    private void OnDisable()
    {
        EffectsMgr.EnableScrabledWords -= EnableScramble;
        EffectsMgr.EnableScrabledWords -= DisableScramble;
    }

    private void Start()
    {
        myTxt = GetComponent<TextMeshProUGUI>();
        for (int i = 0; i < texts.Length; i++)
        {
            originalTexts.Add(texts[i].text);
        }
    }

    private void EnableScramble()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = scrambledTexts[i];
        }
    }

    private void DisableScramble()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = originalTexts[i];
        }
    }
}
