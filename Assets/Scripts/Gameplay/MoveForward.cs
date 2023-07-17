using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed;
    public int dir;
    public float canMoveDistance;

    private Vector2 startPos;

    private void Start()
    {
        startPos = transform.position;
        transform.localScale = new Vector3(dir, 1, 1);
    }

    void Update()
    {
        if(Mathf.Abs(transform.position.x - startPos.x) > canMoveDistance)
        {
            Destroy(this.gameObject);
        }
        Move();
    }

    private void Move()
    {
        transform.position += dir * speed * Time.deltaTime * transform.right;
    }
}
