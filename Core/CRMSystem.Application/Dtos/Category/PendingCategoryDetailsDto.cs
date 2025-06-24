namespace CRMSystem.Application.Dtos.Category
{
    public class PendingCategoryDetailsDto
    {
        public string Id { get; set; }
        public string? OldName { get; set; } 
        public string? NewName { get; set; } 

  
        public bool IsUpdated { get; set; }
 
    }
}
