using ATA.HR.Client.Web.APIs.HRInsuranceModels;
using ATA.HR.Client.Web.APIs.Insurance.Models;
using ATA.HR.Client.Web.APIs.Insurance.Models.Requests;
using ATA.HR.Client.Web.APIs.Insurance.Models.Responses;
using ATABit.Shared;
using ATABit.Shared.Workflow;
using Refit;
using System.Threading;

namespace ATA.HR.Client.Web.APIs;

public interface IInsuranceAPIs
{
    [Get("/insurance/api/User/GetCurrentUserData")]
    Task<UserDto> Insurance_GetCurrentUserData();

    [Get("/insurance/api/Insured/GetMyInsureds")]
    Task<List<InsuredDto>> Insurance_GetMyInsureds();

    [Post("/insurance/odata/AllInsureds")] //Only usable when we need no odata query params
    Task<ODataResponse<InsuredDto>> Insurance_GetAllInsureds([Body] GetAllInsuredsQuery query, ODataParameters odataParams, CancellationToken cancellationToken);

    [Get("/insurance/api/Policy/GetActivePolicyObligations")]
    Task<List<PolicyObligationDto>> Insurance_GetActivePolicyObligations();

    [Post("/insurance/api/Insured/CancelInsurance/{insuredId}")]
    Task Insurance_CancelInsurance(int insuredId);

    [Get("/insurance/api/Insured/GetAllInsuranceStatusTypes")]
    Task<List<SelectListItem>> Insurance_GetAllInsuranceStatusTypes();

    [Get("/insurance/api/Insured/GetAllLifeInsuranceStatusTypes")]
    Task<List<SelectListItem>> Insurance_GetAllLifeInsuranceStatusTypes();

    [Post("/insurance/api/Cost/GetMyCosts")]
    Task<List<CostDto>> Insurance_GetMyCosts([Body] GetCostsQuery query);

    [Post("/insurance/api/Cost/GetAllCostsSum")]
    Task<long> Insurance_GetAllCostsSum([Body] GetCostsQuery query);

    [Post("/insurance/api/Cost/SetCost")]
    Task<CostDto?> Insurance_AddCost([Body] SetCostCommand command);

    [Post("/insurance/api/Cost/SetCost")]
    Task Insurance_EditCost([Body] SetCostCommand command);

    [Delete("/insurance/api/Cost/DeleteCostById/{costId}")]
    Task Insurance_DeleteCost(int costId);

    [Get("/insurance/api/Cost/GetCostByIdForForm/{costId}")]
    Task<SetCostCommand> Insurance_GetCostByIdForForm(int costId);

    [Post("/insurance/api/Insured/ReviveMainInsuredToActiveState/{personnelCode}")]
    Task Insurance_ReviveMainInsuredToActiveState(string personnelCode);

    [Post("/insurance/api/Insured/ReviveInsuredToActiveState/{insuredId}")]
    Task Insurance_ReviveInsuredToActiveState(int insuredId);

    [Get("/insurance/api/Workflow/GetPossibleActions/{costId}")]
    Task<PossibleActions> Insurance_GetPossibleActions(int costId);

    [Get("/insurance/api/Workflow/GetCostFlowHistory/{costId}")]
    Task<WorkFullHistory> Insurance_GetCostFlowHistory(int costId);

    [Get("/insurance/api/Workflow/GetCostFlowForms/{costId}")]
    Task<InsuranceFlowFormsDto> Insurance_GetCostFlowForms(int costId);

    [Post("/insurance/api/Workflow/DoActionOnCost")]
    Task Insurance_DoActionOnCost([Body] DoActionArgs action);

    [Post("/insurance/api/Cost/ConfirmCost")]
    Task Insurance_ConfirmCost([Body] ConfirmCostCommand command);

    [Get("/insurance/api/Cost/GetRemainedBalance")]
    Task<int> Insurance_GetRemainedBalance([Query] GetRemainedBalanceQuery query);

    [Patch("/insurance/api/Cost/UpdateCostAmountAndObligationType")]
    Task Insurance_UpdateCostAmountAndObligationType([Body] UpdateCostAmountAndObligationTypeCommand command);
    
    [Post("/insurance/api/BankPayment/GetFinancialReport")]
    Task<List<FinancialReportDto>> Insurance_GetFinancialReport([Body] GetFinancialReportQuery query);
    
    [Get("/insurance/api/BankPayment/GetAllBankPayments")]
    Task<List<BankPaymentDto>> Insurance_GetAllBankPayments();
    
    [Post("/insurance/api/BankPayment/SaveBankPayment")]
    Task Insurance_SaveBankPayment([Body] SaveBankPaymentCommand command);
}