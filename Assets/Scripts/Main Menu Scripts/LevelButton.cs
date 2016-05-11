/***
 * 
 *    Title: "快乐消消乐" 项目
 *           
 *    选择“关卡”
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

public class LevelButton : MonoBehaviour {

    public int levelNo;

	void Start () 
    {
        transform.FindChild("Text").GetComponent<TextMesh>().text = name;

    }

    void OnMouseDown()
    {
        Application.LoadLevel(int.Parse(name));
        Debug.Log(name);
    }
}
