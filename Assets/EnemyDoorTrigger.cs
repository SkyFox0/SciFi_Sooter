using UnityEngine;

public class EnemyDoorTrigger : MonoBehaviour
{
    public DoorSystem DoorSystem;

    private void Start()
    {
        if (!DoorSystem)
        {
            Debug.Log("������� ����� Door System");
            try
            {
                DoorSystem = GetComponentInParent<DoorSystem>();
                Debug.Log("Door System ������");
            }
            catch
            {
                Debug.Log("����� Door System �� �������");
            }
        }        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("� ����� ��������� ���������" + other.gameObject.tag.ToString());

        if (other.gameObject.tag == "Player")
        {
            // ���� � ����� ������� �����
            Debug.Log("� ����� ������� �����");
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
            // ���� � ����� ������� ����
            Debug.Log("� ����� ������� ����");
            DoorSystem.EnemyInTrigger();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        Debug.Log("� ����� ��������� ���������" + other.gameObject.tag.ToString());
        if (other.gameObject.tag == "Enemy")
        {
            // ���� � ����� ������� ����
            Debug.Log("� ����� ������� ����");
            DoorSystem.EnemyInTrigger();
        }
    }
}
