using UnityEditor.Build.Content;
using UnityEngine;

public class SlowTower : TowerBase {

    private void Start() {
        type = "Slow";
    }

    private void Update() {
        Shoot();
    }

    public void Shoot() {
        for(int i = enemiesInRange.Count - 1; i >= 0; i--) {
            EnemyBase enemy = enemiesInRange[i];
            enemy.isSlowed = true;

            Vector3 target = enemy.transform.position;
            Debug.DrawLine(transform.position , target , Color.cyan , Time.deltaTime);
        }
    }
}
