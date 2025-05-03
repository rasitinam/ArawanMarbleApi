namespace ArawanMarbleApi.Dto
{
    public class SubProductUploadDto
    {
        public int Productid { get; set; }

        public IFormFile Productimg { get; set; } = null!;
    }

}
