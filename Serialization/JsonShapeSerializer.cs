using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Serialization;

// JSON serializer using polymorphic serialization via virtual methods
public class JsonShapeSerializer : IShapeSerializer
{
    public string FileExtension => ".json";
    
    // Central registry - shapes self-register via RegisterType method
    private static readonly Dictionary<string, Func<Shape>> TypeRegistry = new();
    
    // Called by each shape's static constructor to self-register
    public static void RegisterType(string typeName, Func<Shape> factory)
    {
        TypeRegistry[typeName] = factory;
    }

    public void Serialize(string filePath, IEnumerable<Shape> shapes)
    {
        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
        
        writer.WriteStartObject();
        writer.WriteStartArray("Shapes");
        
        foreach (var shape in shapes)
        {
            writer.WriteStartObject();
            shape.WriteJson(writer); 
            writer.WriteEndObject();
        }
        
        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Flush();
        
        File.WriteAllBytes(filePath, stream.ToArray());
    }

    public List<Shape> Deserialize(string filePath)
    {
        var json = File.ReadAllText(filePath);
        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;
        var shapesArray = root.GetProperty("Shapes");
        
        var shapes = new List<Shape>();
        
        foreach (var element in shapesArray.EnumerateArray())
        {
            var typeName = element.GetProperty("Type").GetString();
            
            if (TypeRegistry.TryGetValue(typeName, out var factory))
            {
                var shape = factory();
                shape.ReadJson(element); 
                shapes.Add(shape);
            }
        }
        
        return shapes;
    }
}