/***
 * 
 *    Title: "Diamond Crash" Project
 *           
 *    Level Structure
 *           
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
using System.Collections;

public class LevelStructure : MonoBehaviour 
{
    public static LevelStructure instance;

    internal int numberOfColumns;
    public int numberOfRows = 5;
    internal ColumnStructure[] columnStructures;


    void Awake()
    {
        instance = this;
        numberOfColumns = transform.childCount;
        columnStructures = transform.GetComponentsInChildren<ColumnStructure>();
    }
	
}
