using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animation anim;
    Vector3 destino;
    Vector3 stop = new Vector3(0, 0, 0);
    int speed = 0;
    public bool ask = false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ask)
        {
            anim.Play("Dizzy");
        }
        else if (navMeshAgent.velocity != stop)
        {
            anim.Play("Run");
        }
        else
            anim.Play("Idle");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Avatar")
        {
            if (other.gameObject.GetComponent<PlayerMovement>().currentObject == "Trigo")
            {
                // ir a dejar el trigo y decir quiero mas noseque
            }
            else if (other.gameObject.GetComponent<PlayerMovement>().currentObject == "Egg")
            {
                
            }
        }
    }
}
