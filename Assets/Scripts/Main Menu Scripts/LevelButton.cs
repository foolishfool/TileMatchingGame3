/***
 * 
 *    Title: "����������" ��Ŀ
 *           
 *    ѡ�񡰹ؿ���
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

public class LevelButton : MonoBehaviour {

    public int levelNo;

	void Start () 
    {
        transform.Find("Text").GetComponent<TextMesh>().text = name;

    }

    void OnMouseDown()
    {
        Application.LoadLevel(int.Parse(name));
        Debug.Log(name);
    }
}
