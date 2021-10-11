using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    private bool m_canMove;

    private void Update()
    {
        if (!m_canMove || GameManager.Instance == null) return;
        Move(GameManager.GetPlayerPosition());
    }

    private void Move(Vector3 position)
    {
        agent.SetDestination(position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            m_canMove = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            m_canMove = false;
    }
}
