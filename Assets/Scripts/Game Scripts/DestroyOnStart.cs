/***
 * 
 *    Title: "Diamond Crash" Project
 *           
 *    Eliminate
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

public class DestroyOnStart : MonoBehaviour {

	void Awake () 
    {
        Destroy(gameObject);	
	}
}
