/***
 * 
  Title: "Diamond Crash" Project
 *           
 *    Game Manager  
 *           
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

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject[] playingObjectPrefabs;
    public GameObject[] horizontalPrefabs;
    public GameObject[] verticalPrefabs;
    public GameObject universalPlayingObjectPrefab;
    public GameObject[] jellyPrefab;


    internal int numberOfColumns;                          //the number of column
    internal int numberOfRows;                             //the number of row
    public float gapBetweenObjects = .7f;
    public float swappingTime = .8f;
    public float objectFallingDuration = .5f;
    internal float initialObjectFallingDuration;
    internal bool isBusy = false;
    public int totalNoOfJellies = 0;

    public iTween.EaseType objectfallingEase;

    internal TextMesh scoreText;
    internal TextMesh jellyText;
    internal TextMesh goalText;
    int score;

    internal static int numberOfItemsPoppedInaRow = 0;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        scoreText = GameObject.Find("Score Text").GetComponent<TextMesh>();
        goalText = GameObject.Find("Goal").GetComponent<TextMesh>();
        jellyText = GameObject.Find("Jelly Text").GetComponent<TextMesh>();
        initialObjectFallingDuration = objectFallingDuration;
        numberOfColumns = LevelStructure.instance.numberOfColumns;
        numberOfRows = LevelStructure.instance.numberOfRows;
        numberOfItemsPoppedInaRow = 0;
        scoreText.text = "Score : " + score.ToString();
        jellyText.text = "Jelly : " + totalNoOfJellies.ToString();
        goalText.text = "Goal: " + LevelStructure.instance.completeScore.ToString();

    }

    internal void AddScore()
    {
        int temp = 10 * numberOfItemsPoppedInaRow * (numberOfItemsPoppedInaRow / 5 + 1);
        score += temp;
        scoreText.text = "Score : " + score.ToString();
    }


    void Update()
    {
        if (score >= LevelStructure.instance.completeScore && isBusy == false)
        {
            Application.LoadLevel(9);
        }
    }

    IEnumerator Waitfor(float second)
    {
        yield return new WaitForSeconds(second);
    }
}
