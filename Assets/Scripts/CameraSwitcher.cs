using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera mainCamera;
    public Camera secondCamera;
    public Camera thirdCamera;
    public Camera fourthCamera;
    public Camera fifthCamera;

    void Start()
    {
        // Start with Main Camera active
        SetActiveCamera(mainCamera);
    }

    public void SwitchToMainCamera()
    {
        SetActiveCamera(mainCamera);
    }

    public void SwitchToSecondCamera()
    {
        SetActiveCamera(secondCamera);
    }

    public void SwitchToThirdCamera()
    {
        SetActiveCamera(thirdCamera);
    }

    public void SwitchToFourthCamera()
    {
        SetActiveCamera(fourthCamera);
    }

    public void SwitchToFifthCamera()
    {
        SetActiveCamera(fifthCamera);
    }

    private void SetActiveCamera(Camera targetCamera)
    {
        // Deactivate all cameras first
        if (mainCamera != null) mainCamera.gameObject.SetActive(false);
        if (secondCamera != null) secondCamera.gameObject.SetActive(false);
        if (thirdCamera != null) thirdCamera.gameObject.SetActive(false);
        if (fourthCamera != null) fourthCamera.gameObject.SetActive(false);
        if (fifthCamera != null) fifthCamera.gameObject.SetActive(false);

        // Activate the target one
        if (targetCamera != null)
        {
            targetCamera.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Target camera is not assigned!");
        }
    }
}
