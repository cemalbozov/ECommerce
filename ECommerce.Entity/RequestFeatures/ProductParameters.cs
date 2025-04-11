namespace ECommerce.Entity.RequestFeatures
{
    public class ProductParameters : RequestParameters
	{
        public uint MinPrice { get; set; }
        public uint MaxPrice { get; set; } = 300000;
        public bool ValidPriceRange => MaxPrice > MinPrice;
        public string? SearchTerm { get; set; }
        public ProductParameters()
        {
            OrderBy = "dateAdded";
        }

    }
}
