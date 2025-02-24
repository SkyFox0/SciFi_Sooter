using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorButton : MonoBehaviour
{
    public DoorSystem DoorSystem;
    public GameObject Button;  // ���� ������
    public Button _button;     // ������ ������ �� ����������� �� ��������
//    public Image _activeButtonImage;

    public Image[] _buttonImageList;        // ������ ���� ������ ������
    public Image _activeButtonImage;          // ������� �������� ������
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

    /*public myEnum _buttonType;  // ��� ������ ����� ��� ��������*/


    //public Color _activeColor;
    //private Color _deactiveColor;
    public bool _isPower;   // ���� ������ ����� �������
    [SerializeField] private bool _isVisible; // ������ ������ � �������� ��� �������
    [SerializeField] private bool _isActive; // ������ ������� - ���������� �������
    [SerializeField] private bool _isFlash;  // ������ ������

    public float _activateTime; // �����, ����� ���� ������������ ������

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

    public void CheckButton()  // �������� ��������� �� ����� ������
    {
        
        if (_isPower)  // ���� ���� ������� - ������ ������ �����
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
        else   // ���� ��� ������� - �������� ��� ������
        {
            for (int i = 0; i < ButtonType.Length; i++)
            {
                _buttonImageList[i].enabled = false;
            }
        }
        
        /*
                if (_activeButtonImage.name == ButtonType[_activeButtonIndex]) // ���� ������� ���������� ������
                {
                   // _buttonType = myEnum[1];

                }
                else  // ���������� ���������� ������
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
                StartCoroutine(CheckActiveButton());   // ������ �������� ������� �� �� ��� ������?
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
        // ����������!
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


    IEnumerator CheckActiveButton()  // �������� ������� �� ������
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
    public void Click()  // ������ ������ �������  - ����� �������
    {
        if (_isPower) // ���� ������ ��������
        {
            //Debug.Log("������ ��������� ������3");
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
            //Debug.Log("������ ��������� ������4");
        }
        else
        {
            BlockedSound.Play();
            CheckButton();
        }
    }
    public void SetButtonImageToOpen()  // ����������� ����� �� ToOpen
    {
        _activeButtonIndex = 0;
        CheckButton();
    }
    public void SetButtonImageToClose()  // ����������� ����� �� ToClose
    {
        _activeButtonIndex = 1;
        CheckButton();
    }
    public void SetButtonImageToLockedClosed()  // ����������� ����� �� LockedClosed
    {
        _activeButtonIndex = 2;
        CheckButton();
    }
    public void SetButtonImageToBlocked()  // ����������� ����� �� Blocked
    {
        _activeButtonIndex = 3;
        CheckButton();
    }

}
