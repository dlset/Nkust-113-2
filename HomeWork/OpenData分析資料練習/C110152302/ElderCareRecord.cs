using CsvHelper.Configuration.Attributes;

public class ElderCareRecord
{
    [Name("��F��")]
    public string? Area { get; set; }

    [Name("�H��")]
    public int People { get; set; }

    [Name("�q�ܰݦw")]
    public string? PhoneGreetingsRaw { get; set; }

    [Name("���h�X��")]
    public string? VisitsRaw { get; set; }

    [Name("�N���U")]
    public int MedicalHelp { get; set; }

    [Name("�ͬ���U")]
    public int LifeHelp { get; set; }

    [Name("�����w�˺��ϴ��s�u�H�ơ]�H�^")]
    public int Emergency { get; set; }

    [Name("���ӪA��")]
    public int LongTermCare { get; set; }

    [Name("�A�ȦX�p")]
    public string? TotalServiceRaw { get; set; }

    public int PhoneGreetings => int.Parse(PhoneGreetingsRaw.Replace(",", ""));
    public int Visits => int.Parse(VisitsRaw.Replace(",", ""));
    public int TotalService => int.Parse(TotalServiceRaw.Replace(",", ""));
}
