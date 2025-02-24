using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorButton : MonoBehaviour
{
    public DoorSystem DoorSystem;
    public GameObject Button;  // сама кнопка
    public Button _button;     // обьект кнопка на изображении со скриптом
//    public Image _activeButtonImage;

    public Image[] _buttonImageList;        // список всех кнопок кнопка
    public Image _activeButtonImage;          // текущая активная кнопка
    [SerializeField] private int _activeButtonIndex = 1;
    [SerializeField]
    private string[] ButtonType = new string[]
        { "ButtonToOpen",
          "ButtonToClose",
          "ButtonLockedClosed",
          "ButtonDooorBlocked" };

    /*    public Image _buttonToOpen;
        public Image _buttonToClose;
        public Image _buttonLockedClosed;
        public Image _buttonDooorBlocked;*/

    /*public enum myEnum
    {
        _buttonToOpen,
        _buttonToClose,
        _buttonLockedClosed,
        _buttonDooorBlocked
    };*/

    /*public myEnum _buttonType;  // тип кнопки виден как дропдаун*/


    //public Color _activeColor;
    //private Color _deactiveColor;
    public bool _isPower;   // есди кнопка имеет питание
    [SerializeField] private bool _isVisible; // кнопка видима и доступна для нажатия
    [SerializeField] private bool _isActive; // кнопка активна - подсвечена игроком
    [SerializeField] private bool _isFlash;  // кнопка мигает

    public float _activateTime; // время, когда была активирована кнопка

    /*public VideoManager VideoManager;
    public bool _isTVEnabled;
    public int _typeOfButton; //1 = On, 2 = Off, 3 = Next, 4 = Back; */

    public AudioSource ClickSound;
    public AudioSource BlockedSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        //_button = GetComponent<Button>();
        //_image = GetComponent<Image>();
        //_isVisible = true;

        _isActive = false;
        _isFlash = false;
        //_deactiveColor = Color.white;
        //_isTVEnabled = VideoManager.isPower;
        Invoke("CheckButton", 0.1f);
    }

    public void CheckButton()  // проверка правильно ли горит кнопка
    {
        
        if (_isPower)  // если есть питание - зажечь нужный экран
        {
            _activeButtonImage = _buttonImageList[_activeButtonIndex];
            for (int i = 0; i < ButtonType.Length; i++)
            {
                //Debug.Log(i.ToString());
                if (_buttonImageList[i] == _activeButtonImage)
                {
                    _buttonImageList[i].enabled = true;
                }
                else
                {
                    _buttonImageList[i].enabled = false;
                }
            }
        }
        else   // если нет питания - погасить все кнопки
        {
            for (int i = 0; i < ButtonType.Length; i++)
            {
                _buttonImageList[i].enabled = false;
            }
        }
        
        /*
                if (_activeButtonImage.name == ButtonType[_activeButtonIndex]) // если активна правильная кнопка
                {
                   // _buttonType = myEnum[1];

                }
                else  // установить правильную кнопку
                {
                    _activeButtonImage = _buttonImageList[_activeButtonIndex];
                    for (int i = 0; i < ButtonType.Length; i++)
                    {
                        if (_buttonImageList[i] == _activeButtonImage)
                        {
                            _activeButtonImage.enabled = true;
                        }
                        else
                        {
                            _activeButtonImage.enabled = false;
                        }
                    }            
                }*/
    }

    // Update is called once per frame
    public void SetButtonActive()
    {
        if (_activeButtonIndex < 2) 
        {
            if (!_isActive)
            {
                StartCoroutine(CheckActiveButton());   // запуск проверка активна ли всё ещё кнопка?
                                                       //StartCoroutine(Flashing());

                _activeButtonImage.transform.localScale = new Vector3(11f, 10f, 10f);
                //_activeButtonImage.color = _activeColor;
                _isActive = true;
                _isFlash = true;
                _activateTime = Time.time;
            }
            else
            {
                if (_isFlash)
                {
                    _activeButtonImage.transform.localScale = new Vector3(10f, 10f, 10f);
                    //_activeButtonImage.color = _deactiveColor;
                    _isActive = true;
                    _isFlash = false;
                    _activateTime = Time.time;
                }
                else
                {
                    _activeButtonImage.transform.localScale = new Vector3(11f, 10f, 10f);
                    //_activeButtonImage.color = _activeColor;
                    _isActive = true;
                    _isFlash = true;
                    _activateTime = Time.time;
                }
            }
        }        
    }


    public void CheckButtonEnabled()
    {
        // ПЕРЕДЕЛАТЬ!
        //
        //
        //_isTVEnabled = VideoManager.isPower
        if (_isPower)
        {
            //_activeButtonImage = _buttonImageList[_activeButtonIndex];
            //CheckButton();
            SetButtonActive();
            /*
                        if (_typeOfButton > 1)
                        {
                            SetButtonActive();
                        }
                    }
                    else
                    {
                        if (_typeOfButton == 1)
                        {
                            SetButtonActive();
                        }
                    }*/
        }
    }


    IEnumerator CheckActiveButton()  // проверка активна ли кнопка
    {
        while (true)
        {
            if (((Time.time - _activateTime) > 0.35f) && _isActive)
            {
                _activeButtonImage.transform.localScale = new Vector3(10f, 10f, 10f);
                //_activeButtonImage.color = _deactiveColor;
                _isActive = false;
            }

            yield return new WaitForSeconds(0.4f);
        }
    }
    public void Click()  // кнопка нажата игроком  - вызов события
    {
        if (_isPower) // если кнопка запитана
        {
            //Debug.Log("кнопка активации нажата3");
            try
            {
                _button = _activeButtonImage.transform.GetComponent<Button>();
                _button.onClick.Invoke();
                ClickSound.Play();
            }
            catch
            {
                BlockedSound.Play();
                CheckButton();
            }
            //Debug.Log("кнопка активации нажата4");
        }
        else
        {
            BlockedSound.Play();
            CheckButton();
        }
    }
    public void SetButtonImageToOpen()  // переключить экран на ToOpen
    {
        _activeButtonIndex = 0;
        CheckButton();
    }
    public void SetButtonImageToClose()  // переключить экран на ToClose
    {
        _activeButtonIndex = 1;
        CheckButton();
    }
    public void SetButtonImageToLockedClosed()  // переключить экран на LockedClosed
    {
        _activeButtonIndex = 2;
        CheckButton();
    }
    public void SetButtonImageToBlocked()  // переключить экран на Blocked
    {
        _activeButtonIndex = 3;
        CheckButton();
    }

}
