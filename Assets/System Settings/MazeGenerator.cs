using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/*
Creating a tileset in Unity and using it to generate a specific maze involves several steps, from setting up the tileset to scripting the maze generation. Here's a detailed guide on how to do this:

### Step 1: Preparing Your Tileset

1. **Crop the Tileset**:
   - Use an image editor to crop your maze into individual tiles as described previously. Save each tile as a separate image or arrange them into a single tileset image grid.

2. **Import Tileset to Unity**:
   - Open your Unity project.
   - Drag and drop your tileset image(s) into the `Assets` folder in Unity.

### Step 2: Setting Up the Tileset in Unity

1. **Sprite Settings**:
   - Select the tileset image in the `Assets` folder.
   - In the `Inspector` window, set the `Sprite Mode` to `Multiple`.
   - Click on `Sprite Editor` and slice the image into individual tiles using the `Grid By Cell Size` option (e.g., 32x32 pixels).
   - Apply the changes.

2. **Create Tile Assets**:
   - In the `Assets` folder, create a new folder called `Tiles`.
   - Right-click in the `Tiles` folder, select `Create` -> `2D` -> `Tiles` -> `Tile`.
   - Name the tile according to the component it represents (e.g., "HorizontalPath", "VerticalPath", etc.).
   - Drag each sprite from the tileset into the `Sprite` field of the corresponding tile asset.

### Step 3: Setting Up the Tilemap

1. **Create Tilemap**:
   - In the `Hierarchy` window, right-click and select `2D Object` -> `Tilemap` -> `Rectangular`.
   - This will create a `Grid` object with a `Tilemap` child object.
   - Ensure the `Tilemap Renderer` is set to the correct sorting layer and order.

### Step 4: Creating the Maze with the Game Manager

1. **Design the Maze Layout**:
   - Represent the maze layout in a 2D array where each element corresponds to a tile type (e.g., 0 for empty, 1 for horizontal path, 2 for vertical path, etc.).

2. **Script to Generate Maze**:
   - Create a script called `MazeGenerator` and attach it to an empty GameObject called `GameManager`.

```csharp
using UnityEngine;
using UnityEngine.Tilemaps;

public class MazeGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase horizontalPath;
    public TileBase verticalPath;
    public TileBase innerCornerTL;
    public TileBase innerCornerTR;
    public TileBase innerCornerBL;
    public TileBase innerCornerBR;
    // Add other tile references as needed

    private int[,] mazeLayout = {
        { 1, 2, 1, 1, 2 }, // Example layout
        { 2, 0, 0, 0, 2 },
        { 1, 1, 1, 1, 1 },
        { 2, 0, 2, 0, 2 },
        { 1, 1, 1, 1, 1 }
    };

    void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        for (int y = 0; y < mazeLayout.GetLength(0); y++)
        {
            for (int x = 0; x < mazeLayout.GetLength(1); x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, -y, 0); // Adjust if necessary
                TileBase tile = GetTileByType(mazeLayout[y, x]);
                if (tile != null)
                {
                    tilemap.SetTile(tilePosition, tile);
                }
            }
        }
    }

    TileBase GetTileByType(int type)
    {
        switch (type)
        {
            case 1:
                return horizontalPath;
            case 2:
                return verticalPath;
            case 3:
                return innerCornerTL;
            case 4:
                return innerCornerTR;
            case 5:
                return innerCornerBL;
            case 6:
                return innerCornerBR;
            // Add cases for other tile types
            default:
                return null;
        }
    }
}
```

3. **Assign Tile References**:
   - In the `Inspector`, select the `GameManager` object and assign the corresponding tile assets to the `MazeGenerator` script fields.
   - Assign the `Tilemap` reference as well.

### Step 5: Testing

1. **Run the Game**:
   - Press `Play` to start the game. The `MazeGenerator` script should read the `mazeLayout` array and place the tiles accordingly on the tilemap.

### Notes

- Adjust the maze layout array (`mazeLayout`) to match your specific maze design.
- Ensure that the tile positions are correct (e.g., you might need to adjust the y-coordinate).
- You can expand the `GetTileByType` method to handle more tile types as needed.

By following these steps, you can create a tileset in Unity, use it to generate a maze dynamically, and apply the necessary colliders to each tile.
*/
