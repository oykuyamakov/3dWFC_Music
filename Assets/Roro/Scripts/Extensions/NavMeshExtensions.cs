using UnityEngine;
using UnityEngine.AI;

namespace Utility.Extensions
{
    public static class NavMeshExtensions
    {
        public static bool HasReachedDestination(this NavMeshAgent agent)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}