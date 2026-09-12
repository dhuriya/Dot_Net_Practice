namespace CrudOperationWithDto;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string Description {get;set;}
}
//Single Responsibility principle (SRP) in SOLID Principles:
// A class should have only single responsiblity and single reason to chnage
