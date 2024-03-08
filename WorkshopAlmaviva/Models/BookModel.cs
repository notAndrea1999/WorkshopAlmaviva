namespace WorkshopAlmaviva.Models
{
    public class BookModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public DateOnly ReleaseDate { get; set; }

        public bool Availability { get; set; }

        public string AvilabilityStr
        {
            get
            {
                return Availability.ToString();
            }
        }
    }
}
