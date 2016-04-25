/***
 * 
 *    Title: "快乐消消乐" 项目
 *           
 *    交换棋子
 *           
 *    Description: 
 *          [描述]   
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
    Vector3 pos1;                                          //方位1
    Vector3 pos2;                                          //方位2
    GameObject object1;                                    //对象1
    GameObject object2;                                    //对象2
	
	void Start () 
    {
        instance = this;
	}

    /// <summary>
    /// 第二个棋子（相对第一个棋子的）方位
    /// </summary>
    /// <param name="obj1"></param>
    /// <param name="obj2"></param>
    /// <returns>
    /// 返回：
    /// 0：水平方向，第二个棋子在（第一个棋子的）右边。
    /// 1：水平方向，第二个棋子在（第一个棋子的）左边。
    /// 2：垂直方向，第二个棋子，在上面
    /// 3：垂直方向，第二个棋子，在下面
    /// </returns>
    int GetDirectionOfSecondObject(GameObject obj1, GameObject obj2)
    {
        int index = -1;

        //两个棋子，在同一垂直方向上（Y）
        if (obj1.transform.position.x == obj2.transform.position.x)
        {
            //第二个棋子，在上面
            if (obj1.transform.position.y < obj2.transform.position.y)
                index = 2;
            //第二个棋子，在下面
            else
                index = 3;
        }
        //两个棋子，在同一水平方向上（X）
        else
        {
            //第二个棋子在（第一个棋子的）左边。
            if (obj1.transform.position.x > obj2.transform.position.x)
                index = 1;
            else
            //第二个棋子在（第一个棋子的）右边。
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

        //普通类型
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
        //特殊类型
        else if ((type2 == ObjectType.None && (type1 == ObjectType.Horizontal || type1 == ObjectType.Vertical))
            || (type1 == ObjectType.None && (type2 == ObjectType.Horizontal || type2 == ObjectType.Vertical)))
        {
            if (item1.isMovePossibleInDirection(GetDirectionOfSecondObject(object1, object2)) == false && (item2.isMovePossibleInDirection(GetDirectionOfSecondObject(object2, object1)) == false))
            {
                Invoke("ChangePositionBack", GameManager.instance.swappingTime);
                return;
            }
            //符合条件，开始正式交换棋子
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
    /// 交换复位（交换回来）
    /// </summary>
    void ChangePositionBack()
    {
        SoundFxManager.instance.wrongMoveSound.Play();
        iTween.MoveTo(object1, pos1, GameManager.instance.swappingTime * .6f);
        iTween.MoveTo(object2, pos2, GameManager.instance.swappingTime * .6f);

        GameOperations.instance.FreeMachine();
    }

    //交换
    internal void Swipe(PlayingObject item1, PlayingObject item2)
    {   
        ColumnScript firstColumn = item1.myColumnScript;   //所属“列”脚本
        ColumnScript secondColumn = item2.myColumnScript;

        PlayingObject temp = item1;


        item1.transform.parent = secondColumn.transform;   //第1个棋子父节点，被第2“列”赋值
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
