/***
 * 
 *    Title: "����������" ��Ŀ
 *           
 *    ��������
 *           
 *    Description: 
 *          [����]   
 * 
 *    Date: 2015
 *    
 *    Version: 0.1
 *    
 *    Modify Recoder: 
 *   
 */
using UnityEngine;
using System.Collections;

public class SwapTwoObject : MonoBehaviour 
{
    internal static SwapTwoObject instance;
    Vector3 pos1;                                          //��λ1
    Vector3 pos2;                                          //��λ2
    GameObject object1;                                    //����1
    GameObject object2;                                    //����2
	
	void Start () 
    {
        instance = this;
	}

    /// <summary>
    /// �ڶ������ӣ���Ե�һ�����ӵģ���λ
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns>
    /// ���أ�
    /// 0��ˮƽ���򣬵ڶ��������ڣ���һ�����ӵģ��ұߡ�
    /// 1��ˮƽ���򣬵ڶ��������ڣ���һ�����ӵģ���ߡ�
    /// 2����ֱ���򣬵ڶ������ӣ�������
    /// 3����ֱ���򣬵ڶ������ӣ�������
    /// </returns>
    int GetDirectionOfSecondObject(GameObject obj1, GameObject obj2)
    {
        int index = -1;

        //�������ӣ���ͬһ��ֱ�����ϣ�Y��
        if (obj1.transform.position.x == obj2.transform.position.x)
        {
            //�ڶ������ӣ�������
            if (obj1.transform.position.y < obj2.transform.position.y)
                index = 2;
            //�ڶ������ӣ�������
            else
                index = 3;
        }
        //�������ӣ���ͬһˮƽ�����ϣ�X��
        else
        {
            //�ڶ��������ڣ���һ�����ӵģ���ߡ�
            if (obj1.transform.position.x > obj2.transform.position.x)
                index = 1;
            else
            //�ڶ��������ڣ���һ�����ӵģ��ұߡ�
                index = 0;
        }
       
        return index;
    }


    internal void SwapTwoItems(PlayingObject item1, PlayingObject item2)
    {
        iTween.Defaults.easeType = iTween.EaseType.easeOutBack;
        GameManager.instance.isBusy = true;

        object1 = item1.gameObject;
        object2 = item2.gameObject;

        pos1 = item1.transform.position;
        pos2 = item2.transform.position;

        //exchange animation
        iTween.MoveTo(object1, pos2, GameManager.instance.swappingTime);
        iTween.MoveTo(object2, pos1, GameManager.instance.swappingTime);

        ObjectType type1 = item1.objectType;
        ObjectType type2 = item2.objectType;

        //��ͨ����
        if (type1 == ObjectType.None && type2 == ObjectType.None)
        {
            if (item1.isMovePossibleInDirection(GetDirectionOfSecondObject(object1, object2)) == false && (item2.isMovePossibleInDirection(GetDirectionOfSecondObject(object2, object1)) == false))
            {
                Invoke("ChangePositionBack", GameManager.instance.swappingTime);
                return;
            }
            else
            {
                GameOperations.instance.StopShowingHint();
                Swipe(item1, item2);
                GameOperations.instance.Invoke("AssignNeighbours", .1f);
            }
        }
        //��������
        else if ((type2 == ObjectType.None && (type1 == ObjectType.Horizontal || type1 == ObjectType.Vertical))
            || (type1 == ObjectType.None && (type2 == ObjectType.Horizontal || type2 == ObjectType.Vertical)))
        {
            if (item1.isMovePossibleInDirection(GetDirectionOfSecondObject(object1, object2)) == false && (item2.isMovePossibleInDirection(GetDirectionOfSecondObject(object2, object1)) == false))
            {
                Invoke("ChangePositionBack", GameManager.instance.swappingTime);
                return;
            }
            //������������ʼ��ʽ��������
            else
            {
                GameOperations.instance.StopShowingHint();
                Swipe(item1, item2);
                GameOperations.instance.Invoke("AssignNeighbours", .1f);
            }
        }
        else
        {
            GetComponent<SwapSpecialObjects>().Swap(item1, item2);
        }

    }

    /// <summary>
    /// ������λ������������
    /// </summary>
    void ChangePositionBack()
    {
        SoundFxManager.instance.wrongMoveSound.Play();
        iTween.MoveTo(object1, pos1, GameManager.instance.swappingTime * .6f);
        iTween.MoveTo(object2, pos2, GameManager.instance.swappingTime * .6f);

        GameOperations.instance.FreeMachine();
    }

    //����
    internal void Swipe(PlayingObject item1, PlayingObject item2)
    {   
        ColumnScript firstColumn = item1.myColumnScript;   //�������С��ű�
        ColumnScript secondColumn = item2.myColumnScript;

        PlayingObject temp = item1;


        item1.transform.parent = secondColumn.transform;   //��1�����Ӹ��ڵ㣬����2���С���ֵ
        item2.transform.parent = firstColumn.transform;

        item1.myColumnScript = secondColumn;
        item2.myColumnScript = firstColumn;

        firstColumn.playingObjectsScriptList.RemoveAt(item1.indexInColumn);
        firstColumn.playingObjectsScriptList.Insert(item1.indexInColumn, item2);

        secondColumn.playingObjectsScriptList.RemoveAt(item2.indexInColumn);
        secondColumn.playingObjectsScriptList.Insert(item2.indexInColumn, item1);

        int tempIndex = item1.indexInColumn;
        item1.indexInColumn = item2.indexInColumn;
        item2.indexInColumn = tempIndex;
    }

    
}
