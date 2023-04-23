namespace BasicAPI.Models
{
    public class BaseState
    {
        public string? Name { get; set; }
        public int CountryId { get; set; }
    }

    public class State: BaseState
    {
        public int Id { get; set; }
    }
}
