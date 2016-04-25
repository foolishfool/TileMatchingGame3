/***
 * 
 *    Title: "Diamond Crash" Project
 *           
 *    column script  
 *           
 *     1£ºget all the avaliable chess in current column
 *     2£ºclone chess and make it as self's child node
 *     3£ºclone the background chess
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

public class ColumnScript : MonoBehaviour 
{
    public int[] itemAvailability;                         //the avaliability of chess (yes no)

    internal int columnIndex = 0;                          //the index of current column
    internal ArrayList playingObjectsScriptList;           //the list of chess script(the list of playOjbect)
    internal ArrayList jellyObjects;
    int totalNoOfItems = 0;                                //the avaliable chess in current column
    int numberOfEmptySpace = 0;                            //the empty spcace in current column
    int numberOfRows;                                      //the number of row
    float fallingDuration;                                 //the peroid of falling 
    int numberOfItemsToAdd;


    void Awake()
    {
        
    }
	
	void Start () 
    {
        numberOfRows = LevelStructure.instance.numberOfRows;
        playingObjectsScriptList = new ArrayList();
        jellyObjects = new ArrayList();

        //get all the avaliable chess in current column£¨0 no£¬1yes£©
        itemAvailability = LevelStructure.instance.columnStructures[columnIndex].itemAvailability;
        Invoke("PopulateInitialColumn", .2f);
    }//Start_end

    /// <summary>
    /// initialize the column
    /// </summary>
    void PopulateInitialColumn()
    {
        for (int i = 0; i < itemAvailability.Length; i++)
        {            
            if (itemAvailability[i] >= 1)
                totalNoOfItems++;
        }

        //the empty spcace in current column
        numberOfEmptySpace = numberOfRows - totalNoOfItems;


        //traversing current column's every row (every chess)
        for (int i = 0; i < numberOfRows; i++)
        {
            if (itemAvailability[i] == 0)
            {
                playingObjectsScriptList.Add(null);
                jellyObjects.Add(null);
                continue;
            }
            

            int index = Random.Range(0, 6);

            GameObject objectPrefab;

            //normal prefab
            if (itemAvailability[i] == 1)
            {
                objectPrefab = GameManager.instance.playingObjectPrefabs[index];                
            }
            //horizonal and vertical prefab
            else if (itemAvailability[i] == 2)
            {
                if (Random.value < .5f)
                    objectPrefab = GameManager.instance.horizontalPrefabs[Random.Range(0, GameManager.instance.horizontalPrefabs.Length)];
                else
                    objectPrefab = GameManager.instance.verticalPrefabs[Random.Range(0, GameManager.instance.verticalPrefabs.Length)];
            }
            //invincable prefab
            else if (itemAvailability[i] == 3)
                objectPrefab = GameManager.instance.universalPlayingObjectPrefab;
            else
                objectPrefab = GameManager.instance.playingObjectPrefabs[index];

            //clone chess and make it as self's child node
            GameObject temp = (GameObject)Instantiate(objectPrefab, Vector3.zero, Quaternion.identity);
            temp.GetComponent<PlayingObject>().myColumnScript = this;
            temp.GetComponent<PlayingObject>().indexInColumn = i;
            playingObjectsScriptList.Add(temp.GetComponent<PlayingObject>());
            temp.transform.parent = transform;
            temp.transform.localPosition = new Vector3(0, -i * GameManager.instance.gapBetweenObjects, 0);

            //clone the background chess
            GameObject temp1 = (GameObject)Instantiate(GamePrefabs.instance.playingObjectBackPrefab, temp.transform.position - new Vector3(0, 0, 1), Quaternion.identity);
            temp1.transform.localEulerAngles = new Vector3(90, 0, 0);
        }        
    }//PopulateInitialColumn_end

    internal void AssignNeighbours()
    {
        for (int i = 0; i < playingObjectsScriptList.Count; i++)
        {
            if (playingObjectsScriptList[i] == null)
                continue;

            if (columnIndex == 0) //left
                ((PlayingObject)playingObjectsScriptList[i]).adjacentItems[0] = null;
            else
                ((PlayingObject)playingObjectsScriptList[i]).adjacentItems[0] = ((PlayingObject)ColumnManager.instance.gameColumns[columnIndex - 1].playingObjectsScriptList[i]);

            if (columnIndex == ColumnManager.instance.gameColumns.Length - 1) // right
                ((PlayingObject)playingObjectsScriptList[i]).adjacentItems[1] = null;
            else
                ((PlayingObject)playingObjectsScriptList[i]).adjacentItems[1] = ((PlayingObject)ColumnManager.instance.gameColumns[columnIndex + 1].playingObjectsScriptList[i]);

            if (i == 0) //up
                ((PlayingObject)playingObjectsScriptList[i]).adjacentItems[2] = null;
            else
                ((PlayingObject)playingObjectsScriptList[i]).adjacentItems[2] = ((PlayingObject)playingObjectsScriptList[i - 1]);

            if (i == numberOfRows - 1) // down
                ((PlayingObject)playingObjectsScriptList[i]).adjacentItems[3] = null;
            else
                ((PlayingObject)playingObjectsScriptList[i]).adjacentItems[3] = ((PlayingObject)playingObjectsScriptList[i + 1]);
        }
    }
    
    // change the item to a new 
    internal void ChangeItem(int index, GameObject newItemPrefab,string _name)  
    {
        GameObject temp = (GameObject)Instantiate(newItemPrefab, Vector3.zero, Quaternion.identity);
        temp.GetComponent<PlayingObject>().myColumnScript = this;
        temp.GetComponent<PlayingObject>().indexInColumn = index;
        playingObjectsScriptList[index] = temp.GetComponent<PlayingObject>();
        temp.transform.parent = transform;
        temp.transform.localPosition = new Vector3(0, -index * GameManager.instance.gapBetweenObjects, 0);
        temp.GetComponent<SpecialPlayingObject>().name = _name;
        iTween.ScaleFrom(temp, Vector3.zero, .6f);
    }

    internal void DeleteBrustedItems()
    {
        for (int i = 0; i < numberOfRows; i++)
        {
            if (playingObjectsScriptList[i] != null)
            {
                if (((PlayingObject)playingObjectsScriptList[i]).brust)
                {
                    ((PlayingObject)playingObjectsScriptList[i]).DestroyMe();

                    GameObject specialObject = ((PlayingObject)playingObjectsScriptList[i]).WillFormSpecialObject(); //compose the speical object

                    if (specialObject!= null)
                    {
                        ChangeItem(i, specialObject, ((PlayingObject)playingObjectsScriptList[i]).name);
                    }
                    else
                        playingObjectsScriptList[i] = null;
                    
                    
                }
            }
        }

        int count = 0;
        for (int i = 0; i < playingObjectsScriptList.Count; i++,count++)
        {
            if ((PlayingObject)playingObjectsScriptList[i] == null && itemAvailability[count] >= 1)
            {
                playingObjectsScriptList.RemoveAt(i);
                i--;
            }
        }

        numberOfItemsToAdd = numberOfRows - playingObjectsScriptList.Count;
    }

    //all the chesses should fall to get the empty position to be on top
    void ArrangeColumnItems()
    {
        for (int i = numberOfRows - 1; i >= 0; i--)
        {
            if (itemAvailability[i] >= 1 && playingObjectsScriptList[i] == null)
            {
                for (int j = i; j >= 0; j--)
                {
                    if (playingObjectsScriptList[j] != null)
                    {                        
                        playingObjectsScriptList[i] = playingObjectsScriptList[j];
                        playingObjectsScriptList[j] = null;
                        break;
                    }
                }
            }
        }
    }

    internal int GetNumberOfItemsToAdd()
    {
        return numberOfRows - playingObjectsScriptList.Count;
    }

    internal void AddMissingItems()
    {
        numberOfItemsToAdd = numberOfRows - playingObjectsScriptList.Count;
        if (numberOfItemsToAdd == 0)
            return;
        
        for (int i = 0; i < numberOfItemsToAdd; i++)
        {
            GameObject temp = (GameObject)Instantiate(GameManager.instance.playingObjectPrefabs[Random.Range(0, 6)], Vector3.zero, Quaternion.identity);
            temp.GetComponent<PlayingObject>().myColumnScript = this;
            playingObjectsScriptList.Insert(0, temp.GetComponent<PlayingObject>()); //numberOfEmptySpace
            temp.transform.parent = transform;
            temp.transform.localPosition = new Vector3(0, (i + 1) * GameManager.instance.gapBetweenObjects, 0);
        }
        
        ArrangeColumnItems();

        for (int i = 0; i < playingObjectsScriptList.Count; i++)
        {
            if ((PlayingObject)playingObjectsScriptList[i] != null)
                ((PlayingObject)playingObjectsScriptList[i]).indexInColumn = i;
        }

        iTween.Defaults.easeType = iTween.EaseType.bounce;
        iTween.Defaults.easeType = GameManager.instance.objectfallingEase;

        fallingDuration = GameManager.instance.initialObjectFallingDuration * (.9f + numberOfItemsToAdd / 10f);

        GameManager.instance.objectFallingDuration = Mathf.Max(GameManager.instance.objectFallingDuration, fallingDuration);

        SoundFxManager.instance.PlayFallingSound();
        StartMovingDownOldPart();
        StartMovingDownNewPart();
        SoundFxManager.instance.Invoke("StopFallingSound", fallingDuration * .8f);
        Invoke("PlayColumnFallSound", fallingDuration * .2f);
    }

    void PlayColumnFallSound()
    {
        //SoundFxManager.instance.columnFallSound.Play();
    }

    void StartMovingDownOldPart()
    {
        for (int i = numberOfItemsToAdd; i < playingObjectsScriptList.Count; i++)
        {
            if(itemAvailability[i] >= 1)
                iTween.MoveTo(((PlayingObject)playingObjectsScriptList[i]).gameObject, new Vector3(transform.position.x, -i * GameManager.instance.gapBetweenObjects + transform.position.y, 0), fallingDuration);
        }
    }

    void StartMovingDownNewPart()
    {
        for (int i = 0; i < numberOfItemsToAdd; i++)
        {
            if (itemAvailability[i] >= 1)
                iTween.MoveTo(((PlayingObject)playingObjectsScriptList[i]).gameObject, new Vector3(transform.position.x, -i * GameManager.instance.gapBetweenObjects + transform.position.y, 0), fallingDuration);
        }       
    }

    internal void UnCheckTracedAttribute()
    {
        for (int i = 0; i < playingObjectsScriptList.Count; i++)
        {
            ((PlayingObject)playingObjectsScriptList[i]).isTraced = false;           
        }
    }

    internal void UnCheckBrustAttribute()
    {
        for (int i = 0; i < playingObjectsScriptList.Count; i++)
        {
            ((PlayingObject)playingObjectsScriptList[i]).brust = false;
        }
    }

    internal void AssignBrustToAllItemsOfName(string itemName)
    {
        for (int i = 0; i < playingObjectsScriptList.Count; i++)
        {
            if ((PlayingObject)playingObjectsScriptList[i])
            {
                if (((PlayingObject)playingObjectsScriptList[i]).name == itemName)
                    ((PlayingObject)playingObjectsScriptList[i]).AssignBurst("smoke");
            }
        }
    }
}
