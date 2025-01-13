using UnityEngine;


namespace StarterAssets
{
    public class MedKitTrigger : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public int Health;
        public Medkit Medkit;
        public bool _isUse = false;


        //public MedkitSpawnSystem MedkitSpawnSystem;

        void Start()
        {
            Medkit = GetComponentInParent<Medkit>();
            Health = Medkit.Health;
        }

        // Update is called once per frame


        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Поднята аптечка");
            if (other.gameObject.tag == "Player")
            {
                if (!_isUse)
                {
                    
                    Debug.Log("Игрок поднял аптечку");
                    if (other.GetComponent<PlayerHealthComponentNew>().HealthNew < other.GetComponent<PlayerHealthComponentNew>()._maxHealth)
                    {
                        _isUse = true;
                        other.GetComponent<PlayerHealthComponentNew>().AddHealth(Health);
                        other.GetComponent<PlayerHealthComponentNew>().Healing.Play();
                        Medkit.Destroy();
                    }
                    else
                    {
                        // если здоровье полное
                        other.GetComponent<PlayerHealthComponentNew>().HealthFull();
                        _isUse = false;
                    }
                }
                
            }
        }
    }
}
