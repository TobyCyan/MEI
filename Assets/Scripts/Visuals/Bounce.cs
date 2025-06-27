using UnityEngine;

public class Bounce : MonoBehaviour
{
    private Vector3 _initialPos;
    private Vector3 _finalPos;
    private Vector3 _targetPos;
    [SerializeField] private float _speed = 0.35f;
    [SerializeField] private float _posOffset = 0.2f;
    // Delay only happens at the end.
    [SerializeField] private float _delay = 0.0f;
    [SerializeField] private bool _isActive = true;
    [SerializeField] private bool _isBounceUpward = true;
    [SerializeField] private bool _isRectTransform = false;
    [SerializeField] private bool _isDoOnce = false;
    private bool _hasBounced = false;

    private readonly float _epsilon = 0.01f;
    private float _timer = 0.0f;
    private RectTransform _rectTransform;

    void Awake()
    {
        if (_isRectTransform)
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialPos = _rectTransform.anchoredPosition;
        }
        else
        {
            _initialPos = transform.position;

        }
        _finalPos = _initialPos + new Vector3(0.0f, _isBounceUpward? _posOffset : -_posOffset, 0.0f);
        _targetPos = _finalPos;
    }

    void Update()
    {
        if (!_isActive || (_isDoOnce && _hasBounced))
        {
            return;
        }

        if (_isRectTransform)
        {
            DoBounceOnRect();
            return;
        }

        DoBounce();
    }

    public void DoBounce()
    {
        Vector3 currPos = transform.position;
        float difference = (_targetPos - currPos).magnitude;
        if (Mathf.Abs(difference) > _epsilon)
        {
            transform.position = Vector3.MoveTowards(
                currPos,
                _targetPos,
                Time.deltaTime * _speed
                );
            return;
        }

        // Reached target, wait for delay.
        _timer += Time.deltaTime;
        if (_timer >= _delay)
        {
            _timer = 0.0f;
            SwapTarget();
        }
    }

    public void DoBounceOnRect()
    {
        Vector3 currPos = _rectTransform.anchoredPosition;
        float difference = (_targetPos - currPos).magnitude;
        if (Mathf.Abs(difference) > _epsilon)
        {
            _rectTransform.anchoredPosition = Vector3.MoveTowards(
                currPos,
                _targetPos,
                Time.deltaTime * _speed
                );
            return;
        }

        // Reached target, wait for delay.
        _timer += Time.deltaTime;
        if (_timer >= _delay)
        {
            _timer = 0.0f;
            SwapTarget();
        }
    }

    private void SwapTarget()
    {
        if (_targetPos != _initialPos)
        {
            _targetPos = _initialPos;
            return;
        }

        if (_isDoOnce)
        {
            _hasBounced = true;
        }
        _targetPos = _finalPos;
    }

    public void ResetBounce()
    {
        _hasBounced = false;
        _isActive = true;
    }

    public bool HasBounced()
    {
        return _hasBounced;
    }

    public bool IsDoOnce()
    {
        return _isDoOnce;
    }
}
