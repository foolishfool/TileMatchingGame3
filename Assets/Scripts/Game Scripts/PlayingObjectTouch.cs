/***
 * 
 *    Title: "快乐消消乐" 项目
 *           
 *    “游戏对象”操作类 
 *           
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

public class PlayingObjectTouch : MonoBehaviour
{
    PlayingObject playingObjectScript;
    bool isTouched = false;


    void Start()
    {
        playingObjectScript = GetComponent<PlayingObject>();
    }

    internal void OnMouseDown()
    {
        if (GameManager.instance.isBusy)
            return;

        isTouched = true;
        SoundFxManager.instance.objectPickSound.Play();
        ObjectSelected();
    }

    internal void ObjectSelected()
    {
        if (playingObjectScript.isSelected)
        {
            playingObjectScript.UnSelectMe();//不选择发暗
            GameOperations.instance.item1 = null;
            return;
        }

        //第一个棋子
        if (GameOperations.instance.item1 == null)
        {
            GameOperations.instance.item1 = playingObjectScript;
            playingObjectScript.SelectMe();
        }
        //第二个棋子
        else if (Vector2.Distance((Vector2)GameOperations.instance.item1.transform.position, (Vector2)transform.position) < GameManager.instance.gapBetweenObjects + .2f)
        {
            GameOperations.instance.item2 = playingObjectScript;
            playingObjectScript.SelectMe();
            //交换类的交换项目。
            SwapTwoObject.instance.SwapTwoItems(GameOperations.instance.item1, GameOperations.instance.item2);
        }
        //第二个棋子，但距离比较远的时候（参数清零）
        else
        {
            GameOperations.instance.item1.UnSelectMe();
            GameOperations.instance.item1 = null;

            GameOperations.instance.item1 = playingObjectScript;
            playingObjectScript.SelectMe();
        }
    }

    void OnMouseDrag()
    {
        if (!isTouched)
            return;

        Vector3 tempPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Vector2.Distance(transform.position, tempPosition) > GameManager.instance.gapBetweenObjects * .4f)
        {
            if (Mathf.Abs(tempPosition.x - transform.position.x) > Mathf.Abs(tempPosition.y - transform.position.y)) //left right moved
            {
                if (tempPosition.x > transform.position.x)
                {
                    if (playingObjectScript.adjacentItems[0] != null)
                        playingObjectScript.adjacentItems[0].GetComponent<PlayingObjectTouch>().ObjectSelected();
                }
                else
                {
                    if (playingObjectScript.adjacentItems[1] != null)
                        playingObjectScript.adjacentItems[1].GetComponent<PlayingObjectTouch>().ObjectSelected();
                }
            }
            else
            {
                if (tempPosition.y > transform.position.y)
                {
                    if (playingObjectScript.adjacentItems[2] != null)
                        playingObjectScript.adjacentItems[2].GetComponent<PlayingObjectTouch>().ObjectSelected();
                }
                else
                {
                    if (playingObjectScript.adjacentItems[3] != null)
                        playingObjectScript.adjacentItems[3].GetComponent<PlayingObjectTouch>().ObjectSelected();
                }
            }
            //OnMouseUp();
        }
    }

    //鼠标抬起，参数清零
    void OnMouseUp()
    {
        isTouched = false;
        playingObjectScript.UnSelectMe();
        GameOperations.instance.item1 = null;
    }

}

    

   

