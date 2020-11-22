namespace ArraySorter.ReadModels.Models
{
    public static class JobExtensions
    {
        public static Job ToReadModel(this Domain.Job self) => new Job
        {
            Id = self.Id,
            Input = self.Input,
            Output = self.Output,
            Timestamp = self.Timestamp,
            Duration = self.Duration?.TotalMilliseconds,
            Status = (Status)self.Status,
        };
    }
}
