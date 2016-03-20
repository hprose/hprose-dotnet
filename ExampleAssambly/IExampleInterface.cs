using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleAssambly
{
    /// <summary>
    /// 示例接口
    /// </summary>
    public interface IExampleInterface : IPlay
    {
        /// <summary>
        /// 示例方法
        /// </summary>
        /// <param name="param1">整型参数1</param>
        /// <param name="param2">浮点型参数2</param>
        /// <param name="param3">字符串参数3</param>
        /// <returns></returns>
        string ExampleMethod(int param1, double param2, string param3);
    }
}
