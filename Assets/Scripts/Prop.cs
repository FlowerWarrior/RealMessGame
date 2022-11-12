using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    [SerializeField] internal PropType propType;
    [SerializeField] internal SpawnType spawnType;
    [SerializeField] internal Outline myOutline;
    [SerializeField] internal Transform myMeshT;
    [SerializeField] MeshFilter regularFilter;
    [SerializeField] MeshFilter glitchedFilter;
    [SerializeField] MeshRenderer regularRenderer;
    [SerializeField] MeshRenderer glitchedRenderer;
    [SerializeField] MeshCollider myMeshCollider;
    [SerializeField] Color32 colOutlineGreen;
    [SerializeField] Color32 colOutlineRed;
    [SerializeField] internal Transform cupPoint;

    internal Vector3 meshOriginalRelPos;
    bool isOutlineOverwrite = false;
    [SerializeField] internal bool isTarget = false;

    // Start is called before the first frame update
    void Start()
    {
        myMeshCollider.sharedMesh = glitchedFilter.mesh;
        meshOriginalRelPos = myMeshT.position - transform.position;
        if (isTarget)
        {
            glitchedRenderer.enabled = true;
            regularRenderer.enabled = false;
        }
        else
        {
            regularRenderer.enabled = true;
            glitchedRenderer.enabled = false;
        }
    }

    void UpdateOutline(Outline activeOutline)
    {
        if (isOutlineOverwrite)
            return;

        if (activeOutline == myOutline)
        {
            myOutline.enabled = true;
        }
        else
        {
            myOutline.enabled = false;
        }
    }

    private void OnEnable()
    {
        SC_Interactor.UpdateOutlines += UpdateOutline;
    }

    private void OnDisable()
    {
        SC_Interactor.UpdateOutlines -= UpdateOutline;
    }

    public void ForceShowGreenOutline()
    {
        isOutlineOverwrite = true;
        myOutline.enabled = true;
        myOutline.OutlineColor = colOutlineGreen;
    }

    public void ForceShowRedOutline()
    {
        isOutlineOverwrite = true;
        myOutline.enabled = true;
        myOutline.OutlineColor = colOutlineRed;
    }

    public void CancelOulineOverwrite()
    {
        isOutlineOverwrite = false;
        myOutline.OutlineColor = Color.white;
    }
}

public enum PropType
{
    Cup,
    Plant,
    Monkey,
    Shoes,
    Hammer,
    Lightbulb,
    Medicine,
    DrinkCan,
    Book,
    Headphones,
    Cap,
    Lamp,
    KnifeForkSpoon,
    Tomato,
}