using CLI.Model;
using CLI.Serialization;

namespace CLI.Model;

public class StudentPredmet : ISerializable
{
    public int StudentId { get; set; }

    public int PredmetId { get; set; }

    public StudentPredmet()
    {
    }

    public StudentPredmet(int studentId, int predmetId)
    {
        StudentId = studentId;
        PredmetId = predmetId;
    }

    public void FromCSV(string[] values)
    {
        StudentId = int.Parse(values[0]);
        PredmetId = int.Parse(values[1]);
    }

    public string[] ToCSV()
    {
        string[] csvValues =
        {
            StudentId.ToString(),
            PredmetId.ToString()
        };
        return csvValues;
    }

    public override string ToString()
    {
        return $"{StudentId} {PredmetId}";
    }
}

