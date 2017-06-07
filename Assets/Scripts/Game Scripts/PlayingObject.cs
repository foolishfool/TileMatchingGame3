/***
 * 
 *    Title: "Diamond Crash" Project
 *           
 *    ��GameObject��Class 
 *           
 *    check whether the gameobject could be eliminated
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

public enum ObjectType
{
    None,
    Horizontal,
    Vertical,
    Universal
};

public class PlayingObject : MonoBehaviour 
{
    public ObjectType objectType = ObjectType.None; //the type of object
    static PlayingObject parentCallingScript;  

    public GameObject horizontalPowerPrefab;
    public GameObject verticalPowerPrefab;
    internal ColumnScript myColumnScript;
    internal int indexInColumn;
    public bool isTraced = false;
    public bool brust = false;                             //whether could be eliminated
    //the chess nearby  left,right,up,down
    public PlayingObject[] adjacentItems;                  //neighbour array��0��1��2��3 /left right up down�� 

    public bool isSelected = false;
    public int itemId;                                     //current object id
    public Vector3 initialScale;                           //initialize the size
    internal SpecialPlayingObject specialObjectScript;     //speical object script
    internal GameObject specialObjectToForm = null;        //form the speicial object

    string left1 = "left1";
    string left2 = "left2";
    string left3 = "left3";

    string right1 = "right1";
    string right2 = "right2";
    string right3 = "right3";

    string up1 = "up1";
    string up2 = "up2";
    string up3 = "up3";

    string down1 = "down1";
    string down2 = "down2";
    string down3 = "down3";

    string brustBy = "normal";                          //the type of eliminating
    bool isDestroyed = false;
    int counter = 0;


    
    void Awake()
    {
        //control the size of chess
        transform.localScale = new Vector3(.7f, .7f, .7f);
        initialScale = transform.localScale;
    }
	
	void Start () 
    {
        specialObjectScript = GetComponent<SpecialPlayingObject>();
        itemId = GetInstanceID();
        int ind = Random.Range(0, 6);

        adjacentItems = new PlayingObject[4];	
	}

    //checking the speical chess for eliminating
    void CheckForSpecialCandyFormation(string objName)
    {
        if(objName == left2 && objName == left1 && objName == right1 && objName == right2)
            parentCallingScript.specialObjectToForm = GameManager.instance.universalPlayingObjectPrefab;

        else if(objName == up2 && objName == up1 && objName == down1 && objName == down2)
            parentCallingScript.specialObjectToForm = GameManager.instance.universalPlayingObjectPrefab;

        else if ((objName == left2 && objName == left1 && objName == right1) || (objName == left1 && objName == right1 && objName == right2))
        {
            if (Random.value < .5f)
                parentCallingScript.specialObjectToForm = parentCallingScript.horizontalPowerPrefab;
            else
                parentCallingScript.specialObjectToForm = parentCallingScript.verticalPowerPrefab;
        }

        else if ((objName == up2 && objName == up1 && objName == down1) || (objName == up1 && objName == down1 && objName == down2))
        {
            if (Random.value < .5f)
                parentCallingScript.specialObjectToForm = parentCallingScript.horizontalPowerPrefab;
            else
                parentCallingScript.specialObjectToForm = parentCallingScript.verticalPowerPrefab;
        }
        
    }

    /// <summary>
    /// 2) allocating current chess's left right up down neighbour chess
    /// </summary>
    void AssignLRUD()
    {
        left1 = "left1";
        left2 = "left2";
        left3 = "left3";
        right1 = "right1";
        right2 = "right2";
        right3 = "right3";
        up1 = "up1";
        up2 = "up2";
        up3 = "up3";
        down1 = "down1";
        down2 = "down2";
        down3 = "down3";

        //left
        if (adjacentItems[0])
        {
            left1 = adjacentItems[0].name;
            if (adjacentItems[0].adjacentItems[0])
            {
                left2 = adjacentItems[0].adjacentItems[0].name;
                if (adjacentItems[0].adjacentItems[0].adjacentItems[0])
                    left3 = adjacentItems[0].adjacentItems[0].adjacentItems[0].name;
            }
        }
        //right
        if (adjacentItems[1])
        {
            right1 = adjacentItems[1].name;
            if (adjacentItems[1].adjacentItems[1])
            {
                right2 = adjacentItems[1].adjacentItems[1].name;
                if (adjacentItems[1].adjacentItems[1].adjacentItems[1])
                    right3 = adjacentItems[1].adjacentItems[1].adjacentItems[1].name;
            }
        }
        //up
        if (adjacentItems[2])
        {
            up1 = adjacentItems[2].name;
            if (adjacentItems[2].adjacentItems[2])
            {
                up2 = adjacentItems[2].adjacentItems[2].name;
                if (adjacentItems[2].adjacentItems[2].adjacentItems[2])
                    up3 = adjacentItems[2].adjacentItems[2].adjacentItems[2].name;
            }
        }
        //down
        if (adjacentItems[3])
        {
            down1 = adjacentItems[3].name;
            if (adjacentItems[3].adjacentItems[3])
            {
                down2 = adjacentItems[3].adjacentItems[3].name;
                if (adjacentItems[3].adjacentItems[3].adjacentItems[3])
                    down3 = adjacentItems[3].adjacentItems[3].adjacentItems[3].name;
            }
        }
    }

    //check whether could be eliminated
    internal bool JustCheckIfCanBrust(string objName, int parentIndex)
    {
        bool canBurst = false;                             

        //allocating current chess's left right up down neighbour chess
        AssignLRUD(); // get the neighbour chess

        if (parentIndex == 0)
            right1 = "right1";
        if (parentIndex == 1)
            left1 = "left1";
        if (parentIndex == 2)
            down1 = "down1";
        if (parentIndex == 3)
            up1 = "up1";

        //meet the match 3 situation coule be eliminated
        if ((objName == left1 && objName == left2) || (objName == left1 && objName == right1) || (objName == right1 && objName == right2)
            || (objName == up1 && objName == up2) || (objName == up1 && objName == down1) || (objName == down1 && objName == down2))
        {
            canBurst = true;                               //allow to eliminated
            //notice the GameOperation.cs that there is match3 chess 
            GameOperations.instance.doesHaveBrustItem = true;
        }

        if (canBurst)
        {
            if (parentCallingScript)  // if there is playing object
                CheckForSpecialCandyFormation(objName);    //check whether the speical eliminating situation exists
        }

        return canBurst;
    }

    /// <summary>
    /// ��1��check whether can eliminate
    /// </summary>
    /// <returns></returns>
    internal bool CheckIfCanBrust()
    {
        if (isDestroyed)
            return false;

        if (brust)
        {
            GameOperations.instance.doesHaveBrustItem = true;
            return true;
        }

        AssignLRUD();

        //meet the match 3 situation
        if ((name == left1 && name == left2) || (name == left1 && name == right1) || (name == right1 && name == right2)
            || (name == up1 && name == up2) || (name == up1 && name == down1) || (name == down1 && name == down2))
        {
            //allocating the eliminated chess
            AssignBurst("normal");
            GameOperations.instance.doesHaveBrustItem = true;
        }

        return brust;
    }

    /// <summary>
    /// ��3��allocating the eliminated chess
    /// </summary>
    /// <param name="who"></param>
    internal void AssignBurst(string who)
    {
        //print("PlayObject.cs/AssignBurst()");
        if (brust)
            return;

        brustBy = who;
        brust = true;

        if (specialObjectScript)
        {
           // GameOperations.instance.delay = .5f;
            GetComponent<SpecialPlayingObject>().AssignBrustToItsTarget();
        }
    }

    //check whether could move with neighbour chesses.
    internal bool isMovePossible() 
    {
        for (int i = 0; i < 4; i++)
        {
            if (adjacentItems[i])
            {
                if (adjacentItems[i].JustCheckIfCanBrust(name, i))
                {
                    GameOperations.instance.suggestionItems[0] = this;
                    GameOperations.instance.suggestionItems[1] = adjacentItems[i];
                    return true;
                }
            }
        }

        return false;
    }

    internal bool isMovePossibleInDirection(int dir)
    {
        parentCallingScript = this;

        if (adjacentItems[dir])
        {
            if (adjacentItems[dir].JustCheckIfCanBrust(name, dir))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ��4��eliminate self
    /// </summary>
    internal void DestroyMe()
    {
        if (isDestroyed)
            return;

        isDestroyed = true;
        GameManager.numberOfItemsPoppedInaRow++;
        GameManager.instance.AddScore();

        if (specialObjectScript)
        {
            iTween.ScaleTo(gameObject, Vector3.zero, .8f);
        }
        else
            iTween.ScaleTo(gameObject, Vector3.zero, .8f);

        if (brustBy == "smoke")
            Instantiate(GamePrefabs.instance.smokeParticles, transform.position, Quaternion.identity);
        else
            Instantiate(GamePrefabs.instance.starParticles, transform.position, Quaternion.identity);

        Invoke("Dest", 1f);

    }

    void Dest()
    {
        iTween.Stop(gameObject);
        Destroy(gameObject);
    }

    internal void Animate()
    {
        if (counter % 2 == 0)
        {
            iTween.ScaleTo(gameObject, initialScale * 1.1f, .8f);
        }
        else
        {
            iTween.ScaleTo(gameObject, initialScale / 1.1f, .8f);
        }
        counter++;
        CancelInvoke("Animate");
        Invoke("Animate", .8f);
    }

    internal void StopAnimating()
    {
        CancelInvoke("Animate");
        if(isSelected == false)
            iTween.Stop(gameObject);

        transform.localScale = initialScale;
        
    }

    internal void SelectMe()
    {
        isSelected = true;
        transform.Find("Image").GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1, 1, 1, .5f));
        
    }

    internal void UnSelectMe()
    {
        isSelected = false;
        transform.Find("Image").GetComponent<Renderer>().material.SetColor("_TintColor", new Color(.5f, .5f, .5f, .5f));
    }

    /// <summary>
    /// ��5�� compose the speical object
    /// </summary>
    /// <returns></returns>
    internal GameObject WillFormSpecialObject()
    {
        return specialObjectToForm;
    }

    internal void Burn()
    {
       transform.Find("Image").GetComponent<Renderer>().material.SetColor("_TintColor", new Color(.2f, .2f, .2f, .5f));
    }
}
