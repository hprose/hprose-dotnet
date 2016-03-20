namespace ExampleAssambly
{
    /// <summary>
    /// 示例
    /// </summary>
    public interface IExample : IExampleInterface
    {
        /// <summary>
        /// 示例
        /// </summary>
        /// <param name="param1">整型参数1</param>
        /// <param name="param2">浮点型参数2</param>
        /// <param name="param3">字符串参数3</param>
        /// <returns></returns>
        int Example(int param1, double param2, string param3);

        /// <summary>
        /// 示例方法
        /// </summary>
        /// <param name="param1">整型参数1</param>
        /// <param name="param2">浮点型参数2</param>
        /// <param name="param3">字符串参数3</param>
        /// <returns></returns>
        new bool ExampleMethod(int param1, double param2, string param3);
    }
}