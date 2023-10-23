using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TFA.Storage;

public class User
{
    [Key]
    public Guid UserId { get; set; }

    public string Name { get; set; }

    [MaxLength(30)]
    public string Email { get; set; }   // Login


    [InverseProperty(nameof(Topic.Author))]
    public ICollection<Topic> Topics { get; set; }


    [InverseProperty(nameof(Comment.Author))]
    public ICollection<Comment> Comments{ get; set; }
    
}