using UnityEngine;

public class EnemyDoorTrigger : MonoBehaviour
{
    public DoorSystem DoorSystem;

    private void Start()
    {
        if (!DoorSystem)
        {
            Debug.Log("Попытка найти Door System");
            try
            {
                DoorSystem = GetComponentInParent<DoorSystem>();
                Debug.Log("Door System найден");
            }
            catch
            {
                Debug.Log("Найти Door System не удалось");
            }
        }        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("у двери обнаружен коллайдер" + other.gameObject.tag.ToString());

        if (other.gameObject.tag == "Player")
        {
            // если к двери подошел игрок
            Debug.Log("к двери подошел игрок");
            DoorSystem.PlayerInTrigger();

            //_isOpen = true;
            /*DoorsOpen.Play();
            DoorsAnimator.SetBool("isOpen", true);
            DoorsAnimator.SetTrigger("ToOpen");

            if (_isCloseTimer)
            {
                _timer = 0f;
            }*/
        }
        
        if (other.gameObject.tag == "Enemy")
        {
            // если к двери подошел враг
            Debug.Log("К двери подошел враг");
            DoorSystem.EnemyInTrigger();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Debug.Log("у двери обнаружен коллайдер" + other.gameObject.tag.ToString());
        if (other.gameObject.tag == "Enemy")
        {
            // если к двери подошел враг
            Debug.Log("К двери подошел враг");
            DoorSystem.EnemyInTrigger();
        }
    }
}
