using System;
using System.Data;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Animator cameraAnimator;
    public float defaultBlendTime;

    private void Start()
    {
        cameraAnimator.GetComponent<CinemachineStateDrivenCamera>().m_DefaultBlend.m_Time = defaultBlendTime;
    }

    public void SwitchCamera(string stateName)
    {
        try
        {
            cameraAnimator.Play(stateName);
            cameraAnimator.GetComponent<CinemachineStateDrivenCamera>().m_DefaultBlend.m_Time = defaultBlendTime;
        }
        catch
        {
            throw new Exception("@CameraManager.cs: State name " + stateName + " not found!");
        }
    }

    public void SwitchCamera(string stateName, float time)
    {
        try
        {
            cameraAnimator.Play(stateName);
            cameraAnimator.GetComponent<CinemachineStateDrivenCamera>().m_DefaultBlend.m_Time = time;
        }
        catch
        {
            throw new Exception("@CameraManager.cs: State name " + stateName + " not found!");
        }
    }
}