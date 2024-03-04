namespace RedisLibrary
{
    public record Address : IAddress
    {
        public string IP { get; set; }
        public string Port { get; set; }

        public Address(string ip, string port)
        {
            IP = ip;
            Port = port;
        }

        public string Get()
        {
            return $"{IP}:{Port}";
        }
    }
}