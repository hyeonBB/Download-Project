public class Jumpscare_Ball : Jumpscare
{
    public override void Active_Jumpscare()
    {
        m_isTrigger = true;

        /*
          [A]������ ���ΰ��� ����[B]���� ���׸� ���� ȭ��ǥ �������� �������� �����Ѵ�.
          ��� ������ Ƣ��ٰ� �������� �����ͼ� ���ߴ� ��������   
          �����ϰ� �����ϴ�.(��......��....��...����������...)*
          ȭ��ǥ�� �������� ���� ���ߴ� �Ÿ�?������ ���� ������ �����Դϴ�.  
          ���� ���� �� �ڸ��� �״�� �ֽ��ϴ�.* �� Ƣ��� �Ҹ� ���� O
        */
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}