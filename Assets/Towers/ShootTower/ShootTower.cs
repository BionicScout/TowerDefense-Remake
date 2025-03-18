using UnityEngine;

public class ShootTower : TowerBase {
    Timer timer;
    bool readyToShoot = true;
    public float shootTime = 3f;

    public float damage = 5f;

    private void Start() {
        timer = GetComponent<Timer>();
        if(timer == null ) {
            Debug.LogError("Error[ShootTower] - No Timer Added");
        }
        timer.timerComplete += ReadyToShoot;

        type = "Shoot";
    }

    private void Update() {
        EnemyBase enemy = GetFirstEnemy();

        if(enemy != null && readyToShoot) {
            Vector3 target = enemy.transform.position;
            Debug.DrawLine(transform.position , target , Color.red , 1.5f);
            Shoot(enemy);
        }

    }

    public void ReadyToShoot() {
        readyToShoot = true;
    }

    public void Shoot(EnemyBase enemy) {
        Debug.Log("Shoot");
        readyToShoot = false;
        timer.StartTimer(shootTime);
        enemy.damage(damage);
    }
}