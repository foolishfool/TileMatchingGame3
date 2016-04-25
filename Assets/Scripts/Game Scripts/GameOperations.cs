/***
 * 
 *    Title: "Diamond Crash" Project
 *           
 *    Operation Manager
 *           
 *    1£º coordinate all parts' work (whether in "busy" state)
 *    2£º allocate the neighbour
 * 
 *    Description: 
 *         
 * 
 *    Date: 2016
 *    
 *    Version: 0.1
 *    
 *    Modify Recoder: 
 *   
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOperations : MonoBehaviour 
{
    public static GameOperations instance;
    internal bool doesHaveBrustItem = false; 

    internal PlayingObject item1;
    internal PlayingObject item2;

    internal PlayingObject []suggestionItems;
    public float delay = .2f;
    float baseDelay;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        baseDelay = delay;
        suggestionItems = new PlayingObject[2];
        GameManager.instance.isBusy = true;
        Invoke("AssignNeighbours", .5f);
    }

    internal void FreeMachine()
    {
        GameManager.instance.objectFallingDuration = GameManager.instance.initialObjectFallingDuration;
        delay = baseDelay;
        GameManager.instance.isBusy = false;

        if (item1)
            item1.UnSelectMe();
        if (item2)
            item2.UnSelectMe();

        item1 = null;
        item2 = null;
    }
   
    /// <summary>
    /// check the state of "chessboard"
    /// </summary>
    internal void CheckBoardState()
    {
        GameManager.instance.isBusy = true;
        suggestionItems = new PlayingObject[2];
        CancelInvoke("ShowHint");
        doesHaveBrustItem = false;

        //loop of all the chesses to check whether there are some could be eliminated
        for (int j = 0; j < ColumnManager.instance.gameColumns.Length; j++)
        {
            for (int i = 0; i < ColumnManager.instance.gameColumns[j].playingObjectsScriptList.Count; i++)
            {
                if(ColumnManager.instance.gameColumns[j].playingObjectsScriptList[i] != null)
                    ((PlayingObject)ColumnManager.instance.gameColumns[j].playingObjectsScriptList[i]).CheckIfCanBrust();
            }
        }
       // print(doesHaveBrustItem);

        //could be eliminated
        if (doesHaveBrustItem)
        {
            SoundFxManager.instance.whopSound.Play();
            RemoveBrustItems();
            Invoke("AddMissingItems", delay);
            delay = baseDelay;
        }
        else
        {
            GameManager.numberOfItemsPoppedInaRow = 0;
            FreeMachine();
            CheckForPossibleMove();
            Invoke("ShowHint", 5f);
        }

    }

    internal void RemoveBrustItems()
    {
        for (int i = 0; i < ColumnManager.instance.gameColumns.Length; i++)
        {
            ColumnManager.instance.gameColumns[i].DeleteBrustedItems();
        }
    }

    internal void AddMissingItems()
    {
        float delay = 0;
        for (int i = 0; i < ColumnManager.instance.gameColumns.Length; i++)
        {
            if (ColumnManager.instance.gameColumns[i].GetNumberOfItemsToAdd() > 0)
            {
                ColumnManager.instance.gameColumns[i].Invoke("AddMissingItems", delay);
                delay += .05f;
            }
        }

        Invoke("AssignNeighbours", delay + .1f);
    }

    void ShowHint()
    {       
        if (GameManager.instance.isBusy)
            return;

        GameOperations.instance.suggestionItems[0].Animate();
        GameOperations.instance.suggestionItems[1].Animate();
    }
    

    internal void StopShowingHint()
    {
        if (GameOperations.instance.suggestionItems[0])
            GameOperations.instance.suggestionItems[0].StopAnimating();
        if (GameOperations.instance.suggestionItems[1])
            GameOperations.instance.suggestionItems[1].StopAnimating();
    }

    void CheckForPossibleMove()
    {
        if (!IsMovePossible())
        {
         
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);   
        }        
    }


    bool IsMovePossible()
    {        
        for (int j = 0; j < ColumnManager.instance.gameColumns.Length; j++)
        {
            for (int i = 0; i < ColumnManager.instance.gameColumns[j].playingObjectsScriptList.Count; i++)
            {
                if (ColumnManager.instance.gameColumns[j].playingObjectsScriptList[i] != null)
                {
                    if (((PlayingObject)ColumnManager.instance.gameColumns[j].playingObjectsScriptList[i]).isMovePossible())
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// allocate the neighbour chess
    /// </summary>
    internal void AssignNeighbours()
    {
        for (int i = 0; i < ColumnManager.instance.gameColumns.Length; i++)
        {
            //invoke every column script 's allocating neighbour method
            ColumnManager.instance.gameColumns[i].AssignNeighbours();
        }

        Invoke("CheckBoardState", GameManager.instance.objectFallingDuration);
        GameManager.instance.objectFallingDuration = GameManager.instance.initialObjectFallingDuration;
    }

   


    
}
