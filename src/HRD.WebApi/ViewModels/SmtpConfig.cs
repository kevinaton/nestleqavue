namespace HRD.WebApi.ViewModels
{
    public class SmtpConfig
    {
        public string From { get; set; }
        public string CredentialUserName { get; set; }
        public string CredentialPassword { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
