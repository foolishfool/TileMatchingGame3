/***
 * 
 *    Title: "Diamond Crash" ��Ŀ
 *           
 *    Column Manager 
 *           
 *       
 *       Instantiate the specified quantity "column script" object  according to the class of level structure's column number
 *       and put it in current column manager's child node and confirm the column's positon(x gap)

 * 
 * 
 *    Description: 
 *  
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

public class ColumnManager : MonoBehaviour 
{
    public static ColumnManager instance;
    internal ColumnScript[] gameColumns;                   //the array of column scripts
    internal int numberOfColumns;                          //the number of columns
    

    void Awake()
    {
        instance = this;        
    }
	
	void Start () 
    {
       // Instantiate the specified quantity "column script" object according to the class of level structure's column number
     //      and put it in current column manager's child node
        numberOfColumns = LevelStructure.instance.numberOfColumns;
        gameColumns = new ColumnScript[numberOfColumns];
        for (int i = 0; i < gameColumns.Length; i++)
        {
            GameObject temp1 = new GameObject();
            gameColumns[i] = temp1.AddComponent<ColumnScript>();
            temp1.transform.parent = transform;
            temp1.name = "Column " + i.ToString();
        }
        float x = 2.5f;

        if (numberOfColumns % 2 == 1)
        {
            x = (numberOfColumns / 2) * GameManager.instance.gapBetweenObjects;
        }
        else
        {
            x = (numberOfColumns / 2) * GameManager.instance.gapBetweenObjects - GameManager.instance.gapBetweenObjects * .5f;
        }

        // confirm the column's positon(x gap)
        for (int i = 0; i < gameColumns.Length; i++)
        {
            if (i < numberOfColumns)
            {
                gameColumns[i].columnIndex = i;
                gameColumns[i].transform.localPosition = new Vector3(x - i * GameManager.instance.gapBetweenObjects, 0, 0);
            }
            else
                Destroy(gameColumns[i].gameObject);
        }
	
	}//Start_end

    

}
