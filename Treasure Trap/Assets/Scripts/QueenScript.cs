using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenScript : MonoBehaviour
{
    BoxCollider boxCollider;
    bool isMouseOver = false;

    GameManager gameManager;
    GameObject gameController;

    Player player;
    GameObject playerObject;

    static int idNum = 1;
    public string id;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = this.GetComponent(typeof(BoxCollider)) as BoxCollider;
        gameController = GameObject.FindWithTag("GameController");
        gameManager = gameController.GetComponent(typeof(GameManager)) as GameManager;

        playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent(typeof(Player)) as Player;

        id = id + idNum;
        idNum++;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && isMouseOver) {
            player.SetTile(this.gameObject);
        }
    }

    public void SetId(string newId) {
        id = newId;
    }

    public string GetId() {
        return id;
    }

    void OnMouseOver() {
        isMouseOver = true;
    }

    void OnMouseExit() {
        isMouseOver = false;
    }
}