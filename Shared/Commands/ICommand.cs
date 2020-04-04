using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Commands
{
    public interface ICommand
    {
        void Validate();
    }
}
