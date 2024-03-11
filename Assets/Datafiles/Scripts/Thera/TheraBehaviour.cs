using UnityEngine;
using UnityEngine.AI;

public class TheraBehaviour : MonoBehaviour
{
    public bool shutdown = false;
    public void Shutdown(GameObject []objts, NavMeshAgent agent)
    {
        foreach (var obj in objts)
        {
            obj.SetActive(true);
        }
        agent.isStopped = true;
        shutdown = true;
    }

    public void Reactivate(GameObject []objts, NavMeshAgent agent)
    {
        foreach (var obj in objts)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        agent.isStopped = false;
        shutdown = false;
    }
}
