using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Services.Interfaces
{
    public interface IDialogService
    {
        Task<bool> ConfirmAsync(string title, string message);
        Task DisplayAsync(string title, string message);
    }
}
