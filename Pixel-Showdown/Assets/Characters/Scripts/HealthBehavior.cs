using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector2(15 * transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<PlayerActions>().health / transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<PlayerActions>().maxHealth, transform.localScale.y);
    }
}
