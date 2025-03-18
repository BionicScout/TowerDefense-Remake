using System;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyBase : MonoBehaviour {
    public float maxHealth = 100f;
    float currentHealth;

    Rigidbody2D rigidbody;
    public float speed = 5f;

    public bool isSlowed = false;
    float frozenSpeedReduction = 0.5f;

    Spline path;
    float pointOnPath;

    bool reachedEnd = false;
    bool started = false;

    public event Action<EnemyBase> enemyDeath;

    private void Start() {
        currentHealth = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetInfoAndStart(SplineContainer pathContainer) {
        path = pathContainer[0];
        transform.position = (Vector2)(Vector3)path.ToArray()[0].Position;

        started = true;
    }

    private void FixedUpdate() {
        if(!started)
            return;

        Move();
    }

    public void MoveToTarget(Vector2 targetPos) {
        float distance = Vector2.Distance(targetPos , rigidbody.position);
        if(distance < 0.5f)
            return;


        Vector2 direction = targetPos - (Vector2)transform.position;
        direction.Normalize();

        Vector2 movementOffset = direction * (speed * Time.deltaTime);
        if(isSlowed) { movementOffset = movementOffset * frozenSpeedReduction; isSlowed = false; }
        Vector2 movePos = (Vector2)transform.position + movementOffset;

        rigidbody.MovePosition(movePos);
    }



    public void Move() {
        float moveDistance = speed * Time.deltaTime;
        if(isSlowed) { moveDistance = moveDistance * frozenSpeedReduction; isSlowed = false; }

        float testPoint = pointOnPath; 
        float testDistanceAway = 0;
        Vector3 previousTestPoint = transform.position;

        Vector3 currentTestPoint = transform.position;


        while(testDistanceAway <= moveDistance) {
            testPoint += 0.0001f;
            currentTestPoint = GetPathPoint(testPoint);
            testDistanceAway += Vector2.Distance(currentTestPoint, previousTestPoint);

            previousTestPoint = currentTestPoint;

            if(testPoint > path.ToArray().Length) {
                break;
            }
        }

        pointOnPath = testPoint;
        rigidbody.MovePosition((Vector2)currentTestPoint);
    }



    public void MoveToTarget() {
        pointOnPath += speed * Time.deltaTime;

        Vector2 movePos = GetPathPoint(pointOnPath);
        rigidbody.MovePosition(movePos);

        if(pointOnPath >= path.GetLength())
            Debug.Log("End of Path");

    }


    public Vector2 GetPathPoint(float u) {
        BezierKnot[] knots = path.ToArray();

        if(u > knots.Length) {
            reachedEnd = true;
            return (Vector2)(Vector3)knots[knots.Length-1].Position;
        }
        u = Mathf.Clamp(u, 0, knots.Length);

        int startKnot = Mathf.FloorToInt(u);        
        BezierCurve curve = path.GetCurve(startKnot);
        float t = u - startKnot;

        Vector3 pos = curve.P0 * (-Mathf.Pow(t, 3) + 3*Mathf.Pow(t ,2) - 3*t + 1) + 
                      curve.P1 * (3*Mathf.Pow(t, 3) - 6*Mathf.Pow(t, 2) + 3*t) + 
                      curve.P2 * (-3*Mathf.Pow(t, 3) + 3*Mathf.Pow(t, 2)) + 
                      curve.P3 * (Mathf.Pow(t, 3));

        return (Vector2)pos;
    }

    //Return true if killed
    public bool damage(float damage) {
        currentHealth -= damage;
        //Debug.Log("Health: " + currentHealth + "/" + maxHealth);

        if(currentHealth <= 0) {
            death();
            return true;
        }

        return false;
    }

    public void death() {
        enemyDeath(this);
        Destroy(gameObject);
    }


}