using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    public List<GameObject> towerPrefabs;
    public GameObject towerSprites;
    public List<int> towerCost;

    int tokens = 10;

    int towerSelected;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            Debug.Log("Click");
        }
    }


    public void Buy(int towerIndex) {

        if(towerCost[towerIndex] > tokens) {
            Debug.Log("Not Enough Tokens");
            return;
        }

        tokens -= towerCost[towerIndex];
        Debug.Log("Tokens: " + tokens);
    }


}
