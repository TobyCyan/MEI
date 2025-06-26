using UnityEngine;

public class Bounce : MonoBehaviour
{
    private Vector3 _initialPos;
    private Vector3 _topPos;
    private Vector3 _targetPos;
    private readonly float _speed = 0.35f;
    private readonly float _posOffset = 0.2f;
    private readonly float _epsilon = 0.01f;

    void Awake()
    {
        _initialPos = transform.position;
        _topPos = _initialPos + new Vector3(0.0f, _posOffset, 0.0f);
        _targetPos = _topPos;
    }

    // Update is called once per frame
    void Update()
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
        }
        else
        {
            SwapTarget();
        }
    }

    private void SwapTarget()
    {
        if (_targetPos == _initialPos)
        {
            _targetPos = _topPos;
        }
        else
        {
            _targetPos = _initialPos;
        }
    }
}
