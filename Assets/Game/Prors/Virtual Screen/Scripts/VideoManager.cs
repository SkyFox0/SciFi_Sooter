using TMPro;
using UnityEngine;
using UnityEngine.Video;
//using static UnityEngine.Rendering.DebugUI;

public class VideoManager : MonoBehaviour
{ 

    public VideoPlayer VideoPlayer;
    [SerializeField] private string[] _url;    
    [SerializeField] private float[] _time;    
    [SerializeField] private int _videoClipNumber;     // =1-4
    private int _videoClipNumberMax;  // =4
    [SerializeField] private float _playTimer = -1f;
    public Material _screen;
    public Color _screenColorWhite = Color.white;
    public Color _screenColorBlack = Color.black;
    public TMP_Text _text;
    public int _textLenght;
    [SerializeField] private float _loadTimer = -1f;
    public GameObject BlackScreen;
    public AudioSource PressButtonSound;
    public AudioSource BlockedButtonSound;
    public bool isPower;


    //private string _url1 = "https://drive.google.com/uc?export=download&id=1QfpCr7NTERS0Uf-mCAAHLaUvdfPdKJmK";
    //private string _url2 = "https://drive.google.com/uc?export=download&id=1OAidR1m2_19hYdnGRKVhHhMK1RU3vWIl";
    //private string _url3 = "https://drive.google.com/uc?export=download&id=1hOYWv4YIgqg_3UugkxJMK3on1D8Pz0DS";
    //private string _url4 = "https://drive.google.com/uc?export=download&id=1QPvMcitfRSAcWiq3wU-ofDLyjPff0UuI";
    //private float _timer1 = 49f;
    //private float _timer2 = 118f;
    //private float _timer3 = 114f;
    //private float _timer4 = 84f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BlackScreen.SetActive(true);
        
        //ScreenTexture = BlackTexture;
        //_screen.color = _screenColorBlack;
        _videoClipNumber = 0;
        _videoClipNumberMax = 4;

        _time = new float[_videoClipNumberMax + 1];
        _url = new string[_videoClipNumberMax + 1];

        _url[0] = "https://drive.google.com/uc?export=download&id=1sbxNdtV8V9JdiGF97cSqvgezNNeYw-iA";
        _url[1] = "https://drive.google.com/uc?export=download&id=1QfpCr7NTERS0Uf-mCAAHLaUvdfPdKJmK";
        _url[2] = "https://drive.google.com/uc?export=download&id=1OAidR1m2_19hYdnGRKVhHhMK1RU3vWIl";
        _url[3] = "https://drive.google.com/uc?export=download&id=1hOYWv4YIgqg_3UugkxJMK3on1D8Pz0DS";
        _url[4] = "https://drive.google.com/uc?export=download&id=1QPvMcitfRSAcWiq3wU-ofDLyjPff0UuI";

        _time[0] = 55f;
        _time[1] = 49f;
        _time[2] = 118f;
        _time[3] = 114f;
        _time[4] = 84f;

        VideoPlayer = GetComponent<VideoPlayer>();
        VideoPlayer.url = _url[_videoClipNumber];
//        VideoPlayer.Prepare();
//        _text.text = "Загрузка видео";
        //_screen.color = _screenColorBlack;       
        
//        _loadTimer = - 1f;

        isPower = false;
}

    // Update is called once per frame
    void Update()
    {
        if (isPower)
        {
            if (VideoPlayer.isPrepared && _playTimer == -1f)  // если видео готово к запуску
            {
                _text.text = "";
                BlackScreen.SetActive(false);
                VideoPlayer.Play();
                _playTimer = 0f;  // запуск таймера
                _loadTimer = -1f; // остановка таймера анимации текста
                _screen.color = _screenColorWhite;
                _text.text = "";
            }

            if (_playTimer >= 0f)
            {
                _playTimer += Time.deltaTime;  // счетчик времени запущен
                if (_screen.color != _screenColorWhite)
                {
                    _screen.color = _screenColorWhite;
                    _text.text = "";
                }

            }
            if (_loadTimer >= 0f)
            {
                _loadTimer += Time.deltaTime;  // счетчик времени запущен
                if (_loadTimer > 1f)
                {
                    _loadTimer = 0f;
                    if (_text.text.Length < (_textLenght + 5))
                    {
                        _text.text = _text.text + ".";
                    }
                    /*else
                    {
                        _text.text = "Загрузка видео";
                    }*/
                }
            }

            if (_playTimer >= _time[_videoClipNumber]) // если видео закончилось
            {
                _loadTimer = 0f;  //  запуск таймера анимации текста
                _text.text = "Загрузка видео";
                _textLenght = _text.text.Length;
                //_screen.color = _screenColorBlack;
                _playTimer = -1f;   // остановить таймер видео
                if (_videoClipNumber < _videoClipNumberMax)
                {
                    _videoClipNumber++;  // следующий клип
                }
                else
                {
                    _videoClipNumber = 0;
                }
                VideoPlayer.url = _url[_videoClipNumber];
                VideoPlayer.Prepare();

            }
        }
        else
        {
            _text.text = "Power Off";
        }
    }

    public void OnClickNext()
    {
        if (isPower)
        {
            PressButtonSound.Play();
            BlackScreen.SetActive(true);
            _loadTimer = 0f;  //  запуск таймера анимации текста
            _text.text = "Загрузка следующего видео";
            _textLenght = _text.text.Length;
            //_screen.color = _screenColorBlack;
            _playTimer = -1f;   // остановить таймер видео
            if (_videoClipNumber < _videoClipNumberMax)
            {
                _videoClipNumber++;  // следующий клип
            }
            else
            {
                _videoClipNumber = 0;
            }
            VideoPlayer.url = _url[_videoClipNumber];
            VideoPlayer.Prepare();
        }
        else
        {
            BlockedButtonSound.Play();
        }
        
    }

    public void OnClickBack()
    {
        if (isPower)
        {            
            PressButtonSound.Play();
            BlackScreen.SetActive(true);
            _loadTimer = 0f;  //  запуск таймера анимации текста
            _text.text = "Загрузка предыдущего видео";
            _textLenght = _text.text.Length;
            //_screen.color = _screenColorBlack;
            _playTimer = -1f;   // остановить таймер видео
            if (_videoClipNumber > 0)
            {
                _videoClipNumber--;  // следующий клип
            }
            else
            {
                _videoClipNumber = _videoClipNumberMax - 1;
            }
            VideoPlayer.url = _url[_videoClipNumber];
            VideoPlayer.Prepare();
        }
        else
        {
            BlockedButtonSound.Play();
        }
        

    }

    public void OnClickON()
    {
        if (isPower)
        {
            BlockedButtonSound.Play();
        }
        else
        {            
            PressButtonSound.Play();
            isPower = true;
            BlackScreen.SetActive(true);
            VideoPlayer.url = _url[_videoClipNumber];
            VideoPlayer.Prepare();
            _text.text = "Загрузка видео";
            _textLenght = _text.text.Length;
            //_screen.color = _screenColorBlack;
            _loadTimer = 0f;
        }
        
    }

    public void OnClickOFF()
    {
        if (isPower)
        {
            PressButtonSound.Play();
            isPower = false;
            BlackScreen.SetActive(true);
            VideoPlayer.Stop();
            _text.text = "Power Off";
            _playTimer = -1f;  // запуск таймера
            _loadTimer = -1f;  // остановка таймера анимации текста
        }
        else
        {
            BlockedButtonSound.Play();
        }
        
    }
}
