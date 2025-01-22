using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorButtonClick : MonoBehaviour
{
    public Button _button;
    public Image _image;
    public Color _activeColor;
    private Color _deactiveColor;

    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isFlash;

    public float _activateTime; // врем€, когда была активирована кнопка

    /*public VideoManager VideoManager;
    public bool _isTVEnabled;
    public int _typeOfButton; //1 = On, 2 = Off, 3 = Next, 4 = Back; */


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _isActive = false;
        _isFlash = false;
        _deactiveColor = Color.white;
        //_isTVEnabled = VideoManager.isPower;
    }

    // Update is called once per frame
    public void SetButtonActive()
    {
        if (!_isActive)
        {
            StartCoroutine(CheckActiveButton());   // запуск проверка активна ли всЄ ещЄ кнопка?
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
        // ѕ≈–≈ƒ≈Ћј“№!
        //
        //
        /*_isTVEnabled = VideoManager.isPower;
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

    public void Click()
    {
        //Debug.Log("кнопка активации нажата3");
        _button.onClick.Invoke();
        //Debug.Log("кнопка активации нажата4");

    }
}
