namespace Domain.Models;

public record Order
{
    public int Id { get; set; }
    public string Address { get; set; }

    public string Email { get; set; }

    public int Total { get; set; }
    
};