using System.Collections;
using UnityEngine;

public class Jumpscare_Sign : Jumpscare
{
    public override void Active_Jumpscare()
    {
        /*
        [A]�� ��Ŵ�(�� �ִ� �ȳ��ǰ��� ����)�� ���ִµ�
        ���ΰ��� ���� ���� ���� ��������� ������ �� ������ ��������.
        �ݶ��̴� �������� �ʾƵ� �� �� �����ϴ�.* �Ѿ����� �Ҹ� ���� O
        */
        //x�� 0 -> -90

        m_isTrigger = true;

        StartCoroutine(Move_Sign());
        GetComponent<AudioSource>().Play();
    }

    private IEnumerator Move_Sign()
    {
        float duration = 0.2f;

        Quaternion startRotation = transform.GetChild(0).localRotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(-90, 0f, 0f);

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            transform.GetChild(0).localRotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.GetChild(0).localRotation = endRotation;
        yield break;
    }
}
