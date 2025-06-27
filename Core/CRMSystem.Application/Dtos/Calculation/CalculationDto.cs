public class CalculationDto
{
    public string Id { get; set; }
    public string CompanyId { get; set; }
    public string CompanyName { get; set; }
    public int Year { get; set; }
    public string MonthName { get; set; }
    public decimal InitialAmount { get; set; }
    public decimal OrderTotalAmount { get; set; }
    public decimal TotalAmount { get; set; }
}
