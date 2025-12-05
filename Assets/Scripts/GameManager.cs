using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            Destroy(currentCell);

        string newCell = $"{x}_{y}";

        GameObject prefab = Resources.Load<GameObject>($"Cells/{newCell}");

        if (prefab == null)
        {
            Debug.LogError($"Missing cell prefab: {newCell}");
            return;
        }

        currentCell = Instantiate(prefab);

        // Cache markers
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

        LoadCell(xCoord, yCoord);

            // Determine the entry object name (opposite of direction moved)
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

}
