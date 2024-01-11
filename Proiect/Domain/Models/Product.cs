namespace Domain.Models;

public record Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int Price { get; set; }

    public int Stock { get; set; }
};