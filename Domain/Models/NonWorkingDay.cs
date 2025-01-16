using Domain.Enums;

namespace Domain.Models
{
    public class NonWorkingDay
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public NonWorkingDayReason Reason { get; set; }

        public void Validate()
        {
            if (Date == null)
                throw new ArgumentNullException("Must select a Date");
            if (Reason == null)
                throw new ArgumentNullException("Must Select a Reason");
        }
    }
}
