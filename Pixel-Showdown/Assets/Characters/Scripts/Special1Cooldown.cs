using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special1Cooldown : MonoBehaviour
{
    private string specialName;
    // Start is called before the first frame update
    void Start()
    {
        specialName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(specialName == "1Spe1" ? GutsActions.spe1Cooldown1 : GutsActions.spe1Cooldown2, transform.localScale.y);
    }
}
