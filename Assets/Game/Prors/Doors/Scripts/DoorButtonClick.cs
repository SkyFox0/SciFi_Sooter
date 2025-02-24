using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorButtonClick : MonoBehaviour
{
    public DoorSystem DoorSystem;
    public Button _button;
    public Image _image;

    public Image[] _buttonImageList;        // список всех кнопок кнопка
    public Image _activeButtonImage;          // текущая активная кнопка
    [SerializeField] private int _activeButtonIndex = 1;
    [SerializeField] private string[] ButtonType = new string[]
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


    public Color _activeColor;
    private Color _deactiveColor;
    [SerializeField] private bool _isVisible; // кнопка видима и доступна для нажатия
    [SerializeField] private bool _isActive; // кнопка активна - подсвечена игроком
    [SerializeField] private bool _isFlash;  // кнопка мигает

    public float _activateTime; // время, когда была активирована кнопка

    /*public VideoManager VideoManager;
    public bool _isTVEnabled;
    public int _typeOfButton; //1 = On, 2 = Off, 3 = Next, 4 = Back; */


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _isVisible = true;

        _isActive = false;
        _isFlash = false;
        _deactiveColor = Color.white;
        //_isTVEnabled = VideoManager.isPower;
        Invoke("CheckButton", 3f);
    }

    public void CheckButton()  // проверка правильно ли горит кнопка
    {
        _activeButtonImage = _buttonImageList[_activeButtonIndex];
        for (int i = 0; i < ButtonType.Length; i++)
        {
            Debug.Log(i.ToString());
            if (_buttonImageList[i] = _activeButtonImage)
            {
                _buttonImageList[i].enabled = true;
            }
            else
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
        if (!_isActive)
        {
            StartCoroutine(CheckActiveButton());   // запуск проверка активна ли всё ещё кнопка?
                                                   //StartCoroutine(Flashing());

            _button.transform.localScale = new Vector3(1.1f, 1f, 1f);
            _image.color = _activeColor;
            _isActive = true;
            _isFlash = true;
            _activateTime = Time.time;
        }
        else
        {
            if (_isFlash)
            {
                _button.transform.localScale = new Vector3(1f, 1f, 1f);
                _image.color = _deactiveColor;
                _isActive = true;
                _isFlash = false;
                _activateTime = Time.time;
            }
            else
            {
                _button.transform.localScale = new Vector3(1.1f, 1f, 1f);
                _image.color = _activeColor;
                _isActive = true;
                _isFlash = true;
                _activateTime = Time.time;
            }
        }
    }


    public void CheckButtonEnabled()
    {
        // ПЕРЕДЕЛАТЬ!
        //
        //
        /*_isTVEnabled = VideoManager.isPower
        if (_isTVEnabled)
        {
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


    IEnumerator CheckActiveButton()  // проверка активна ли кнопка
    {
        while (true)
        {
            if (((Time.time - _activateTime) > 0.35f) && _isActive)
            {
                _button.transform.localScale = new Vector3(1f, 1f, 1f);
                _image.color = _deactiveColor;
                _isActive = false;
            }

            yield return new WaitForSeconds(0.4f);
        }
    }

    public void Click()  // кнопка нажата игроком  - вызов события
    {
        if (_isVisible)
        {
            //Debug.Log("кнопка активации нажата3");
            _button = _activeButtonImage.transform.GetComponent<Button>();
            _button.onClick.Invoke();
            //Debug.Log("кнопка активации нажата4");
        }
    }
}
