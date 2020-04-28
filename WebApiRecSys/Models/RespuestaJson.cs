namespace WebApiRecSys
{
    public class RespuestaJson
    {   public RespuestaJson(bool success, string message, object response)
        {
            this.success = success;
            this.message = message;
            this.response = response;
        }

        public bool success { get; set; }
        public string message { get; set; }
        public object response { get; set; }
    }
}


        