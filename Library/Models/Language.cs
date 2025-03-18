using Library.Data;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
    public class Language
    {
        public static List<Language> Languages { get; set; }
        public static Dictionary<string, Language> LanguagesById { get; set; }


        [Key]
        [Display (Name="Code")]
        [StringLength (2)]
        public string Id { get; set; }

        [Display (Name = "Language")]
        public string Name { get; set; }

        [Display (Name = "System")]
        public bool IsSystemLanguage { get; set; }

        [Display(Name = "Available From")]
        [DataType(DataType.DateTime)]
        public DateTime IsAvailable { get; set; } = DateTime.Now;


        public static void GetLanguages(ApplicationDbContext context)
        {
            List<string> eu_cultures = new List<string>{ "BE", "FR" };
            Languages = context.Languages.Where(lan => lan.IsAvailable < DateTime.Now).OrderBy(lan => lan.Name).ToList();
            LanguagesById = new Dictionary<string, Language>();

            RequestLocalizationOptions options = new RequestLocalizationOptions();
            List<string> optionList = new List<string>();

            foreach (Language language in Languages)
            {
                LanguagesById[language.Id] = language;
                optionList.Add(language.Id);
            }
            options.AddSupportedCultures(optionList.ToArray());
            options.AddSupportedUICultures(optionList.ToArray());
            options.SetDefaultCulture("en");

            // Reinitialize the RequestLocalization
            Globals.App.UseRequestLocalization(options);
        }
    }
}
