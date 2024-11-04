using MemoriesBackend.Domain.Interfaces.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace MemoriesBackend.Domain.Entities
{
    public class FolderDetails : IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(255)]
        [CustomValidation(typeof(Folder), nameof(ValidateFolderName))]
        public string Name { get; set; }
        public bool IsStared { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastOpenedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        //Navigation properties
        [ForeignKey(nameof(Id))]
        public Folder Folder { get; set; }

        //Validation
        private static readonly Regex InvalidCharsRegex = new(@"[<>:""/\\|?*]", RegexOptions.Compiled);
        public static ValidationResult ValidateFolderName(string name, ValidationContext context)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new ValidationResult("Folder name cannot be empty.");

            if (InvalidCharsRegex.IsMatch(name))
                return new ValidationResult("Folder name contains invalid characters.");

            if (name.EndsWith(" ") || name.EndsWith("."))
                return new ValidationResult("Folder name cannot end with a space or period.");

            return ValidationResult.Success;
        }
    }
}
