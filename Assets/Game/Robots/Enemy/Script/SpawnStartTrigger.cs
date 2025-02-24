using UnityEngine;

public class SpawnStartTrigger : MonoBehaviour
{
    public EnemySpawnSystem EnemySpawnSystem;
      

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("������� �������");
        if (other.gameObject.tag == "Player")
        {
            if (!EnemySpawnSystem._isAutoSpawnOn)
            {
                EnemySpawnSystem.Player = other.gameObject;
                EnemySpawnSystem._isAutoSpawnOn = true;
                Debug.Log("������ ������ ������!");
            }


            /*if (!_isUse)
            {

                Debug.Log("����� ������ �������");
                if (other.GetComponent<PlayerHealthComponentNew>().HealthNew < other.GetComponent<PlayerHealthComponentNew>()._maxHealth)
                {
                    _isUse = true;
                    other.GetComponent<PlayerHealthComponentNew>().AddHealth(Health);
                    other.GetComponent<PlayerHealthComponentNew>().Healing.Play();
                    Medkit.Destroy();
                }
                else
                {
                    // ���� �������� ������
                    other.GetComponent<PlayerHealthComponentNew>().HealthFull();
                    _isUse = false;
                }
            }*/
        }
    }

}
