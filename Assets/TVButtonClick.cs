using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor.PackageManager;
using UnityEditor;


public class TVButtonClick : MonoBehaviour
{
    public Button _button;
    public Image _image;
    public Color _activeColor;
    private Color _deactiveColor;
    
    [SerializeField] private bool _isActive;
    [SerializeField] private bool _isFlash;

    public float _activateTime; // время, когда была активирована кнопка



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _isActive = false;
        _isFlash = false;
        _deactiveColor = Color.white;
        

    }

    // Update is called once per frame
    void Update()
    {
        /*if (_isActive && (Time.time - _activateTime) > 0.3f)
        {
            _isActive = false;
            _activateTime = 0f;
            _button.transform.localScale = new Vector3(1f, 1f, 1f);
            _image.color = _deactiveColor;
        }*/
    }

    public void SetButtonActive()
    {
        if ( !_isActive )
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
        Debug.Log("кнопка активации нажата3");
        _button.onClick.Invoke();
        Debug.Log("кнопка активации нажата4");

    }
}
