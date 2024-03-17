using System.ComponentModel.DataAnnotations;

namespace Comp1640_Final.Models
{
    public enum EPublishStatus
    {
        [Display(Name = "Approved")]
        Approval = 1,
        [Display(Name = "Not Approved")]
        NotApproval = 2,
        [Display(Name = "Pending")]
        Pending = 3,

    }
}
