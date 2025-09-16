namespace RealState.Core.DTOs;

public class PropertyDto
{
    public string Id { get; set; } = string.Empty;
    public string IdOwner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? Image { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public OwnerDto? Owner { get; set; }
}

public class PropertyDetailDto : PropertyDto
{
    public List<PropertyImageDto> Images { get; set; } = new();
    public List<PropertyTraceDto> Traces { get; set; } = new();
}

public class PropertyImageDto
{
    public string Id { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}

public class PropertyTraceDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
}
