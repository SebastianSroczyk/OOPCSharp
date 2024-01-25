using System;
using System.IO;

namespace StudentGame.Code.GameObjects.Tiles
{
    // More closely resembles the Java OOP example, but doesn't work quite so well with MonoGameEngine
    internal class TileMapManager
    {
        private const int TILE_SIZE = 64;

        private Tile[] _tiles;

        private readonly int[][] _map =
        {
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 1, 1, 1, 1, 0, 0, 0, 0, 0 ,0, 1, 1, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 3, 3, 0, 0, 0, 1, 1, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
            new int[] {2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2},
            new int[] {2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 2, 2},
            new int[] {2, 2, 2, 2, 2, 2, 2, 2, 0, 0, 0, 0, 0, 0, 0, 2}
        };

        private Vector2 _cameraCenter;

        private int _xMin;
        private int _xMax;
        private int _yMin;
        private int _yMax;

        private int _numberOfColumns;
        private int _numberOfRows;

        private int _rowOffset;
        private int _columnOffset;

        private int _numColumnsToDraw;
        private int _numRowsToDraw;

        public TileMapManager()
        {
            int width;
            int height;

            //Camera2D.Instance.ClampWithinWorld = true;

            _numberOfRows = _map.Length;
            _numberOfColumns = _map[0].Length;

            _numColumnsToDraw = (int)Settings.GameResolution.X / TILE_SIZE + 2;
            _numRowsToDraw = (int)Settings.GameResolution.Y / TILE_SIZE + 2;

            width = _numColumnsToDraw * TILE_SIZE;
            height = _numRowsToDraw * TILE_SIZE;

            _xMin = (int)Settings.GameResolution.X - width;
            _xMax = 0;
            _yMin = (int)Settings.GameResolution.Y - height;
            _yMax = 0;

            _cameraCenter = Camera.Instance.Origin;

            //LoadMap();
            LoadTiles();
        }

        private void LoadTiles()
        {
            _tiles = new Tile[3];
            try
            {
                _tiles[0] = new Tile(Core.GetResource<Texture2D>("images/col1"), TileType.Normal);
                _tiles[1] = new Tile(Core.GetResource<Texture2D>("images/col2"), TileType.Normal);
                _tiles[2] = new Tile(Core.GetResource<Texture2D>("images/col3"), TileType.Normal);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Error loading tiles");
            }
        }

        private Tile GetTileAt(float x, float y)
        {
            x = x - _cameraCenter.X;
            y = y - _cameraCenter.Y;

            int row = (int) y / TILE_SIZE;
            int column = (int) x / TILE_SIZE;

            int tileID = _map[row][column];

            tileID = MatchTile(tileID);
            Console.WriteLine("X: " + x + ", Y: " + y);
            Console.WriteLine("In tile: " + row + ", " + column);
            return _tiles[tileID];
        }

        public void SetCameraPosition(float x, float y)
        {
            Camera.Instance.Position = new Vector2(x, y);
            Camera.Instance.ClampWithinWorld = true;
        }

        private int MatchTile(int tileMapID)
        {
            int mappedTile = 0;

            switch(tileMapID) 
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
            int tileImage;
            float tempX;
            float tempY;

            spriteBatch.Begin();
            spriteBatch.DrawString(Core.GetResource<SpriteFont>("misc/Arial"), 
                "Camera X: " + Camera.Instance.Position.X + " Camera Y: " + Camera.Instance.Position.Y, 
                new Vector2(20, 20), 
                Color.White);

            for (int row = _rowOffset; row < _rowOffset + _numRowsToDraw; row++) 
            {
                if (row >= _numberOfRows)
                    break;

                tempY = Camera.Instance.Position.Y + row * TILE_SIZE;
                for (int column = _columnOffset; column < _columnOffset + _numColumnsToDraw; column++)
                {
                    if (column >= _numberOfColumns)
                        break;

                    tempX = Camera.Instance.Position.X + column * TILE_SIZE;
                    if (_map[row][column] != 0)
                    {
                        tileImage = MatchTile(_map[row][column]);
                        spriteBatch.Draw(_tiles[tileImage].TileImage, new Vector2(tempX, tempY), Color.White);
                        spriteBatch.DrawRectangle(new Vector2(tempX, tempY), TILE_SIZE, TILE_SIZE);
                    }
                }
            }

            spriteBatch.End();
        }
    }
}
