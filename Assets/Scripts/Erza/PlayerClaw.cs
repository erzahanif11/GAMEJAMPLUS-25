using UnityEngine;

public class PlayerClaw : MonoBehaviour
{
  public float attackCooldown = 0.05f;
  public Transform attackPoint;
  public float attackRange = 0.5f;
  public LayerMask enemyLayer;

  private float lastAttackTime = 0f;
  //   void Update(){
  //     if (Input.GetKeyDown(KeyCode.Mouse0)){
  //       Claw();
  //   }

  //   void Claw(){
  //     if (Time.time < lastAttackTime*attackCooldown){
  //       lastAttackTime = Time.time;
  //     }
  //     animator.setTrigger(clawAnimation);
  //   }

  //   void GiveDamage(){
  //     Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
  //     foreach (Collider2D enemy in hitEnemies){
  //       enemy.GetComponent<EnemyHealth>().TakeDamage(1);
  //     }
  //   }
  // }
}