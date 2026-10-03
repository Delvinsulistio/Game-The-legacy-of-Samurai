using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventAttack : MonoBehaviour
{  
    private Player player;

    private void Awake() {
        player = GetComponentInParent<Player>();
    }

    private void DisableJumpAndMove() =>  player.SetCanMoveAndJump(false);

    private void EnableJumpAndMove() => player.SetCanMoveAndJump(true);

    public void TakingDamageEnemies() => player.DamageTarget();
}
