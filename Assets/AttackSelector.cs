using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSelector : StateMachineBehaviour
{
    private int attackType;
    int currentAttackValue;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackType = Animator.StringToHash("Attack");
        currentAttackValue = animator.GetInteger(attackType);
        Debug.Log("curatkval" + currentAttackValue);
        

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger(attackType, currentAttackValue);        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    
}
