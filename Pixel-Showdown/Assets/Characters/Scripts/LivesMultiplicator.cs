using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesMultiplicator : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        if (transform.parent.name.EndsWith("1")){
            transform.localScale *= new Vector2(-1, 1);
        }
    }
    void Update(){
        if (transform.parent.name.EndsWith("2")){
            if ((gameManager.lives2 < 2 && gameObject.name == "Life 2") || (gameManager.lives2 < 1 && gameObject.name == "Life")){
                Destroy(gameObject);
            }
        }
        if (transform.parent.name.EndsWith("1")){
            if ((gameManager.lives1 < 2 && gameObject.name == "Life 2") || (gameManager.lives1 < 1 && gameObject.name == "Life")){
                Destroy(gameObject);
            }
        }
    }
}
