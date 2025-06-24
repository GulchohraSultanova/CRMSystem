namespace CRMSystem.Application.Dtos.Category
{
    public class UpdatePendingCategoryDto
    {
        public string Id { get; set; }               // Mövcud Category ID-si
        public string? NewName { get; set; }         // Yeni ad (əgər dəyişibsə)
// Yeni ölçü vahidi (əgər dəyişibsə)
    }
}
