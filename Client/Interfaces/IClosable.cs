using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Interfaces
{
    /// <summary>
    /// Интерфейс, который позволяет закрыть view из vm, не ломая принципы MVVM
    /// </summary>
    public interface IClosable
    {
        void Close();
    }
}
