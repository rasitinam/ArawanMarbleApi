public class ProductUpdateDto
{
    public string ProductName { get; set; }
    public string Description { get; set; }
    public IFormFile ProductImage { get; set; }
}