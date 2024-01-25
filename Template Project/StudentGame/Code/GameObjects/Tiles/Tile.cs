namespace StudentGame.Code.GameObjects.Tiles
{
    public enum TileType { Normal = 0, Blocked, Slow };

    // A faithful recreation of Java OOP's Tile class
    public class Tile
    {
        public TileType Type { get; set; }
        public Texture2D TileImage { get; set; }

        public Tile()
        {
            Type = TileType.Normal;
            TileImage = null;
        }

        public Tile(Texture2D image, TileType type)
        {
            TileImage = image;
            Type = type;
        }
    }
}
