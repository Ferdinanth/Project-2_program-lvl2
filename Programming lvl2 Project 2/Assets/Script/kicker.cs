using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kicker : NPC
{
    [Header("Kicker Vars")]
    public float attackDist = 10f;
    public float attackSpeed = 50f;
    public float attackCooldown = 30f;
    public bool canKick = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(curBall.transform);
    }

    private void FixedUpdate()
    {
        Move();
        float distToTarget = Vector3.Distance(transform.position, myTarget.transform.position);
        if (distToTarget < attackDist && canKick)
        {
            StartCoroutine(kickAction(attackCooldown));
            canKick = false;
        }
    }

    internal override void Move()
    {
        if (!canKick) { return; }
        Vector3 dir = transform.TransformDirection(Vector3.forward);
        dir *= mySpeed;
        myRB.AddForce(dir); 
    }

    internal override void Kick()
    {
        Vector3 dirTowards = myTarget.transform.position - transform.position;
        dirTowards = dirTowards.normalized;
        myRB.AddForce(dirTowards * attackSpeed);
    }
    IEnumerator kickAction(float time)
    {
        Debug.Log("kick initiated");
        Kick();
        yield return new WaitForSeconds(time);
        Debug.Log("kick cooldown ended");
        canKick = true;
    }
}
