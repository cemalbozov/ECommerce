namespace ECommerce.Entity.DTO
{
    public record CategoryDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Url { get; init; }
    }
}
