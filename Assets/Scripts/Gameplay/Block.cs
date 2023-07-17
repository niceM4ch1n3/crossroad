using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float destoryDistance;

    // Update is called once per frame
    void Update()
    {
        //FIXME
        CheckPosition();
    }

    private void CheckPosition()
    {
        if(Camera.main.transform.position.y - transform.position.y > destoryDistance)
        {
            Destroy(this.gameObject);
        }
    }
}
