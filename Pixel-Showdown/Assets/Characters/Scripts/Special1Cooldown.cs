using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special1Cooldown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.parent.GetChild(transform.parent.childCount - 1).GetComponent<GutsActions>().spe1Cooldown, transform.localScale.y);
    }
}
