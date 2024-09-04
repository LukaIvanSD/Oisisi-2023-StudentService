namespace CLI.Model;

public class Datum
{
    public int dan { get; set; }
    public int mesec { get; set; }
    public int godina { get; set; }

    public Datum() { }

    public Datum(string values)
    {
        string[] datumVrednosti = values.Split('.');
        this.dan = int.Parse(datumVrednosti[0]);
        this.mesec = int.Parse(datumVrednosti[1]);
        this.godina = int.Parse(datumVrednosti[2]);
    }
    public void setDatum(string values) {
        string[] datumVrednosti = values.Split('.');
        this.dan = int.Parse(datumVrednosti[0]);
        this.mesec = int.Parse(datumVrednosti[1]);
        this.godina = int.Parse(datumVrednosti[2]);
    }

    public Datum(int dan, int mesec, int godina)
    {
        this.dan = dan;
        this.mesec = mesec;
        this.godina = godina;
    }

    public override string ToString()
    {
        return $"{this.dan}.{this.mesec}.{this.godina}.";
    }
}


