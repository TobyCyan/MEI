using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
 * Attach this script to the camera.
 * Remember to set the left and right border x values.
 * A script to make the camera follow the player's movement while limiting the x position.
 */
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float _leftBorderX;
    [SerializeField] private float _rightBorderX;
    
    private readonly float _camYPos = 1.5f;
    private readonly float _camFOV = 60.0f;
    private Vector3 _camSize;
    private PlayerController _player;
    private Camera _cam;

    void Start()
    {
        _player = PlayerController.Instance;
        _cam = GetComponent<Camera>();
        // Cache the camera's original scale to use for resetting later.
        _camSize = transform.localScale;
        ResetCamera();
    }

    void Update()
    {
        UpdateCamPos();
    }

    /** <summary>
        Resets the camera back to its original parameters.
        Called after any sequence where the camera is manipulated is any way.
        </summary>
    */
    public void ResetCamera()
    {
        transform.localScale = _camSize;
        _cam.transform.position = new Vector3(_cam.transform.position.x, _camYPos, _cam.transform.position.z);
        _cam.fieldOfView = _camFOV;
    }

    private void UpdateCamPos()
    {
        if (_player != null)
        {
            Vector3 playerPos = _player.transform.position;
            Vector3 camPos = _cam.transform.position;
            _cam.transform.position = new Vector3(Mathf.Clamp(playerPos.x, _leftBorderX, _rightBorderX),
                                                    camPos.y,
                                                    camPos.z);
        }
    }
}
