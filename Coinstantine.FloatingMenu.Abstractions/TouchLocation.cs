namespace Coinstantine.FloatingMenu.Abstractions
{
    public class TouchLocation
    {
        public TouchLocation(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }
    }
}
