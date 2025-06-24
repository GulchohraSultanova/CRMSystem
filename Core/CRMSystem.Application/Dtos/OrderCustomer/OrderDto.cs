using CRMSystem.Application.Dtos.Account;
using CRMSystem.Application.Dtos.OrderItem;
using CRMSystem.Application.Dtos.OrderOverheadDtos;
using CRMSystem.Application.Dtos.Section;
namespace CRMSystem.Application.Dtos.OrderCustomer;
public class OrderDto
{
    public string Id { get; set; }

    public string CreatedDate { get; set; }       // dd.MM.yyyy
    public string OrderLimitTime { get; set; }    // dd.MM.yyyy
    public string OrderDeliveryTime { get; set; } // dd.MM.yyyy

    public bool EmployeeConfirm { get; set; }
    public bool FighterConfirm { get; set; }
    public bool EmployeeDelivery { get; set; }

    public SectionDto Section { get; set; }
    public AdminInfoDto AdminInfo { get; set; }
    public AdminInfoDto FighterInfo { get; set; }

    public List<OrderItemDto> Items { get; set; }
    public List<string> OverheadNames { get; set; }
}