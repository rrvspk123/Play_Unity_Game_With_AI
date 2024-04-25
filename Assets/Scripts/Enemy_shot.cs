using UnityEngine;
using System.Collections;

public class EnemyShot : MonoBehaviour
{
    public GameObject fireballPrefab; // 火球预制体
    public Transform firePoint; // 发射火球的位置
    public float fireballSpeed = 3f; // 火球的速度
    public float cooldownTime = 3f; // 冷却时间

    private bool hasDetectedPlayer = false; // 是否检测到玩家
    private bool canFire = true; // 是否可以发射火球

    private void Update()
    {
        if (IsMonsterWithinCameraView())
        {
            hasDetectedPlayer = true;
        }

        if (!hasDetectedPlayer)
            return;

        // 在这里编写怪物发射火球的逻辑
        if (canFire)
        {
            FireAtPlayer();
        }
    }

    private bool IsMonsterWithinCameraView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds))
        {
            return true;
        }

        return false;
    }

    private void FireAtPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        // 计算火球的方向
        Vector3 direction = (player.transform.position - firePoint.position).normalized;

        // 创建火球实例
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        // 获取火球的刚体组件
        Rigidbody2D fireballRigidbody = fireball.GetComponent<Rigidbody2D>();

        // 设置火球的速度和方向
        fireballRigidbody.velocity = direction * fireballSpeed;

        // 设置冷却时间
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        canFire = false;
        yield return new WaitForSeconds(cooldownTime);
        canFire = true;
    }
}