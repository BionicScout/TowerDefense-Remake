using System;
using TMPro;
using UnityEngine;

public class PlayerDefense : MonoBehaviour {
    public int health = 10;

    public TMP_Text healthHUD;

    public event Action<int> HealthUpdated;

    private void Start() {
        UpdateHUD(); 
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        if(enemy == null) {
            return;
        }

        damage(1);
        enemy.death();
    }

    public void damage(int damage) {
        health -= damage;
        if(health < 0) { health = 0; }

        Debug.Log("Health: " + health);
        HealthUpdated?.Invoke(health);

        if(health == 0) {
            Dead();
        }
        UpdateHUD();
    }

    public void Dead() {
        Debug.Log("Player Death!");
    }

    public void UpdateHUD() {
        healthHUD.text = "Health: " + health;
    }
}
