namespace Domain.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; }

        public void Validate()
        {
            if(StartTime == null)
                throw new ArgumentNullException("Must declare a Startime");
            if (EndTime == null)
                throw new ArgumentNullException("Must declare an Endtime");

        }
    }

}
