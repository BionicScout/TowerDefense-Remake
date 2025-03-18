using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour {

    public List<EnemyBase> enemiesInRange;
    public string type = "Base";

    private void OnTriggerEnter2D(Collider2D collision) {

        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        if(enemy == null) {
            return;
        }

        enemiesInRange.Add(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision) {

        EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
        if(enemy == null) {
            return;
        }

        enemiesInRange.Remove(enemy);
    }


    /******************************************************
        Functions to Get Enemies
    ******************************************************/
    public EnemyBase GetFirstEnemy() {
        if(enemiesInRange.Count == 0)
            return null;

        return enemiesInRange[0];
    }

    public EnemyBase GetLastEnemy() {
        if(enemiesInRange.Count == 0)
            return null;

        return enemiesInRange[enemiesInRange.Count - 1];
    }

    public EnemyBase GetClosestEnemy() {
        if(enemiesInRange.Count == 0)
            return null;


        EnemyBase closestEnemy = enemiesInRange[0];
        float closestRange = Vector2.Distance(this.transform.position, closestEnemy.transform.position);

        foreach(EnemyBase enemy in enemiesInRange) {
            float enemyDistance = Vector2.Distance(this.transform.position, enemy.transform.position);

            if(enemyDistance <= closestRange) {
                closestRange = enemyDistance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}