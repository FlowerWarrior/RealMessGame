using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingScreen : MonoBehaviour
{
    internal static System.Action StartBlink;
    internal static System.Action EndBlink;
    internal static System.Action EyesFullyClosed;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        GameMgr.OnTimeForBlink += Blink;
    }

    private void OnDisable()
    {
        GameMgr.OnTimeForBlink -= Blink;
    }

    private void Blink()
    {
        animator.Play("Blink", 0, 0);
        StartBlink?.Invoke();
    }

    public void OnTimeForReshuffle()
    {
        GameMgr.instance.ReshuffleProps();
        EyesFullyClosed?.Invoke();
    }

    public void OnEndedBlinking()
    {
        GameMgr.instance.StartCountdownToBlink();
        EndBlink?.Invoke();
    }
}
