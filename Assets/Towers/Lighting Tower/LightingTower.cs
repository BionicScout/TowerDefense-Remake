using UnityEngine;

public class LightingTower : TowerBase {
    public float damagePerSecond = 10f;

    private void Start() {
        type = "Lightning";
    }

    private void Update() {
        Shoot();
    }

    public void Shoot() {
        //Debug.Log("Lighting");

        float damage = damagePerSecond * Time.deltaTime;

        //Connectiong Enemies

        for(int i = enemiesInRange.Count - 1; i >= 0; i--) {
            EnemyBase enemy = enemiesInRange[i];
            bool dead = enemy.damage(damage);

            Vector3 target = enemy.transform.position;
            Debug.DrawLine(transform.position , target , Color.blue , Time.deltaTime);
        }
    }
}