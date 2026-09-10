namespace CrudOperationWithRepo;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Description {get;set;}
}
// single responibility principle (SRP) states that a class should have only one reason to change, meaning it should be
