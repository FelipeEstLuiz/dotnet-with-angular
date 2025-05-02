namespace Application.Domain.VO;

public class PhotoVo
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }
}
