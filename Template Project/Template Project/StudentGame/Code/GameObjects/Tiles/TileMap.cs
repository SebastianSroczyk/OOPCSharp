using System.Collections.Generic;
using System.IO;

namespace StudentGame.Code.GameObjects.Tiles
{
    // A slightly re-jigged (but still somewhat faithful) MonoGameEngine-specific TileMapManager class
    public class TileMap
    {
        private const int TILE_SIZE = 64;
        private Tile[] _tiles;
        /* Multi-Dimensional array structure
        int[,] _map =
        {   { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2 },
            { 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2 }
        };
        */

        /* Jagged array structure */
        int[][] _map =
        {
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new int[] { 0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0 },
            new int[] { 0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 1, 1, 0, 0 },
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2 },
            new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2 },
            new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2 }
        };

        public TileMap()
        {
            LoadTiles();
            LoadMap("tilemap");
        }

        private void LoadTiles()
        {
            _tiles = new Tile[3];
            try
            {
                _tiles[0] = new Tile(Core.GetResource<Texture2D>("images/col1"), TileType.Normal);
                _tiles[1] = new Tile(Core.GetResource<Texture2D>("images/col2"), TileType.Slow);
                _tiles[2] = new Tile(Core.GetResource<Texture2D>("images/col3"), TileType.Blocked);
            }
            catch (IOException)
            {
                throw new IOException("Error loading tiles");
            }
        }

        /// <summary>
        /// Method which reads a text file of comma separated integer characters and parses it into a 2d array of integers. <br/> 
        /// <strong>The desired text file should be passed through the MGCB Editor as usual, but should be marked as Copy rather than Build in the Build Action settings.</strong>
        /// </summary>
        /// <param name="filename"></param>
        private void LoadMap(string filename)
        {
            List<int[]> tileColumn = new();
            
            using (StreamReader streamReader = new StreamReader(Core.RootContentDirectory + "/" + filename + ".txt"))
            { 
                while(!streamReader.EndOfStream)
                {
                    List<int> tileRow = new();
                    string[] row = streamReader.ReadLine().Split(',');
                    foreach(var num in row)
                    {
                        tileRow.Add(int.Parse(num));
                    }

                    tileColumn.Add(tileRow.ToArray());
                }
            }
            
            _map = tileColumn.ToArray();
        }

        private int MatchTile(int tileMapID)
        {
            int mappedTile = 0;

            switch (tileMapID)
            {
                case 1:
                    mappedTile = 0;
                    break;
                case 2:
                    mappedTile = 1;
                    break;
                case 3:
                    mappedTile = 2;
                    break;
            }

            return mappedTile;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle cameraBounds = Camera.Instance.ViewBounds;
            for(int i = 0; i < _map.Length; i++) 
            {
                for(int j = 0; j < _map[i].Length; j++)
                {
                    Rectangle tileRect = new Rectangle(j * TILE_SIZE, i * TILE_SIZE, TILE_SIZE, TILE_SIZE);
                    if(cameraBounds.Intersects(tileRect)) //Only render the tile if they are visible
                    {
                        if (_map[i][j] != 0)
                        {
                            spriteBatch.Draw(_tiles[MatchTile(_map[i][j])].TileImage, tileRect.Location.ToVector2(), Color.White);
                        }
                    }
                }
            }
        }

        /* EXTRAS */

        public Vector2 ConvertWorldPositionToGridPosition(Vector2 worldPosition)
        {
            return new Vector2((int)(worldPosition.X / TILE_SIZE), (int)(worldPosition.Y / TILE_SIZE));
        }

        public Tile GetTileAtWorldPosition(Vector2 worldPosition)
        {
            Vector2 gridPosition = ConvertWorldPositionToGridPosition(worldPosition);
            return GetTileAtGridPosition(gridPosition);
        }

        public Tile GetTileAtGridPosition(Vector2 gridPosition)
        {
            if (gridPosition.Y >= 0 && gridPosition.Y < _map.Length)
            {
                if (gridPosition.X >= 0 && gridPosition.X < _map[(int)gridPosition.Y].Length)
                {
                    return _tiles[MatchTile(_map[(int)gridPosition.Y][(int)gridPosition.X])];
                }
            }

            return null;
        }
    }
}
