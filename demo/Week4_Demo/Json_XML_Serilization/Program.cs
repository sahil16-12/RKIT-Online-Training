using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace JsonXmlDemo;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public DateTime CreatedOn { get; set; }
}

class Demo
{
    static void Main()
    {
        var prod = new Product
        {
            Id = 101,
            Name = "Gaming Mouse",
            Price = 59.99m,
            CreatedOn = DateTime.Now
        };

        string jsonFile = "product.json";
        string xmlFile = "product.xml";

        SerializeToJson(prod, jsonFile);
        Product? fromJson = DeserializeFromJson(jsonFile);

        SerializeToXml(prod, xmlFile);
        Product? fromXml = DeserializeFromXml(xmlFile);

        Console.WriteLine("\nFrom JSON: " + (fromJson != null ? $"{fromJson.Name}, {fromJson.Price}" : "null"));
        Console.WriteLine("From XML: " + (fromXml != null ? $"{fromXml.Name}, {fromXml.Price}" : "null"));
    }

    static void SerializeToJson(Product p, string path)
    {
        var opts = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(p, opts);
        File.WriteAllText(path, json);
        Console.WriteLine($"Wrote JSON to {path}");
    }

    static Product? DeserializeFromJson(string path)
    {
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Product>(json);
    }

    static void SerializeToXml(Product p, string path)
    {
        var xmlSer = new XmlSerializer(typeof(Product));
        using var writer = File.CreateText(path);
        xmlSer.Serialize(writer, p);
        Console.WriteLine($"Wrote XML to {path}");
    }

    static Product? DeserializeFromXml(string path)
    {
        if (!File.Exists(path)) return null;

        var xmlSer = new XmlSerializer(typeof(Product));
        using var reader = File.OpenText(path);
        return (Product?)xmlSer.Deserialize(reader);
    }
}
