using HurricaneVR.Framework.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spoon : MonoBehaviour
{
    // Start is called before the first frame update

    bool spoonWatered;
    public MeshRenderer spoonWaterMeshRenderer;
    AudioHandler audioHandler;
    HVRGrabbable grabbable;
    public UnityEvent SteamStart;

    private void Start()
    {
        grabbable = GetComponentInParent<HVRGrabbable>();

        audioHandler = GetComponentInParent<AudioHandler>();
        SteamStart.AddListener(StartFog);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!spoonWatered && other.CompareTag("WaterBox")) // && other.TryGetComponent(out VegemiteInJar vegemiteInJar))
        {
            audioHandler.Play("WaterSound");
            spoonWatered = true;
            spoonWaterMeshRenderer.enabled = true;
            other.gameObject.SetActive(false);
        }
        else if (spoonWatered && other.CompareTag("StoneBox")) //&& other.TryGetComponent(out BreadMealPart breadMealPart))
        {

            SteamStart.Invoke();
        }
    }

    void StartFog()
    {
        audioHandler.Play("SteamSound");
        spoonWatered = false;
        spoonWaterMeshRenderer.enabled = false;

        grabbable.ForceRelease();
        grabbable.enabled = false;
        //You can put the stuff here or in the unity editor
    }
}
