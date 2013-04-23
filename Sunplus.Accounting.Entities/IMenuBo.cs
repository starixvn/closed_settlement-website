using System.Collections.Generic;


namespace Sunplus.Accounting.Entities
{
    /// <summary>
    /// Summary description for IMenuItem
    /// </summary>
    public interface IMenuBO
    {
        int Id { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        bool IsOpenNewWindow { get; set; }
        List<MenuBO> SubMenu { get; set; }

        int ParentId { get; set; }
        string FunctionName { get; set; }
        string Target { get; set; }
        bool IsHide { get; set; }
        int Order { get; set; }

        int Level { get; set; }

        string System { get; set; }
    }
}