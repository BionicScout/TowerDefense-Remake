using System;
using UnityEngine;

public class Timer : MonoBehaviour {
    float time;
    bool running;

    public event Action timerComplete;

    public void StartTimer(float startTime) {
        time = startTime;
        running = true;
    }

    public void Pause() {
        running = false;
    }
    public void Unpause() {
        running = true;
    }

    private void Update() {
        if(running) {
            time -= Time.deltaTime;

            if(time <= 0) {
                running = false;

                if(timerComplete != null)
                    timerComplete();
            }
        }
    }
}
