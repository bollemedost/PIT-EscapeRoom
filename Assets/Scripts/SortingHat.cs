using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SortingHat : MonoBehaviour
{
    public AudioClip[] houseSounds;
    private AudioSource audioSource;
    private bool hasBeenClicked = false;

    public float maxRayDistance = 10f;
    public Transform rayOrigin;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller, devices);

        foreach (var device in devices)
        {
            if (device.TryGetFeatureValue(CommonUsages.triggerButton, out bool pressed) && pressed)
            {
                Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, maxRayDistance))
                {
                    if (hit.collider.GetComponent<SortingHat>() == this)
                    {
                        OnRayClick();
                    }
                }
            }
        }
    }

    public void OnRayClick()
    {
        if (hasBeenClicked) return;

        hasBeenClicked = true;
        int index = Random.Range(0, houseSounds.Length);
        audioSource.PlayOneShot(houseSounds[index]);
    }
}
