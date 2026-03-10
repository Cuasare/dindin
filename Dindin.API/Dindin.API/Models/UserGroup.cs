namespace Dindin.API.Models;

public class UserGroup
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int GrupoId { get; set; }
    public Group Grupo { get; set; }
}