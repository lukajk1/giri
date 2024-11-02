using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class BlurOnPause : MonoBehaviour
{

    [SerializeField] private Volume volume;

    private UnityEngine.Rendering.Universal.DepthOfField depthOfField;
    void Start()
    {
        volume.profile.TryGet(out depthOfField);
    }
    public void SetBlur(bool isOn)
    {
        depthOfField.active = isOn;
    }
}
