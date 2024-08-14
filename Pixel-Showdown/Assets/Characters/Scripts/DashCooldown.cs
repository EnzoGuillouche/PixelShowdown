using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashCooldown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.parent.GetChild(0).GetComponent<GutsActions>().dashCooldown*3, transform.localScale.y);
    }
}
