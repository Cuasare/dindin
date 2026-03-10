namespace Dindin.API.Models;

public class Group
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string CodigoConvite { get; set; }
    public ICollection<UserGroup> UserGrupo { get; set; }
}