using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace MemoriesBackend.Domain.Entities
{
    public class FileDetails : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(255)]
        [CustomValidation(typeof(FileDetails), nameof(ValidateFileName))]
        public string Name { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        [MaxLength(255)]
        public string Description { get; set; } = "";
        public bool IsStared { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastOpenedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        //Navigation properties
        [ForeignKey(nameof(Id))]
        public File File { get; set; }

        //Validation
        private static readonly Regex InvalidCharsRegex = new(@"[<>:""/\\|?*]", RegexOptions.Compiled);
        public static ValidationResult ValidateFileName(string name, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new ValidationResult("File name cannot be empty.");

            if (InvalidCharsRegex.IsMatch(name))
                return new ValidationResult("File name contains invalid characters.");

            if (name.EndsWith(" ") || name.EndsWith("."))
                return new ValidationResult("File name cannot end with a space or period.");

            return ValidationResult.Success;
        }
    }
}
