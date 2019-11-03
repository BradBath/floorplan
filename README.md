# floorplan 2.0.0

I have taken the liberty of modifying floorplan and completing most of the ToDos that Alexis Morin had put in the Readme in original repository.

Changes:

    * New completely revamped UI and controls. You can now just select the floorplan handle and just draw directly onto your scene. The UI now lets you choose how you want to place objects (filled rectangle, hollow rectangle) and which materials to apply to what you are drawing
    * Overlap prevention. Objects will now check on creation if there's another object at that position (via OverlapSphere) and will remove any duplicate parts.
    * New tileset functionality. Tilesets now have 5 groups of objects they can place instead of 5 individual gameobjects. These groups follow the same structure as before (WallTiles, FloorTiles, ArchTiles, PillarTiles, and WindowTiles)
    * Basic modding capabilities. Obviously, with access to the source code, you can change anything you want. I have implemented the strategy design pattern while making the drawing tools which means you are/should be able to easily create new drawing tools given you have programming knowledge

##Demonstration
[![3 minute demonstration of my changes](https://img.youtube.com/vi/IMBXjzlqeQA/0.jpg)](https://www.youtube.com/watch?v=IMBXjzlqeQA)
