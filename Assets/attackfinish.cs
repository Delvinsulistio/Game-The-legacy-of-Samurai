using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackfinish : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Enemy enemyScript = animator.GetComponent<Enemy>();
        

        if (enemyScript != null)
        {
            enemyScript.FinishAttack();
        }
    }
}
