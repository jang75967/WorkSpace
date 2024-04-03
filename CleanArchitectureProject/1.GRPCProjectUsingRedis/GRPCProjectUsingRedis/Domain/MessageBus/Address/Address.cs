namespace Domain.MessageBus.Address
{
    public record Address : IAddress
    {
        public string IP { get; set; }
        public string Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public Address(string ip, string port, string username, string password)
        {
            IP = ip;
            Port = port;
            UserName = username;
            Password = password;
        }

        public string Get()
        {
            return $"{IP}:{Port}";
        }
    }
}
