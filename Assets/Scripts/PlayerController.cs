using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float maxHealth = 100;
    public float health = 0;
	public Slider healthBar;

	// Start is called before the first frame update
	void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = health / maxHealth;

        if (health <= 0)
        {
			Cursor.lockState = CursorLockMode.None;
			SceneManager.LoadScene("GameOver");
		}
    }

    public void TakeDamage(float hp)
    {
        health -= hp;
    }
}
