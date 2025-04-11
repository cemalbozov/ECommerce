namespace ECommerce.Entity.Exceptions
{
    public class PriceOutOfRangeBadRequestException : BadRequestException
    {
        public PriceOutOfRangeBadRequestException() 
            : base("Maximum price should be less than 300000")
        {
        }
    }
}
