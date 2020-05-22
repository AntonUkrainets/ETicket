﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Android.Util;
using ETicketMobile.Resources;
using ETicketMobile.Views.ForgotPassword;
using ETicketMobile.Views.Login;
using ETicketMobile.WebAccess.DTO;
using ETicketMobile.WebAccess.Network;
using ETicketMobile.WebAccess.Network.WebService;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.ForgotPassword
{
    public class ForgotPasswordViewModel : ViewModelBase
    {
        #region Fields

        private readonly INavigationService navigationService;

        private ICommand navigateToConfirmForgotPasswordView;
        private ICommand cancelCommand;

        private readonly HttpClientService httpClient;

        private string emailWarning;

        private const int EmailMaxLength = 50;

        #endregion

        #region Properties

        public ICommand NavigateToConfirmForgotPasswordView => navigateToConfirmForgotPasswordView 
            ??= new Command<string>(OnNavigateToConfirmForgotPasswordView);

        public ICommand CancelCommand => cancelCommand 
            ??= new Command(OnCancelCommand);

        public string EmailWarning
        {
            get => emailWarning;
            set => SetProperty(ref emailWarning, value);
        }

        #endregion

        public ForgotPasswordViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));

            httpClient = new HttpClientService(ServerConfig.Address);
        }

        private async void OnNavigateToConfirmForgotPasswordView(string email)
        {
            if (! await IsValidAsync(email))
                return;

            await RequestActivationCodeAsync(email);

            var navigationParameters = new NavigationParameters
            {
                { "email", email }
            };

            await navigationService.NavigateAsync(nameof(ConfirmForgotPasswordView), navigationParameters);
        }

        private async void OnCancelCommand()
        {
            await navigationService.NavigateAsync(nameof(LoginView));
        }

        #region Validation

        private async Task<bool> IsValidAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                //TODO incorrect
                EmailWarning = AppResource.EmailCorrect;

                return false;
            }

            if (!IsEmailValid(email))
            {
                EmailWarning = AppResource.EmailInvalid;

                return false;
            }

            // TODO EmailHasCorrectLength
            if (!IsEmailConstainsCorrectLong(email))
            {
                EmailWarning = AppResource.EmailCorrectLong;

                return false;
            }

            var userExists = await RequestUserExistsAsync(email);

            if (!userExists)
            {
                EmailWarning = AppResource.EmailWrong;

                return false;
            }

            return true;
        }

        private async Task<bool> RequestUserExistsAsync(string email)
        {
            var signUpRequestDto = new ForgotPasswordRequestDto { Email = email };

            var isUserExists = await httpClient.PostAsync<ForgotPasswordRequestDto, ForgotPasswordResponseDto>(
                    AuthorizeEndpoint.CheckEmail, 
                    signUpRequestDto);

            return isUserExists.Succeeded;
        }

        private bool IsEmailValid(string email)
        {
            return Patterns.EmailAddress.Matcher(email).Matches();
        }

        private bool IsEmailConstainsCorrectLong(string email)
        {
            return email.Length <= EmailMaxLength;
        }

        #endregion

        private async Task RequestActivationCodeAsync(string email)
        {
            await httpClient.PostAsync<string, string>(AuthorizeEndpoint.RequestActivationCode, email);
        }
    }
}