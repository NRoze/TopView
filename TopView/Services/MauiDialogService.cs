using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopView.Services.Interfaces;

namespace TopView.Services
{
    public class MauiDialogService : IDialogService
    {
        public async Task<bool> ConfirmAsync(string title, string message)
        {
            return await Application.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
        }

        public async Task DisplayAsync(string title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, "Ok");
        }
    }
}
