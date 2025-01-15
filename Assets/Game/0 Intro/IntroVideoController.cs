using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class IntroVideoController : MonoBehaviour
{
    public VideoPlayer _player;
    //public Material _screen;
    //public Color _screenColorWhite = Color.white;
    //public Color _screenColorBlack = Color.black;
    public AudioSource _audioSource;
    public AudioClip _audioClip;
    public TMP_Text _text;
    public float _timer;
    public Material _screen;
    public Color _screenColorWhite = Color.white;
    public Color _screenColorBlack = Color.black;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _screen.color = _screenColorBlack;
        _player.Prepare();
        _text.text = "�������� ����� �������� ����";
        _timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer >= 0f)
        {
            _timer += Time.deltaTime;
            if (_timer > 1f)
            {
                _timer = 0f;
                if (_text.text.Length < 33)
                {
                    _text.text = _text.text + ".";
                }
                else
                {
                    _text.text = "�������� ����� �������� ����";
                }
            }
        }
        if (_player.isPrepared)  // ���� ����� ������ � �������
        {
            //_audioSource.Play();            
            _timer = -1f;
            _text.text = "������� ����� �������, ��� �� ���������� �������� �����...";
            Invoke("VideoStart", 1f);
        }
        
    }

    public void VideoStart()
    {
        //_audioSource.Play();
        // _audioSource.PlayOneShot(_audioClip);
        _screen.color = _screenColorWhite;
        _audioSource.volume = 0.3f;
        _player.Play();
        
        /*_timer = 0f;  // ������ �������
        _screen.color = _screenColorWhite;
        _text.text = "";*/
    }

    
}