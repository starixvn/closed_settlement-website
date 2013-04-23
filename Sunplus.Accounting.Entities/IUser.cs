namespace Sunplus.Accounting.Entities
{

    /// <summary>
    /// Summary description for IUser
    /// </summary>
    public interface IUser
    {
        int Id
        { get; set; }

        string Username
        { get; set; }

        string Password
        { get; set; }

        string Fullname
        { get; set; }
    }

}