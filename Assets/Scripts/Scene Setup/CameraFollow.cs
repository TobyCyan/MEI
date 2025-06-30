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
    
    private static readonly float CAM_Y_POS = 1.5f;
    private static readonly float CAM_Z_POS = -10.0f;
    private static readonly float CAM_FOV = 60.0f;
    private static readonly float CAM_SIZE = 5.0f;
    private static readonly float CAM_SPEED = 3.0f;
    private Vector3 _camScale;
    private PlayerController _player;
    private Camera _cam;
    private bool _isActive = true;

    void Start()
    {
        _player = PlayerController.Instance;
        _cam = GetComponent<Camera>();
        // Cache the camera's original scale to use for resetting later.
        _camScale = transform.localScale;
        ResetCamera();
    }

    void Update()
    {
        if (!_isActive)
        {
            return;
        }
        UpdateCamPos();
    }

    /** <summary>
        Resets the camera back to its original parameters.
        Called after any sequence where the camera is manipulated is any way.
        </summary>
    */
    public void ResetCamera()
    {
        _isActive = false;
        transform.localScale = _camScale;
        _cam.transform.position = GetClampedXPos();
        _cam.fieldOfView = CAM_FOV;
        _cam.orthographicSize = CAM_SIZE;
        _isActive = true;
    }

    private void UpdateCamPos()
    {
        if (_player != null)
        {
            Vector3 targetPos = GetClampedXPos();
            _cam.transform.position = Vector3.Lerp(
                _cam.transform.position, 
                targetPos, 
                CAM_SPEED * Time.deltaTime
                );
        }
    }

    public void FreezeCameraToPos(Vector3 freezePosition)
    {
        _isActive = false;
        transform.position = freezePosition;
    }

    private Vector3 GetClampedXPos()
    {
        Vector3 playerPos = _player.transform.position;
        Vector3 camPos = _cam.transform.position;
        return new (Mathf.Clamp(playerPos.x, _leftBorderX, _rightBorderX),
                                                    CAM_Y_POS,
                                                    CAM_Z_POS);
    }
}
