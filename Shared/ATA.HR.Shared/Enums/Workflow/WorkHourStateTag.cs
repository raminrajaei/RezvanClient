namespace ATA.HR.Shared.Enums.Workflow;

public enum WorkHourStateTag //These items shouldn't be changed because the string is important here not the enum value
{
    StartWorkHourFlowByHR,

    EditAndFinalizeWorkHourByHRSupervisor, //Mrs. Akbari

    ConfirmWorkHourByHRManagement, //Mr. Asadi

    ConfirmWorkHourByEmployeeDirectManager, //e.g. Mr. Arefi

    //ConfirmWorkHourAfterDirectManagerConfirmationByHRManagement, //Mr. Asadi again [Deleted]

    EditAndConfirmWorkHourByCEO, //Mr. Ghiasi

    //ViewByFinancialDepartment, //Mrs. Najafi [Deleted]

    Finish
}