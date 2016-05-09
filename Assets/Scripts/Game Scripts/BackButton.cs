/***
 * 
 *    Title: "Diamond Crash" project
 *           
 *       return to the main menu
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
using UnityEditor.SceneManagement;
using System.Collections;

public class BackButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	
	void OnMouseDown () 
    {
        EditorSceneManager.LoadScene(0);
	
	}
}
