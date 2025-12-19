using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Common.Infrastructure
{
    public interface IViewModelFactory
    {
        T Create<T>() where T : class;
        T Create<T>(params object[] parameters) where T : class;
    }
}
