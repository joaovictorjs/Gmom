using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gmom.Domain.Interface;

namespace Gmom.Domain.Entities;

[Table("usuarios")]
public class UserEntity:IEntity
{
    [Column("id"), Key]
    public required int Id { get; set; }
    [Column("nome")]
    public required string Name { get; set; }
    [Column("senha")]
    public required string Password { get; set; }
    [Column("administrador")]
    public required bool IsAdmin { get; set; }
}