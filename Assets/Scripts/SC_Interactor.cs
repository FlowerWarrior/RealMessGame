using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Interactor : MonoBehaviour
{
    [SerializeField] SC_FPSController scFpsController;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask raycastLayer;
    [SerializeField] float rayDistance;
    [SerializeField] Transform propHolder;
    [SerializeField] Vector3 heldPropScale;

    internal static System.Action<Outline> UpdateOutlines;
    internal static System.Action<bool> LevelEnded;
    internal static System.Action PickedUp;
    internal static System.Action PutDown;

    internal Prop activeProp;
    bool isHoldingProp = false;
    bool inputsActive = true;
    int chancesLeft = 3;

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (!isHoldingProp)
        {
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, rayDistance, raycastLayer))
            {
                activeProp = hit.collider.gameObject.transform.parent.gameObject.GetComponent<Prop>();
            }
            else
            {
                activeProp = null;
            }
        }
        
        if (activeProp != null)
        {
            if (!isHoldingProp)
            {
                UpdateOutlines?.Invoke(activeProp.myOutline);
            }
            else
            {
                UpdateOutlines?.Invoke(null);
            }
            
            if (Input.GetButtonDown("Interact") && inputsActive)
            {
                if (!isHoldingProp)
                {
                    EnterHoldingProp();
                    EffectsMgr.instance.EnableEffect(activeProp.propType);
                    PickedUp?.Invoke();
                }
                else
                {
                    ExitHoldingProp();
                    EffectsMgr.instance.StartCoroutine(EffectsMgr.instance.DisableEffectAfter(activeProp.propType, 3));
                    PutDown?.Invoke();
                }
            }

            if (Input.GetButtonDown("PickHeldProp") && isHoldingProp && inputsActive)
            {
                if (activeProp.isTarget)
                {
                    activeProp.ForceShowGreenOutline();
                    GameMgr.instance.ShowTextSuccess();
                    LevelEnded?.Invoke(true);
                    inputsActive = false;
                }
                else if (chancesLeft > 1)
                {
                    chancesLeft--;
                    activeProp.ForceShowRedOutline();
                    GameMgr.instance.ShowTextFail();
                    GameMgr.instance.StartCoroutine(GameMgr.instance.ShowTextChances(chancesLeft));
                    inputsActive = true;
                }
                else
                {
                    activeProp.ForceShowRedOutline();
                    GameMgr.instance.ShowTextFail();
                    LevelEnded?.Invoke(false);
                    inputsActive = false;
                }
            }
        }
        else
        {
            UpdateOutlines?.Invoke(null);
        }

        if (isHoldingProp && inputsActive)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector3 rotVector;
            rotVector.y = -mouseX;
            rotVector.x = mouseY;
            rotVector.z = 0;

            //Vector3 currentRot = activeProp.myMeshT.rotation.eulerAngles;
            //activeProp.myMeshT.rotation = Quaternion.Euler(currentRot + rotVector);
            activeProp.myMeshT.RotateAround(activeProp.myMeshT.position, propHolder.up, -mouseX);
            activeProp.myMeshT.RotateAround(activeProp.myMeshT.position, propHolder.right, mouseY);
        }
    }

    private void EnterHoldingProp()
    {
        isHoldingProp = true;
        activeProp.myMeshT.position = propHolder.position;
        activeProp.myMeshT.parent = propHolder;
        activeProp.myMeshT.localScale = heldPropScale;
        scFpsController.DisableControls();
        GameMgr.instance.ToggleHoldPropUI(true);
    }

    private void ExitHoldingProp()
    {
        if (!isHoldingProp)
            return;

        isHoldingProp = false;
        scFpsController.EnableControls();
        GameMgr.instance.ToggleHoldPropUI(false);

        if (activeProp == null)
        {
            return;
        }
        
        activeProp.myMeshT.position = activeProp.transform.position + activeProp.meshOriginalRelPos;
        activeProp.myMeshT.rotation = activeProp.transform.rotation;
        activeProp.myMeshT.parent = activeProp.transform;
        activeProp.myMeshT.localScale = new Vector3(1f, 1f, 1f);
        activeProp.CancelOulineOverwrite();
    }

    private void OnEnable()
    {
        GameMgr.PropsReshuffled += ExitHoldingProp;
    }

    private void OnDisable()
    {
        GameMgr.PropsReshuffled -= ExitHoldingProp;
    }
}
