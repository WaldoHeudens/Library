using NETCore.MailKit.Infrastructure.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models
{
    public class Parameter
    {
        [Key]
        [Display(Name = "Parameter")]
        public string Name { get; set; }

        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [ForeignKey("LibraryUser")]
        [Display(Name = "Changed By")]
        public string UserId { get; set; }

        public LibraryUser? User { get; set; }

        [Display(Name = "Last Changed")]
        [DataType(DataType.Date)]
        public DateTime LastChanged { get; set; }

        [Display(Name = "Obsolete")]
        [DataType(DataType.Date)]
        public DateTime Obsolete { get; set; }



        static public Dictionary<string, Parameter> Parameters { get; set; }

        static public void ConfigureMail(MailKitOptions mailOptions)
        {

            mailOptions.Server = Parameters["Mail.Server"].Value;
            mailOptions.Server = Parameters["Mail.Server"].Value;
            mailOptions.Port = Convert.ToInt32(Parameters["Mail.Port"].Value);
            mailOptions.Account = Parameters["Mail.Account"].Value;
            mailOptions.Password = Parameters["Mail.Password"].Value;
            mailOptions.SenderEmail = Parameters["Mail.SenderEmail"].Value;
            mailOptions.SenderName = Parameters["Mail.SenderName"].Value;
            mailOptions.Security = Convert.ToBoolean(Parameters["Mail.Security"].Value);
        }

    }
}
