using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Western
{
    public class Citizen : Person
    {


        public Citizen() : base()
        {
        }

        public override void Initialize(int groupIndex, int personIndex, Groups groups, int roundIndex)
        {
            base.Initialize(groupIndex, personIndex, groups, roundIndex);
            m_personType = PERSONTYPE.PT_CITIZEN;

            // ���� ���� ��� ��ġ
            if (roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv3)
                Combine_Round3();
            else if (roundIndex == (int)WesternManager.LEVELSTATE.LS_PlayLv2)
                Combine_Round2();
            else
                Combine_Round1();
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void Combine_Round1()
        {
            // ���� ��ҿ� �ù� ��Ҹ� ��� ���� ���� / �ù� ��Ұ� �ּ� 1�� �̻� ���� ��
            // ���� �׷� �� ������ �ߺ� ����

            Person.ElementType1 elementStruct = new Person.ElementType1();

            int citizenIndex = 0;
            int index = 0;
            string eyeName   = "";
            string scarfName = "";

            while (true)
            {
                citizenIndex = Random.Range(0, 3);
                switch(citizenIndex)
                {
                    case 0:
                        // ���� �ùο�� ���
                        index = Random.Range(0, 3);
                        switch (index)
                        {
                            case 0:
                                elementStruct.eye = Person.EYE.EYE_PINK;
                                eyeName = "Eye_Pink/AC_Eye_Pink";
                                break;
                            case 1:
                                elementStruct.eye = Person.EYE.EYE_BLUE;
                                eyeName = "Eye_Blue/AC_Eye_Blue";
                                break;
                            case 2:
                                elementStruct.eye = Person.EYE.EYE_WHITE;
                                eyeName = "Eye_White/AC_Eye_White";
                                break;
                        }

                        // �ȴ� ����
                        index = Random.Range(0, 2);
                        switch(index)
                        {
                            case 0:
                                elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_NON;
                                break;

                            case 1:
                                elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_USE;
                                break;
                        }

                        // ��ī�� ����
                        index = Random.Range(0, 5);
                        switch (index)
                        {
                            case 0:
                                elementStruct.scarf = Person.SCARF.SCARF_SPRITE;
                                scarfName = "Scarf_Sprite/AC_Scarf_Sprite";
                                break;
                            case 1:
                                elementStruct.scarf = Person.SCARF.SCARF_WAVE;
                                scarfName = "Scarf_Wave/AC_Scarf_Wave";
                                break;
                            case 2:
                                elementStruct.scarf = Person.SCARF.SCARF_WATERDROP;
                                scarfName = "Scarf_Waterdrop/AC_Scarf_Waterdrop";
                                break;
                            case 3:
                                elementStruct.scarf = Person.SCARF.SCARF_PAINTING;
                                scarfName = "Scarf_Painting/AC_Scarf_Painting";
                                break;
                            case 4:
                                elementStruct.scarf = Person.SCARF.SCARF_SOLID;
                                scarfName = "Scarf_Solid/AC_Scarf_Solid";
                                break;
                        }
                        break;

                    case 1:
                        // �� ����
                        index = Random.Range(0, 4);
                        switch (index)
                        {
                            case 0:
                                elementStruct.eye = Person.EYE.EYE_BLUE;
                                eyeName = "Eye_Blue/AC_Eye_Blue";
                                break;
                            case 1:
                                elementStruct.eye = Person.EYE.EYE_GREEN;
                                eyeName = "Eye_Green/AC_Eye_Green";
                                break;
                            case 2:
                                elementStruct.eye = Person.EYE.EYE_PINK;
                                eyeName = "Eye_Pink/AC_Eye_Pink";
                                break;
                            case 3:
                                elementStruct.eye = Person.EYE.EYE_WHITE;
                                eyeName = "Eye_White/AC_Eye_White";
                                break;
                        }

                        // �ȴ� �ùο�� ���
                        elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_NON;

                        // ��ī�� ����
                        index = Random.Range(0, 5);
                        switch (index)
                        {
                            case 0:
                                elementStruct.scarf = Person.SCARF.SCARF_SPRITE;
                                scarfName = "Scarf_Sprite/AC_Scarf_Sprite";
                                break;
                            case 1:
                                elementStruct.scarf = Person.SCARF.SCARF_WAVE;
                                scarfName = "Scarf_Wave/AC_Scarf_Wave";
                                break;
                            case 2:
                                elementStruct.scarf = Person.SCARF.SCARF_WATERDROP;
                                scarfName = "Scarf_Waterdrop/AC_Scarf_Waterdrop";
                                break;
                            case 3:
                                elementStruct.scarf = Person.SCARF.SCARF_PAINTING;
                                scarfName = "Scarf_Painting/AC_Scarf_Painting";
                                break;
                            case 4:
                                elementStruct.scarf = Person.SCARF.SCARF_SOLID;
                                scarfName = "Scarf_Solid/AC_Scarf_Solid";
                                break;
                        }
                        break;

                    case 2:
                        // �� ����
                        index = Random.Range(0, 4);
                        switch (index)
                        {
                            case 0:
                                elementStruct.eye = Person.EYE.EYE_BLUE;
                                eyeName = "Eye_Blue/AC_Eye_Blue";
                                break;
                            case 1:
                                elementStruct.eye = Person.EYE.EYE_GREEN;
                                eyeName = "Eye_Green/AC_Eye_Green";
                                break;
                            case 2:
                                elementStruct.eye = Person.EYE.EYE_PINK;
                                eyeName = "Eye_Pink/AC_Eye_Pink";
                                break;
                            case 3:
                                elementStruct.eye = Person.EYE.EYE_WHITE;
                                eyeName = "Eye_White/AC_Eye_White";
                                break;
                        }

                        // �ȴ� ����
                        index = Random.Range(0, 2);
                        switch (index)
                        {
                            case 0:
                                elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_NON;
                                break;

                            case 1:
                                elementStruct.blindfold = Person.BLINDFOLD.BLINDFOLD_USE;
                                break;
                        }

                        // ��ī�� �ùο�� ���
                        elementStruct.scarf = Person.SCARF.SCARF_SOLID;
                        scarfName = "Scarf_Solid/AC_Scarf_Solid";
                        break;
                }

                // ���� �׷� �� �ߺ� �˻�
                if (m_groups.Check_ElementCitizen(m_groupIndex, m_personIndex, elementStruct) == false)
                    break;
            }

            m_element = elementStruct;

            // �� ����
            GameObject eye = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
            eye.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.01f); // 3
            eye.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            eye.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            eye.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Eye/" + eyeName);

            // �ȴ� ����
            if (elementStruct.blindfold == Person.BLINDFOLD.BLINDFOLD_USE)
            {
                GameObject blindfold = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
                blindfold.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.005f); // 2
                blindfold.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                blindfold.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                blindfold.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Blindfold/Blindfold/AC_Blindfold");
            }

            // ��ī�� ����
            GameObject scarf = Instantiate(Resources.Load<GameObject>("5. Prefab/2. Western/Common/PersonElement"), gameObject.transform);
            scarf.GetComponent<Transform>().localPosition = new Vector3(0f, 0f, -0.005f); // 2
            scarf.GetComponent<Transform>().localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            scarf.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            scarf.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("6. Animation/2. Western/Character/Round1/Person/Element/Scarf/" + scarfName);

        }

        public void Combine_Round2()
        {

        }

        public void Combine_Round3()
        {

        }
    }
}

