namespace ChatGpt.Shared.Module;

public class DALLEMoDto
{
    public long created { get; set; }

    public List<DALLEDataDto> data { get; set; }
}

public class DALLEDataDto
{
    public string Url { get; set; }
}