using UnityEngine;


namespace StarterAssets
{
    public class MedKitTrigger : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public int Health;
        public Medkit Medkit;
        //public MedkitSpawnSystem MedkitSpawnSystem;

        void Start()
        {
            Medkit = GetComponentInParent<Medkit>();
            Health = Medkit.Health;
        }

        // Update is called once per frame


        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("������� �������");
            if (other.gameObject.tag == "Player")
            {
                //Debug.Log("����� ������ �������");
                if (other.GetComponent<PlayerHealthComponentNew>().Health < other.GetComponent<PlayerHealthComponentNew>()._maxHealth)
                {
                    other.GetComponent<PlayerHealthComponentNew>().AddHealth(Health);
                    other.GetComponent<PlayerHealthComponentNew>().Healing.Play();
                    Medkit.Destroy();
                }
            }
        }
    }
}
