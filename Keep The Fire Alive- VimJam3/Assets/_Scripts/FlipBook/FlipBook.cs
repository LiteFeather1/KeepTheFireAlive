using UnityEngine;

public class FlipBook : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private FlipSheet _flipSheet;
    private float _startTime;
    private float MyTime => Time.time - _startTime;
    private int _frame;
    private float _fps;

    [SerializeField] private bool _playing;
    [SerializeField] private bool _looping;

    private void Start()
    {
        if (_playing)
        {
            Play(_flipSheet, _flipSheet.FPS, _looping,true);
        }
    }

    private void Update()
    {
        if (_playing)
        {
            UpdateFrame();
        }
    }

    private void UpdateFrame()
    {
        if (MyTime < _flipSheet.Length / _fps || _looping)
        {
            _frame = (int)(_fps * MyTime) % _flipSheet.Length;
            _sr.sprite = _flipSheet.Sprites[_frame];
        }
        else
        {
            _playing = false;
        }
    }
    /// <summary>
    /// DefaultFps
    /// </summary>
    public void Play(FlipSheet flipSheet, bool looping = false, bool overRide = false)
    {
        Play(flipSheet, _flipSheet.FPS ,looping, overRide);
    }

    public void Play(FlipSheet flipSheet, float fps ,bool looping = false, bool overRide = false)
    {
       if(_flipSheet != flipSheet || overRide)
        {
            _playing = true;
            _flipSheet = flipSheet;
            _fps = fps;
            _looping = looping;
            _startTime = Time.time;
        }
    }
}
