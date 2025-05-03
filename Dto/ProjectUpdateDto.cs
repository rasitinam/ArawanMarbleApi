public class ProjectUpdateDto
{
    public string ProjectName { get; set; }
    public string Description { get; set; }
    public IFormFile ProjectImage { get; set; }
    public string Subject { get; set; }
    public string? Projectplace { get; set; }
}
