using System.ComponentModel.DataAnnotations;

namespace ATA.HR.Shared.Enums.Workflow;

public enum ActionType
{
    [Display(Name = "ارسال")]
    Request = 0,

    [Display(Name = "تایید و امضا")]
    Confirm = 1,

    [Display(Name = "رد")]
    Reject = 2
}

public static class ActionTypeExtensions
{
    public static bool IsRejectAction(this ActionType action)
    {
        return action is ActionType.Reject;
    }
}