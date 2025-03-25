using System.ComponentModel.DataAnnotations;

namespace Library.ApiModels
{
    public class Login
    {
        public string UserName { get; set; }

        [DataType (DataType.Password)]
        public string Password { get; set; }
    }
}
