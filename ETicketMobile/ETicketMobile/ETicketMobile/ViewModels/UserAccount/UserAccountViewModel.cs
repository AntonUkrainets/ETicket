﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using ETicketMobile.Business.Model.UserAccount;
using ETicketMobile.Resources;
using ETicketMobile.Views.Tickets;
using ETicketMobile.Views.UserActions;
using Prism.Navigation;
using Xamarin.Forms;

namespace ETicketMobile.ViewModels.UserAccount
{
    public class UserAccountViewModel : ViewModelBase
    {
        private string email;

        private readonly INavigationService navigationService;
        private INavigationParameters navigationParameters;

        private ICommand navigateToAction;

        private IEnumerable<UserAction> actions;

        public IEnumerable<UserAction> UserActions
        {
            get => actions;
            set => SetProperty(ref actions, value);
        }

        public ICommand NavigateToAction => navigateToAction 
            ??= new Command<UserAction>(OnNavigateToAction);

        public UserAccountViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this.navigationService = navigationService
                ?? throw new ArgumentNullException(nameof(navigationService));
        }

        public override void OnAppearing()
        {
            Init();
        }

        private void Init()
        {
            UserActions = new List<UserAction>
            {
                new UserAction { Name = AppResource.BuyTicket, View = nameof(TicketsView) },
                new UserAction { Name = AppResource.TransactionHistory, View = nameof(UserTransactionsView) }
                //new UserAction { Name = AppResource.MyTickets, View = nameof(MyTicketsView) }
            };
        }

        public override void OnNavigatedTo(INavigationParameters navigationParameters)
        {
            if (string.IsNullOrEmpty(email))
            {
                email = navigationParameters.GetValue<string>("email");
            }

            this.navigationParameters = navigationParameters;
        }

        private async void OnNavigateToAction(UserAction action)
        {
            await navigationService.NavigateAsync(action.View, navigationParameters);
        }
    }
}