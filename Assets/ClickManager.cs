using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour {
    public enum TowerType { none = 0, shoot = 1, slow = 2, lightning = 3 }
    TowerType selectedTowerType = TowerType.none;

    public enum ClickType { noClick = 0, ground = 1, tower = 2, UI = 3 }
    ClickType clickType = ClickType.noClick;

    bool hoveringOverButton = false;

    public List<GameObject> towerPrefabs;
    public List<Sprite> towerSprites;
    public List<int> towerCost;

    public GameObject hoverSprite;

    public TMP_Text tokenText;

    int tokens = 5;

    List<GameObject> towers = new List<GameObject>();

    private void Start() {
        hoverSprite.SetActive(false);
        UpdateTokenText();
    }

    private void Update() {
        hoverSprite.transform.position = MousePosition();


        if(GetClickType() == ClickType.ground && selectedTowerType != TowerType.none) {
            Debug.Log("Spawn Tower");
            PlaceTower();
            selectedTowerType = TowerType.none;
        }
        else if(GetClickType() == ClickType.tower) {
            GameObject selectedTower;
            MouseOverTower(out selectedTower);

            PickUpTower(selectedTower);
        }

    }

    public ClickType GetClickType() {
        bool buttonClicked = Input.GetKeyDown(KeyCode.Mouse0);
        GameObject selectedTower;

        if(!buttonClicked) {
            clickType = ClickType.noClick;
        }
        else if(hoveringOverButton) {
            clickType = ClickType.UI;
        }
        else if(MouseOverTower(out selectedTower)) {
            clickType = ClickType.tower;
        }
        else {
            clickType= ClickType.ground;
        }

        return clickType;
    }

    public bool MouseOverTower(out GameObject outTower) {
        outTower = null;

        if(towers.Count == 0) {
            return false;
        }

        GameObject cloestTower = towers[0];
        float cloestDistance = Vector2.Distance(cloestTower.transform.position, MousePosition());

        foreach(GameObject tower in towers) {
            float towerDistance = Vector2.Distance(tower.transform.position , MousePosition());

            if(towerDistance < cloestDistance) {
                cloestDistance = towerDistance;
                cloestTower = tower;
            }
        }

        if(cloestDistance > 0.5f)
            return false;

        outTower = cloestTower;
        return true;
        
    }

    public Vector2 MousePosition() {
        Camera cam = Camera.main;
        Vector2 mousePos_pixelCoords = Input.mousePosition;
        Vector2 mousePos_worldCoords =  cam.ScreenToWorldPoint(mousePos_pixelCoords);
        return mousePos_worldCoords;
    }

    public void PlaceTower() {
        int towerIndex = (int)selectedTowerType-1;

        if(towerCost[towerIndex] > tokens) {
            return;
        }

        GameObject tower = Instantiate(towerPrefabs[towerIndex]);
        tower.transform.position = (Vector3)MousePosition() + tower.transform.position.z * Vector3.forward;

        towers.Add(tower);

        tokens -= towerCost[towerIndex];
        UpdateTokenText();

        setSectedType(0);
    }

    public void PickUpTower(GameObject tower) {
        towers.Remove(tower);

        TowerType type = TowerType.none;

        TowerBase towerBase = tower.GetComponent<TowerBase>();
        if(towerBase == null)
            return;

        switch (towerBase.type) {        
            case "Shoot":
                type = TowerType.shoot; break;

            case "Slow":
                type = TowerType.slow;
                break;

            case "Lightning":
                type = TowerType.lightning;
                break;
        }

        int towerIndex = (int)type - 1;
        tokens += towerCost[towerIndex];
        UpdateTokenText();

        Destroy(tower);
        setSectedType(0);
    }

    public void setHovering(bool value) {
        hoveringOverButton = value;
    }

    public void setSectedType(int value) {        
        switch(value) {
            case 1:
                if(tokens < 1) {
                    setSectedType(0);
                    return;
                }
                selectedTowerType = TowerType.shoot;
                break;

            case 2:
                if(tokens < 3) {
                    setSectedType(0);
                    return;
                }
                selectedTowerType = TowerType.slow;
                break;

            case 3:
                if(tokens < 5) {
                    setSectedType(0);
                    return;
                }
                selectedTowerType = TowerType.lightning;
                break;

            default:
                selectedTowerType = TowerType.none;
                hoverSprite.SetActive(false);
                break;
        }

        if(selectedTowerType != TowerType.none) {
            hoverSprite.SetActive(true);
            hoverSprite.GetComponent<SpriteRenderer>().sprite = towerSprites[(int)selectedTowerType-1];
        }
    }

    public void UpdateTokenText() {
        tokenText.text = "Tokens: " + tokens;
    }

}
