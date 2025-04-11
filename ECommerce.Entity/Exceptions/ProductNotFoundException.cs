namespace ECommerce.Entity.Exceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id) : base($"The product with id : {id} could not found!")
        {
        }
    }

    public sealed class CategoryNotFoundException
        : NotFoundException
    {
        public CategoryNotFoundException(int id) 
            : base($"The category with id : {id} could not found!")
        {
        }
    }

}
