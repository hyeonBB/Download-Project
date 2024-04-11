using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayCD : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            VisualNovelManager.Instance.Add_CD();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        // ������ ǥ��
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, 10.0f);
    }
}
