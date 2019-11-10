namespace Application.CQS.Auth.Input
{
    public class SignInInput
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public SignInInput(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}