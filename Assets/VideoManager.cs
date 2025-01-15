using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.Video;
using static UnityEngine.Rendering.DebugUI;

public class VideoManager : MonoBehaviour
{ 

    public VideoPlayer _player;
    [SerializeField] private string[] _url;
    //private string _url1 = "https://drive.google.com/uc?export=download&id=1QfpCr7NTERS0Uf-mCAAHLaUvdfPdKJmK";
    //private string _url2 = "https://drive.google.com/uc?export=download&id=1OAidR1m2_19hYdnGRKVhHhMK1RU3vWIl";
    //private string _url3 = "https://drive.google.com/uc?export=download&id=1hOYWv4YIgqg_3UugkxJMK3on1D8Pz0DS";
    //private string _url4 = "https://drive.google.com/uc?export=download&id=1QPvMcitfRSAcWiq3wU-ofDLyjPff0UuI";
    [SerializeField] private float[] _time;
    //private float _timer1 = 49f;
    //private float _timer2 = 118f;
    //private float _timer3 = 114f;
    //private float _timer4 = 84f;
    [SerializeField] private int _videoClipNumber;     // =1-4
    private int _videoClipNumberMax;  // =4
    [SerializeField] private float _timer = -1f;
    public Material _screen;
    public Color _screenColorWhite = Color.white;
    public Color _screenColorBlack = Color.black;
    public TMP_Text _text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_screen.color = _screenColorBlack;
        _videoClipNumber = 0;
        _videoClipNumberMax = 3;

        _time = new float[4];
        _url = new string[4];

        _url[0] = "https://drive.google.com/uc?export=download&id=1QfpCr7NTERS0Uf-mCAAHLaUvdfPdKJmK";
        _url[1] = "https://drive.google.com/uc?export=download&id=1OAidR1m2_19hYdnGRKVhHhMK1RU3vWIl";
        _url[2] = "https://drive.google.com/uc?export=download&id=1hOYWv4YIgqg_3UugkxJMK3on1D8Pz0DS";
        _url[3] = "https://drive.google.com/uc?export=download&id=1QPvMcitfRSAcWiq3wU-ofDLyjPff0UuI";

        _time[0] = 49f;
        _time[1] = 118f;
        _time[2] = 114f;
        _time[3] = 84f;

        _player = GetComponent<VideoPlayer>();
        _player.url = _url[_videoClipNumber];
        _player.Prepare();
        _text.text = "Загрузка видео...";
        _screen.color = _screenColorBlack;

    }

    // Update is called once per frame
    void Update()
    {
        if (_player.isPrepared && _timer == -1f)  // если видео готово к запуску
        {
            _text.text = "";
            _player.Play();            
            _timer = 0f;  // запуск таймера
            _screen.color = _screenColorWhite;
            _text.text = "";
        }
         
        if (_timer >= 0f)
        {
            _timer += Time.deltaTime;  // счетчик времени запущен
            if (_screen.color != _screenColorWhite)
            { 
                _screen.color = _screenColorWhite;
                _text.text = "";
            }
        }

        if  (_timer >= _time[_videoClipNumber]) // если видео закончилось
        {
            _text.text = "Загрузка видео...";
            _screen.color = _screenColorBlack;
            _timer = -1f;   // остановить таймер
            if (_videoClipNumber < _videoClipNumberMax)
                {
                    _videoClipNumber++;  // следующий клип
                }
            else
            {
                _videoClipNumber = 0;
            }
            _player.url = _url[_videoClipNumber];
            _player.Prepare();
        }
    }
}
