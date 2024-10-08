﻿using CLI.Serialization;
using CLI.Model;
using CLI.Serialization;

namespace CLI.Storage;

/*
 * Klasa koja je zadužena za čuvanje podataka u fajl sistemu.
 * Ova klasa vodi računa da odgovarajući fajl postoji,
 * piše i čita podatke iz njega. Storage korsiti Serializer
 * klasu za konverziju objekta u string i obrnuto.
 */
public class Storage<T> where T : ISerializable, new()
{
    private readonly string _fileName = @"../../../../CLI/Data/{0}";
    private readonly Serializer<T> _serializer = new();

    public Storage(string fileName)
    {
        _fileName = string.Format(_fileName, fileName);
        
    }
    public List<T> Load()
    {
        // ukoliko fajl ne postoji napravi novi fajl i
        // zatvori stream za pisanje u fajl
        if (!File.Exists(_fileName))
        {
            FileStream fs = File.Create(_fileName);
            fs.Close();
        }

        
            IEnumerable<string> lines = File.ReadLines(_fileName);
            List<T> objects = _serializer.FromCSV(lines);

            return objects;
        
    }

    public void Save(List<T> objects)
    {
        string serializedObjects = _serializer.ToCSV(objects);
        using (StreamWriter streamWriter = new StreamWriter(_fileName))
        {
            streamWriter.Write(serializedObjects);
        }
    }
}