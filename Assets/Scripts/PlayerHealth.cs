using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace XEntity.InventoryItemSystem
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 100;
        public int currentHealth;
        public int damage = 10;
        public int heal = 20;
        public TextMeshProUGUI healthText;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void Update()
        {
            CheckHealth();
            healthText.text = "Health: " + currentHealth.ToString(); 
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Projectile"))
            {
                currentHealth -= damage;
            }

            if (collision.gameObject.CompareTag("Pumpkin"))
            {
                currentHealth += heal;
            }
        }

        public void CheckHealth()
        {
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            SceneManager.LoadScene(0);
        }
    }
}

