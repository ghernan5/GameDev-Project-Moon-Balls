using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Dictionary<string, GameObject> loadedCells = new Dictionary<string, GameObject>();

    public GameObject Player;
    public GameObject currentCell;
    public MapMarker[] currentMarkers;
    //Map coords will be (0,0)
    public int xCoord = 0;
    public int yCoord = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        LoadCell(xCoord, yCoord);

        Player.transform.position = Vector3.zero;

        currentMarkers = currentCell.GetComponentsInChildren<MapMarker>();
    }

    void Update()
    {
        
    }

    private void LoadCell(int x, int y)
    {
        if (currentCell != null)
            currentCell.SetActive(false);

        string key = $"{x}_{y}";

        if (loadedCells.TryGetValue(key, out GameObject savedCell))
        {
            Debug.Log("Reusing saved cell");
            currentCell = savedCell;
            currentCell.SetActive(true);
            currentMarkers = currentCell.GetComponentsInChildren<MapMarker>();
            return;
        }

        Debug.Log("Generating a new cell");
        GameObject prefab = Resources.Load<GameObject>($"Cells/{key}");
        if (prefab == null)
        {
            Debug.LogError($"Missing cell prefab: {key}");
            return;
        }

        currentCell = Instantiate(prefab);
        loadedCells[key] = currentCell;
        currentMarkers = currentCell.GetComponentsInChildren<MapMarker>();
    }


    public void UpdateCell(string direction)
    {
        //Should happen when player walks into a marker
        //This should unload the current cell, update based on markers, load new cell, and place proper position of player
        //should get direction from gameobject 
        switch (direction)
        {
            case "North": yCoord++; break;
            case "West": xCoord--; break;
            case "South": yCoord--; break;
            case "East": xCoord++; break;
            default: break;
        }
        Debug.Log("Coordinates: " + xCoord + ", " + yCoord);

        StoreCellState(); //will stay as a prefab in world
        
        LoadCell(xCoord, yCoord);

        string entryName = direction switch
        {
            "North" => "SouthEntry",
            "South" => "NorthEntry",
            "East"  => "WestEntry",
            "West"  => "EastEntry",
            _ => ""
        };
        Transform entryPoint = currentCell.transform.Find(entryName);

        if (entryPoint != null)
            Player.transform.position = entryPoint.position;
        else
            Player.transform.position = Vector3.zero;
    }

    void StoreCellState()
    {
        //deactivate cell and store it
        currentCell.SetActive(false);
        
    }

}
