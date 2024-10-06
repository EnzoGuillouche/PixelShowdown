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
        transform.localScale = new Vector3(15 * transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<GutsActions>().health / transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<GutsActions>().maxHealth, transform.localScale.y);
    }
}
