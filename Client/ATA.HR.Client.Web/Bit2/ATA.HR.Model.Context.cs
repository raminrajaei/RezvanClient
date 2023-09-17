
using Bit.Http.Implementations;
using System.Collections.Generic;
using Bit.Core.Implementations;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Bit.Http.Contracts;

namespace System.Net.Http
{
						namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoAppSMSDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoJobSettingsDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoAppVersioningDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoCommitmentLetterDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoContractDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoInstructionDocumentDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoPersonnelDocumentDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoBedDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoBookingDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoBuildingDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoRoomDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoUnitDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoPaySlipDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoRoleDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoUserDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoActionDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoDashboardDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoFlowFormDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoWorkHourDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
								namespace Auto { [System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")] public class AutoWorkHourFlowDirectionManagerCommentDto : Bit.Model.Contracts.IDto { public System.Guid Id { get; set; } } }
			
	[System.CodeDom.Compiler.GeneratedCode("BitCodeGenerator", "9.0.0.0")]
    public static class EmploymentContractContextExt2
    {
		
			public static ODataHttpClient<Auto.AutoAppSMSDto> AppSMS(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoAppSMSDto>(httpClient, "AppSMS" , "EmploymentContract" );
			}

			
				public static async Task<List<ATA.HR.Shared.Dtos.AppGeneric.AppSMSReadDto>> GetAllAppSMSes(this ODataHttpClient<Auto.AutoAppSMSDto> appSMSController,ATA.HR.Shared.Dtos.Contract.AllAppSMSesFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/AppSMS/GetAllAppSMSes(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await appSMSController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await appSMSController.DeserializeAsync<List<ATA.HR.Shared.Dtos.AppGeneric.AppSMSReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		
			public static ODataHttpClient<Auto.AutoJobSettingsDto> JobSettings(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoJobSettingsDto>(httpClient, "JobSettings" , "EmploymentContract" );
			}

			
				public static async Task<ATA.HR.Shared.Dtos.AppGeneric.DbAppSettings.NotifyDirectManagersToConfirmWorkHoursDbSettings?> GetNotifyManagersWorkHourJobSettings(this ODataHttpClient<Auto.AutoJobSettingsDto> jobSettingsController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/JobSettings/GetNotifyManagersWorkHourJobSettings(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await jobSettingsController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await jobSettingsController.DeserializeAsync<ATA.HR.Shared.Dtos.AppGeneric.DbAppSettings.NotifyDirectManagersToConfirmWorkHoursDbSettings?>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATABit.Shared.SelectListItem>> GetExcludedUsersFromNotifyManagersWorkHourJobSettings(this ODataHttpClient<Auto.AutoJobSettingsDto> jobSettingsController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/JobSettings/GetExcludedUsersFromNotifyManagersWorkHourJobSettings(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await jobSettingsController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await jobSettingsController.DeserializeAsync<List<ATABit.Shared.SelectListItem>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task UpdateNotifyManagersWorkHourJobSettings(this ODataHttpClient<Auto.AutoJobSettingsDto> jobSettingsController,ATA.HR.Shared.Dtos.AppGeneric.DbAppSettings.NotifyDirectManagersToConfirmWorkHoursDbSettings dbSettings,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/JobSettings/UpdateNotifyManagersWorkHourJobSettings(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							dbSettings
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await jobSettingsController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoAppVersioningDto> AppVersioning(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoAppVersioningDto>(httpClient, "AppVersioning" , "EmploymentContract" );
			}

			
				public static async Task<string> GetCurrentVersion(this ODataHttpClient<Auto.AutoAppVersioningDto> appVersioningController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/AppVersioning/GetCurrentVersion(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await appVersioningController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await appVersioningController.DeserializeAsync<string>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.AppVersioning.AppVersioningDto>> GetVersioningDetails(this ODataHttpClient<Auto.AutoAppVersioningDto> appVersioningController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/AppVersioning/GetVersioningDetails(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await appVersioningController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await appVersioningController.DeserializeAsync<List<ATA.HR.Shared.Dtos.AppVersioning.AppVersioningDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<string> GetUserLastVisitedVersion(this ODataHttpClient<Auto.AutoAppVersioningDto> appVersioningController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/AppVersioning/GetUserLastVisitedVersion(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await appVersioningController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await appVersioningController.DeserializeAsync<string>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task AddOrUpdateUserLastVisitedVersion(this ODataHttpClient<Auto.AutoAppVersioningDto> appVersioningController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/AppVersioning/AddOrUpdateUserLastVisitedVersion(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await appVersioningController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoCommitmentLetterDto> CommitmentLetter(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoCommitmentLetterDto>(httpClient, "CommitmentLetter" , "EmploymentContract" );
			}

			
				public static async Task<List<ATA.HR.Shared.Dtos.CommitmentLetter.CommitmentLetterReadDto>> GetAllCommitmentLetters(this ODataHttpClient<Auto.AutoCommitmentLetterDto> commitmentLetterController,ATA.HR.Shared.Dtos.CommitmentLetter.CommitmentLettersFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/CommitmentLetter/GetAllCommitmentLetters(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await commitmentLetterController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await commitmentLetterController.DeserializeAsync<List<ATA.HR.Shared.Dtos.CommitmentLetter.CommitmentLetterReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task AddCommitmentLetter(this ODataHttpClient<Auto.AutoCommitmentLetterDto> commitmentLetterController,ATA.HR.Shared.Dtos.CommitmentLetter.CommitmentLetterDto commitmentLetter,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/CommitmentLetter/AddCommitmentLetter(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							commitmentLetter
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await commitmentLetterController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task DeleteCommitmentLetter(this ODataHttpClient<Auto.AutoCommitmentLetterDto> commitmentLetterController,ATA.HR.Shared.Dtos.CommitmentLetter.DeleteCommitmentLetterArgs deleteArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/CommitmentLetter/DeleteCommitmentLetter(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							deleteArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await commitmentLetterController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoContractDto> Contract(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoContractDto>(httpClient, "Contract" , "EmploymentContract" );
			}

			
				public static async Task<int> SendContractToAUser(this ODataHttpClient<Auto.AutoContractDto> contractController,ATA.HR.Shared.Dtos.Contract.StartContractFlowDto startContractFlow,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/SendContractToAUser(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							startContractFlow
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await contractController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> AddNewContractPreview(this ODataHttpClient<Auto.AutoContractDto> contractController,ATA.HR.Shared.Dtos.Contract.ContractPreviewDto contractPreview,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/AddNewContractPreview(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							contractPreview
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await contractController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<bool> IsHourlyContract(this ODataHttpClient<Auto.AutoContractDto> contractController,int contractId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/IsHourlyContract(contractId={(contractId == null ? "null" : $"{contractId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await contractController.DeserializeAsync<bool>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> GetContractVersion(this ODataHttpClient<Auto.AutoContractDto> contractController,int contractId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/GetContractVersion(contractId={(contractId == null ? "null" : $"{contractId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await contractController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATA.HR.Shared.Dtos.Contract.ContractReadDto> GetContractById(this ODataHttpClient<Auto.AutoContractDto> contractController,int contractId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/GetContractById(contractId={(contractId == null ? "null" : $"{contractId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await contractController.DeserializeAsync<ATA.HR.Shared.Dtos.Contract.ContractReadDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.Contract.ContractReadDto>> GetMyContracts(this ODataHttpClient<Auto.AutoContractDto> contractController,ATA.HR.Shared.Dtos.MyContractsFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/GetMyContracts(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await contractController.DeserializeAsync<List<ATA.HR.Shared.Dtos.Contract.ContractReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.Contract.ContractReadDto>> GetAllContracts(this ODataHttpClient<Auto.AutoContractDto> contractController,ATA.HR.Shared.Dtos.Contract.AllContractsFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/GetAllContracts(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await contractController.DeserializeAsync<List<ATA.HR.Shared.Dtos.Contract.ContractReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task SendContractsRelatedSmses(this ODataHttpClient<Auto.AutoContractDto> contractController,ATA.HR.Shared.Dtos.Contract.ContractsSmsArgs smsArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/SendContractsRelatedSmses(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							smsArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task DeleteContract(this ODataHttpClient<Auto.AutoContractDto> contractController,ATA.HR.Shared.Dtos.Contract.DeleteContractArgs deleteArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/DeleteContract(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							deleteArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task<List<int>> GetEmploymentStatusIdListAllowedToSendContractTo(this ODataHttpClient<Auto.AutoContractDto> contractController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/GetEmploymentStatusIdListAllowedToSendContractTo(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await contractController.DeserializeAsync<List<int>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task EditContractStartDate(this ODataHttpClient<Auto.AutoContractDto> contractController,ATA.HR.Shared.Dtos.Contract.EditContractDateArgs args,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/EditContractStartDate(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							args
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task EditContractEndDate(this ODataHttpClient<Auto.AutoContractDto> contractController,ATA.HR.Shared.Dtos.Contract.EditContractDateArgs args,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Contract/EditContractEndDate(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							args
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await contractController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoInstructionDocumentDto> InstructionDocument(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoInstructionDocumentDto>(httpClient, "InstructionDocument" , "EmploymentContract" );
			}

			
				public static async Task<List<ATA.HR.Shared.Dtos.Document.InstructionDocumentReadDto>> GetInstructionDocuments(this ODataHttpClient<Auto.AutoInstructionDocumentDto> instructionDocumentController,ATA.HR.Shared.Dtos.Document.InstructionDocumentFilterArgs instructionDocumentFilters,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/InstructionDocument/GetInstructionDocuments(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							instructionDocumentFilters
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await instructionDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await instructionDocumentController.DeserializeAsync<List<ATA.HR.Shared.Dtos.Document.InstructionDocumentReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task AddDocument(this ODataHttpClient<Auto.AutoInstructionDocumentDto> instructionDocumentController,ATA.HR.Shared.Dtos.Document.InstructionDocumentDto instructionDocument,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/InstructionDocument/AddDocument(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							instructionDocument
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await instructionDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoPersonnelDocumentDto> PersonnelDocument(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoPersonnelDocumentDto>(httpClient, "PersonnelDocument" , "EmploymentContract" );
			}

			
				public static async Task<List<ATA.HR.Shared.Dtos.Document.PersonnelDocumentReadDto>> GetUserDocuments(this ODataHttpClient<Auto.AutoPersonnelDocumentDto> personnelDocumentController,ATA.HR.Shared.Dtos.Document.PersonnelDocumentFilterArgs personnelDocumentFilters,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/PersonnelDocument/GetUserDocuments(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							personnelDocumentFilters
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await personnelDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await personnelDocumentController.DeserializeAsync<List<ATA.HR.Shared.Dtos.Document.PersonnelDocumentReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.Document.DocSubCategoryReadDto>> GetAllDocumentsSubCategories(this ODataHttpClient<Auto.AutoPersonnelDocumentDto> personnelDocumentController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/PersonnelDocument/GetAllDocumentsSubCategories(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await personnelDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await personnelDocumentController.DeserializeAsync<List<ATA.HR.Shared.Dtos.Document.DocSubCategoryReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task AddDocument(this ODataHttpClient<Auto.AutoPersonnelDocumentDto> personnelDocumentController,ATA.HR.Shared.Dtos.Document.PersonnelDocumentDto personnelDocument,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/PersonnelDocument/AddDocument(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							personnelDocument
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await personnelDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task EditDocument(this ODataHttpClient<Auto.AutoPersonnelDocumentDto> personnelDocumentController,ATA.HR.Shared.Dtos.Document.PersonnelDocumentEditDto personnelDocumentToEdit,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/PersonnelDocument/EditDocument(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							personnelDocumentToEdit
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await personnelDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task<ATA.HR.Shared.Dtos.Document.PersonnelDocumentDto> GetDocumentByIdForForm(this ODataHttpClient<Auto.AutoPersonnelDocumentDto> personnelDocumentController,int documentId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/PersonnelDocument/GetDocumentByIdForForm(documentId={(documentId == null ? "null" : $"{documentId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await personnelDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await personnelDocumentController.DeserializeAsync<ATA.HR.Shared.Dtos.Document.PersonnelDocumentDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task DeleteDocument(this ODataHttpClient<Auto.AutoPersonnelDocumentDto> personnelDocumentController,ATA.HR.Shared.Dtos.Document.DeleteDocumentArgs deleteArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/PersonnelDocument/DeleteDocument(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							deleteArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await personnelDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task<ATA.HR.Shared.Dtos.Document.PersonnelDocumentsStatistics> GetDocumentsStatistics(this ODataHttpClient<Auto.AutoPersonnelDocumentDto> personnelDocumentController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/PersonnelDocument/GetDocumentsStatistics(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await personnelDocumentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await personnelDocumentController.DeserializeAsync<ATA.HR.Shared.Dtos.Document.PersonnelDocumentsStatistics>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		
			public static ODataHttpClient<Auto.AutoBedDto> Bed(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoBedDto>(httpClient, "Bed" , "EmploymentContract" );
			}

			
		
			public static ODataHttpClient<Auto.AutoBookingDto> Booking(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoBookingDto>(httpClient, "Booking" , "EmploymentContract" );
			}

			
				public static async Task<ATA.HR.Shared.Dtos.BookingFormDto> GetAllRoomBookingInfo(this ODataHttpClient<Auto.AutoBookingDto> bookingController,ATA.HR.Shared.Dtos.BookingFilterArgs bookingFilterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Booking/GetAllRoomBookingInfo(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							bookingFilterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await bookingController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await bookingController.DeserializeAsync<ATA.HR.Shared.Dtos.BookingFormDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.BookingDto>> GetRoomBookingInfoByRoomIdAndDate(this ODataHttpClient<Auto.AutoBookingDto> bookingController,ATA.HR.Shared.Dtos.BookingRoomFilterArgs bookingRoomFilterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Booking/GetRoomBookingInfoByRoomIdAndDate(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							bookingRoomFilterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await bookingController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await bookingController.DeserializeAsync<List<ATA.HR.Shared.Dtos.BookingDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task AddOrUpdate(this ODataHttpClient<Auto.AutoBookingDto> bookingController,ATA.HR.Shared.Dtos.BookingFormDto bookingForm,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Booking/AddOrUpdate(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							bookingForm
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await bookingController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task VacantRoom(this ODataHttpClient<Auto.AutoBookingDto> bookingController,ATA.HR.Shared.Dtos.VacantInfoDto vacantInfo,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Booking/VacantRoom(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							vacantInfo
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await bookingController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoBuildingDto> Building(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoBuildingDto>(httpClient, "Building" , "EmploymentContract" );
			}

			
				public static async Task<ATA.HR.Shared.Dtos.BuildingDto> GetById(this ODataHttpClient<Auto.AutoBuildingDto> buildingController,int id,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Building/GetById(id={(id == null ? "null" : $"{id}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await buildingController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await buildingController.DeserializeAsync<ATA.HR.Shared.Dtos.BuildingDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.BuildingReadDto>> GetCityBuildings(this ODataHttpClient<Auto.AutoBuildingDto> buildingController,ATA.HR.Shared.Dtos.BuildingFilterArgs buildingFilterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Building/GetCityBuildings(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							buildingFilterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await buildingController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await buildingController.DeserializeAsync<List<ATA.HR.Shared.Dtos.BuildingReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task AddBuilding(this ODataHttpClient<Auto.AutoBuildingDto> buildingController,ATA.HR.Shared.Dtos.BuildingDto building,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Building/AddBuilding(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							building
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await buildingController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoRoomDto> Room(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoRoomDto>(httpClient, "Room" , "EmploymentContract" );
			}

			
				public static async Task<ATA.HR.Shared.Dtos.RoomReadDto> GetById(this ODataHttpClient<Auto.AutoRoomDto> roomController,int id,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Room/GetById(id={(id == null ? "null" : $"{id}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await roomController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await roomController.DeserializeAsync<ATA.HR.Shared.Dtos.RoomReadDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.RoomReadDto>> GetBuildingRooms(this ODataHttpClient<Auto.AutoRoomDto> roomController,ATA.HR.Shared.Dtos.RoomFilterArgs roomFilterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Room/GetBuildingRooms(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							roomFilterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await roomController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await roomController.DeserializeAsync<List<ATA.HR.Shared.Dtos.RoomReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.RoomReadDto>> GetUnitRooms(this ODataHttpClient<Auto.AutoRoomDto> roomController,ATA.HR.Shared.Dtos.RoomFilterArgs roomFilterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Room/GetUnitRooms(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							roomFilterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await roomController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await roomController.DeserializeAsync<List<ATA.HR.Shared.Dtos.RoomReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task AddRoom(this ODataHttpClient<Auto.AutoRoomDto> roomController,ATA.HR.Shared.Dtos.RoomDto room,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Room/AddRoom(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							room
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await roomController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task UpdateRoom(this ODataHttpClient<Auto.AutoRoomDto> roomController,ATA.HR.Shared.Dtos.RoomReadDto room,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Room/UpdateRoom(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							room
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await roomController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoUnitDto> Unit(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoUnitDto>(httpClient, "Unit" , "EmploymentContract" );
			}

			
				public static async Task<ATA.HR.Shared.Dtos.UnitDto> GetById(this ODataHttpClient<Auto.AutoUnitDto> unitController,int id,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Unit/GetById(id={(id == null ? "null" : $"{id}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await unitController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await unitController.DeserializeAsync<ATA.HR.Shared.Dtos.UnitDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.UnitReadDto>> GetUnitBuildings(this ODataHttpClient<Auto.AutoUnitDto> unitController,ATA.HR.Shared.Dtos.UnitFilterArgs unitFilterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Unit/GetUnitBuildings(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							unitFilterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await unitController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await unitController.DeserializeAsync<List<ATA.HR.Shared.Dtos.UnitReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task AddUnit(this ODataHttpClient<Auto.AutoUnitDto> unitController,ATA.HR.Shared.Dtos.UnitDto unit,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Unit/AddUnit(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							unit
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await unitController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task UpdateUnit(this ODataHttpClient<Auto.AutoUnitDto> unitController,ATA.HR.Shared.Dtos.UnitReadDto unit,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Unit/UpdateUnit(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							unit
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await unitController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoPaySlipDto> PaySlip(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoPaySlipDto>(httpClient, "PaySlip" , "EmploymentContract" );
			}

			
				public static async Task<List<ATA.HR.Shared.Dtos.PaySlip.PaySlipReadDto>> GetCurrentUserPaySlipData(this ODataHttpClient<Auto.AutoPaySlipDto> paySlipController,int year,string monthTitle,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/PaySlip/GetCurrentUserPaySlipData(year={(year == null ? "null" : $"{year}")},monthTitle={(monthTitle == null ? "null" : $"'{monthTitle}'")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await paySlipController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await paySlipController.DeserializeAsync<List<ATA.HR.Shared.Dtos.PaySlip.PaySlipReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		
			public static ODataHttpClient<Auto.AutoRoleDto> Role(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoRoleDto>(httpClient, "Role" , "EmploymentContract" );
			}

			
				public static async Task<List<ATA.HR.Shared.Dtos.RoleReadDto>> GetAllRoles(this ODataHttpClient<Auto.AutoRoleDto> roleController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Role/GetAllRoles(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await roleController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await roleController.DeserializeAsync<List<ATA.HR.Shared.Dtos.RoleReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<string>> GetAllClaims(this ODataHttpClient<Auto.AutoRoleDto> roleController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Role/GetAllClaims(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await roleController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await roleController.DeserializeAsync<List<string>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task SetRoleClaims(this ODataHttpClient<Auto.AutoRoleDto> roleController,ATA.HR.Shared.Dtos.SetRoleClaimsArgs args,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Role/SetRoleClaims(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							args
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await roleController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task<List<string>> GetUserClaims(this ODataHttpClient<Auto.AutoRoleDto> roleController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Role/GetUserClaims(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await roleController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await roleController.DeserializeAsync<List<string>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		
			public static ODataHttpClient<Auto.AutoUserDto> User(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoUserDto>(httpClient, "User" , "EmploymentContract" );
			}

			
				public static async Task<List<ATABit.Shared.UserDto>> GetUsersHavingRole(this ODataHttpClient<Auto.AutoUserDto> userController,string? searchTerm,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetUsersHavingRole(searchTerm={(searchTerm == null ? "null" : $"'{searchTerm}'")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATABit.Shared.UserDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATABit.Shared.UserDto> GetUserById(this ODataHttpClient<Auto.AutoUserDto> userController,int userId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetUserById(userId={(userId == null ? "null" : $"{userId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<ATABit.Shared.UserDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATABit.Shared.UserDto?> GetUserByPersonnelCode(this ODataHttpClient<Auto.AutoUserDto> userController,int personnelCode,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetUserByPersonnelCode(personnelCode={(personnelCode == null ? "null" : $"{personnelCode}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<ATABit.Shared.UserDto?>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.UserToSendContractDto>> GetATAPersonnelSuitableToSendContract(this ODataHttpClient<Auto.AutoUserDto> userController,ATA.HR.Shared.Dtos.PersonnelToSendContractFilterArgs toSendContractFilters,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetATAPersonnelSuitableToSendContract(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							toSendContractFilters
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATA.HR.Shared.Dtos.UserToSendContractDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.UserToManageDocumentDto>> GetATAPersonnelSuitableToManageDocument(this ODataHttpClient<Auto.AutoUserDto> userController,ATA.HR.Shared.Dtos.PersonnelToManageDocumentFilterArgs toManageDocumentFilters,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetATAPersonnelSuitableToManageDocument(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							toManageDocumentFilters
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATA.HR.Shared.Dtos.UserToManageDocumentDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATABit.Shared.SelectListItem>> GetATAPersonnelSourceSuitableToRegisterCommitmentLetter(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetATAPersonnelSourceSuitableToRegisterCommitmentLetter(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATABit.Shared.SelectListItem>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.UserWithSignatureDto>> GetPersonnelToGetSignatures(this ODataHttpClient<Auto.AutoUserDto> userController,ATA.HR.Shared.Dtos.SignatureFilterArgs filters,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetPersonnelToGetSignatures(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filters
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATA.HR.Shared.Dtos.UserWithSignatureDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> GetPersonnelSignedCount(this ODataHttpClient<Auto.AutoUserDto> userController,ATA.HR.Shared.Dtos.SignatureFilterArgs filters,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetPersonnelSignedCount(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filters
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATABit.Shared.UserDto>> GetATAPersonnelUnregisteredWorkHours(this ODataHttpClient<Auto.AutoUserDto> userController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetATAPersonnelUnregisteredWorkHours(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATABit.Shared.UserDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATABit.Shared.UserDto>> GetPersonnelDismissedButHaveNotSentSmsYet(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetPersonnelDismissedButHaveNotSentSmsYet(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATABit.Shared.UserDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATABit.Shared.UserDto>> GetPersonnelDismissedAndSmsHasBeenSentToThemObsolete(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetPersonnelDismissedAndSmsHasBeenSentToThemObsolete(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATABit.Shared.UserDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.Contract.DismissSmsReadDto>> GetPersonnelDismissedAndSmsHasBeenSentToThem(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetPersonnelDismissedAndSmsHasBeenSentToThem(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATA.HR.Shared.Dtos.Contract.DismissSmsReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<bool> SendDismissSmsToUser(this ODataHttpClient<Auto.AutoUserDto> userController,int userId,string smsContent,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/SendDismissSmsToUser(userId={(userId == null ? "null" : $"{userId}")},smsContent={(smsContent == null ? "null" : $"'{smsContent}'")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<bool>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<string>> GetAllUnits(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetAllUnits(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<string>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<string>> GetAllWorkLocations(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/GetAllWorkLocations(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<string>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<bool> HasCurrentUserActiveSignature(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/HasCurrentUserActiveSignature(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<bool>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<bool> HasUserActiveSignature(this ODataHttpClient<Auto.AutoUserDto> userController,int userId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/HasUserActiveSignature(userId={(userId == null ? "null" : $"{userId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<bool>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.DirectManagerReadDto>> ContractsGetDirectManagers(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/ContractsGetDirectManagers(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATA.HR.Shared.Dtos.DirectManagerReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.DirectManagerReadDto>> WorkHoursGetDirectManagers(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/WorkHoursGetDirectManagers(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<List<ATA.HR.Shared.Dtos.DirectManagerReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task RegisterUserSignature(this ODataHttpClient<Auto.AutoUserDto> userController,ATA.HR.Shared.Dtos.UserSignatureDto dto,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/RegisterUserSignature(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							dto
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task SendManagerIdentityConfirmSMS(this ODataHttpClient<Auto.AutoUserDto> userController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/SendManagerIdentityConfirmSMS(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task<bool> ConfirmMeAsDirectManager(this ODataHttpClient<Auto.AutoUserDto> userController,string confirmationCode,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/User/ConfirmMeAsDirectManager(confirmationCode={(confirmationCode == null ? "null" : $"'{confirmationCode}'")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await userController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await userController.DeserializeAsync<bool>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		
			public static ODataHttpClient<Auto.AutoActionDto> Action(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoActionDto>(httpClient, "Action" , "EmploymentContract" );
			}

			
				public static async Task<ATABit.Shared.Workflow.PossibleActions> UserPossibleActions(this ODataHttpClient<Auto.AutoActionDto> actionController,int flowType,int refKey,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Action/UserPossibleActions(flowType={(flowType == null ? "null" : $"{flowType}")},refKey={(refKey == null ? "null" : $"{refKey}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await actionController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await actionController.DeserializeAsync<ATABit.Shared.Workflow.PossibleActions>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATA.HR.Shared.Dtos.Workflow.ContractDoActionResponse> DoActionOnContract(this ODataHttpClient<Auto.AutoActionDto> actionController,ATA.HR.Shared.Dtos.Workflow.DoActionOnContractArgs args,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Action/DoActionOnContract(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							args
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await actionController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await actionController.DeserializeAsync<ATA.HR.Shared.Dtos.Workflow.ContractDoActionResponse>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task DoActionOnWorkHour(this ODataHttpClient<Auto.AutoActionDto> actionController,ATABit.Shared.Workflow.DoActionArgs args,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Action/DoActionOnWorkHour(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							args
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await actionController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
		
			public static ODataHttpClient<Auto.AutoDashboardDto> Dashboard(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoDashboardDto>(httpClient, "Dashboard" , "EmploymentContract" );
			}

			
				public static async Task<List<ATA.HR.Shared.Dtos.Workflow.DashboardDto>> GetUserWorks(this ODataHttpClient<Auto.AutoDashboardDto> dashboardController,ATA.HR.Shared.Dtos.Workflow.DashboardFilterArgs dashboardFilter,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Dashboard/GetUserWorks(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							dashboardFilter
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await dashboardController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await dashboardController.DeserializeAsync<List<ATA.HR.Shared.Dtos.Workflow.DashboardDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> GetUserContractWorksCount(this ODataHttpClient<Auto.AutoDashboardDto> dashboardController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Dashboard/GetUserContractWorksCount(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await dashboardController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await dashboardController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> GetUserWorkHoursWorksCount(this ODataHttpClient<Auto.AutoDashboardDto> dashboardController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/Dashboard/GetUserWorkHoursWorksCount(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await dashboardController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await dashboardController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		
			public static ODataHttpClient<Auto.AutoFlowFormDto> FlowForm(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoFlowFormDto>(httpClient, "FlowForm" , "EmploymentContract" );
			}

			
				public static async Task<ATA.HR.Shared.Dtos.Workflow.AllContractFormsReadDto> GetAllContractForms(this ODataHttpClient<Auto.AutoFlowFormDto> flowFormController,int contractId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/FlowForm/GetAllContractForms(contractId={(contractId == null ? "null" : $"{contractId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await flowFormController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await flowFormController.DeserializeAsync<ATA.HR.Shared.Dtos.Workflow.AllContractFormsReadDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATA.HR.Shared.Dtos.Workflow.AllWorkHourFormsReadDto> GetAllWorkHourForms(this ODataHttpClient<Auto.AutoFlowFormDto> flowFormController,int workHoursId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/FlowForm/GetAllWorkHourForms(workHoursId={(workHoursId == null ? "null" : $"{workHoursId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await flowFormController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await flowFormController.DeserializeAsync<ATA.HR.Shared.Dtos.Workflow.AllWorkHourFormsReadDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATABit.Shared.Workflow.WorkFullHistory> GetContractHistory(this ODataHttpClient<Auto.AutoFlowFormDto> flowFormController,int contractId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/FlowForm/GetContractHistory(contractId={(contractId == null ? "null" : $"{contractId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await flowFormController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await flowFormController.DeserializeAsync<ATABit.Shared.Workflow.WorkFullHistory>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATABit.Shared.Workflow.WorkFullHistory> GetWorkHourHistory(this ODataHttpClient<Auto.AutoFlowFormDto> flowFormController,int workHoursId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/FlowForm/GetWorkHourHistory(workHoursId={(workHoursId == null ? "null" : $"{workHoursId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await flowFormController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await flowFormController.DeserializeAsync<ATABit.Shared.Workflow.WorkFullHistory>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		
			public static ODataHttpClient<Auto.AutoWorkHourDto> WorkHour(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoWorkHourDto>(httpClient, "WorkHour" , "EmploymentContract" );
			}

			
				public static async Task<List<ATA.HR.Shared.Dtos.WorkHours.WorkHourReadDto>> GetAllWorkHours(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetAllWorkHours(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<List<ATA.HR.Shared.Dtos.WorkHours.WorkHourReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATABit.Shared.SelectListItem>> GetAllRegistrarsSource(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetAllRegistrarsSource(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<List<ATABit.Shared.SelectListItem>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATA.HR.Shared.Dtos.WorkHours.WorkHourReadDto> GetWorkHourById(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,int workHoursId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetWorkHourById(workHoursId={(workHoursId == null ? "null" : $"{workHoursId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<ATA.HR.Shared.Dtos.WorkHours.WorkHourReadDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.WorkHours.WorkHourHRExcelReadDto>> GetAllWorkHoursHRExcel(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetAllWorkHoursHRExcel(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<List<ATA.HR.Shared.Dtos.WorkHours.WorkHourHRExcelReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> GetAllWorkHoursCount(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetAllWorkHoursCount(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task SetWorkHour(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHourDto workHour,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/SetWorkHour(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							workHour
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task<List<ATABit.Shared.UserDto>> GetAllUnregisteredWorkHoursUsers(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetAllUnregisteredWorkHoursUsers(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<List<ATABit.Shared.UserDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> GetAllUnregisteredWorkHoursUsersCount(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetAllUnregisteredWorkHoursUsersCount(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATA.HR.Shared.Dtos.WorkHours.WorkHourReadDto?> GetUserLastMonthWorkHours(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,int userId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetUserLastMonthWorkHours(userId={(userId == null ? "null" : $"{userId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<ATA.HR.Shared.Dtos.WorkHours.WorkHourReadDto?>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<int>> GetFlightCrewLastMonthPersonnelCodesOrdered(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetFlightCrewLastMonthPersonnelCodesOrdered(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<List<int>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<int>> GetNotFlightCrewLastMonthPersonnelCodesOrdered(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetNotFlightCrewLastMonthPersonnelCodesOrdered(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<List<int>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> GetUserWorkHourIdForGivenMonth(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,int userId,int year,int month,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetUserWorkHourIdForGivenMonth(userId={(userId == null ? "null" : $"{userId}")},year={(year == null ? "null" : $"{year}")},month={(month == null ? "null" : $"{month}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task DeleteWorkHour(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.CommitmentLetter.DeleteWorkHourArgs deleteArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/DeleteWorkHour(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							deleteArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task<ATA.HR.Shared.Dtos.WorkHours.WorkHoursCountResult> GetWorkHoursCountStatistics(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs filterArgs,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetWorkHoursCountStatistics(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							filterArgs
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<ATA.HR.Shared.Dtos.WorkHours.WorkHoursCountResult>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<bool> StartWorkHourFlow(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,int workHourId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/StartWorkHourFlow(workHourId={(workHourId == null ? "null" : $"{workHourId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<bool>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<ATA.HR.Shared.Dtos.WorkHours.WorkHourDto> GetWorkHourByIdForForm(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,int workHourId,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetWorkHourByIdForForm(workHourId={(workHourId == null ? "null" : $"{workHourId}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<ATA.HR.Shared.Dtos.WorkHours.WorkHourDto>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.WorkHourToBeSentDto>> GetATAPersonnelSuitableToSendWorkHour(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs workHoursFilters,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetATAPersonnelSuitableToSendWorkHour(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							workHoursFilters
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<List<ATA.HR.Shared.Dtos.WorkHourToBeSentDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<int> GetATAPersonnelSuitableToSendWorkHourCount(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.WorkHoursFilterArgs workHoursFilters,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetATAPersonnelSuitableToSendWorkHourCount(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							workHoursFilters
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<int>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<List<ATA.HR.Shared.Dtos.WorkHours.WorkHourReadDto>> GetReportDataForCeoConfirm(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.Reports.WorkHourForCeoConfirmFilterArgs args,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetReportDataForCeoConfirm(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							args
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<List<ATA.HR.Shared.Dtos.WorkHours.WorkHourReadDto>>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<string?> GetDirectManagerCommentOnWorkHours(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ATA.HR.Shared.Dtos.WorkHours.Reports.WorkHourForCeoConfirmFilterArgs args,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/GetDirectManagerCommentOnWorkHours(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							args
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<string?>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
				public static async Task<bool> SendSmsToAllWorkHoursDirectManagers(this ODataHttpClient<Auto.AutoWorkHourDto> workHourController,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHour/SendSmsToAllWorkHoursDirectManagers(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourController.DeserializeAsync<bool>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		
			public static ODataHttpClient<Auto.AutoWorkHourFlowDirectionManagerCommentDto> WorkHourFlowDirectionManagerComment(this HttpClient httpClient)
			{
				return new ODataHttpClient<Auto.AutoWorkHourFlowDirectionManagerCommentDto>(httpClient, "WorkHourFlowDirectionManagerComment" , "EmploymentContract" );
			}

			
				public static async Task SetManagerComment(this ODataHttpClient<Auto.AutoWorkHourFlowDirectionManagerCommentDto> workHourFlowDirectionManagerCommentController,ATA.HR.Shared.Dtos.WorkHours.WorkHourFlowDirectManagerCommentDto dto,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHourFlowDirectionManagerComment/SetManagerComment(){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
											request.Content = new StringContent(DefaultJsonContentFormatter.Current.Serialize(new 
						{ 
							dto
						}), Encoding.UTF8, DefaultJsonContentFormatter.Current.ContentType);
										using HttpResponseMessage response = (await workHourFlowDirectionManagerCommentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
									}

			
				public static async Task<string> GetManagerComment(this ODataHttpClient<Auto.AutoWorkHourFlowDirectionManagerCommentDto> workHourFlowDirectionManagerCommentController,int year,int month,ODataContext? oDataContext = default,CancellationToken cancellationToken = default)
				{
					string qs = oDataContext?.Query is not null ? $"?{oDataContext.Query}" : string.Empty;
					string requestUri = $"odata/EmploymentContract/WorkHourFlowDirectionManagerComment/GetManagerComment(year={(year == null ? "null" : $"{year}")},month={(month == null ? "null" : $"{month}")}){qs}";
					using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUri);
										using HttpResponseMessage response = (await workHourFlowDirectionManagerCommentController.HttpClient.SendAsync(request, cancellationToken)).EnsureSuccessStatusCode();
											using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
						var oDataResponse = await workHourFlowDirectionManagerCommentController.DeserializeAsync<string>(responseStream, oDataContext, cancellationToken);
						return oDataResponse;
									}

			
		    }
}
