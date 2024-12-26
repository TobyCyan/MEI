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
    [SerializeField] private float _camYPos = 1.6f;

    private PlayerController _player;
    private Camera _cam;

    void Start()
    {
        _player = PlayerController.Instance;
        _cam = GetComponent<Camera>();
        _cam.transform.position = new Vector3(_cam.transform.position.x, _camYPos, _cam.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamPos();
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
