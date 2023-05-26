namespace LoggingKata
{
    public class TacoParser
    {
        readonly ILog logger = new TacoLogger();
        
        public ITrackable Parse(string line)
        {
            logger.LogInfo("Begin parsing");

            var cells = line.Split(',');

            if (cells.Length < 3)
            {
                logger.LogWarning("Less than three items. Incomplete data.");
                return null; 
            }

            var latitude = double.Parse(cells[0]);

            var longitude = double.Parse(cells[1]);

            var name = cells[2];

            var newPoint = new Point();
            newPoint.Latitude = latitude;
            newPoint.Longitude = longitude;

            var tacoBell = new TacoBell();
            tacoBell.Name = name;
            tacoBell.Location = newPoint;

            return tacoBell;
        }
    }
}