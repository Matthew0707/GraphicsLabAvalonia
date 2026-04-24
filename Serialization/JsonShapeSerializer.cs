// GraphicsLabAvalonia/Serialization/JsonShapeSerializer.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Serialization;

// JSON serializer for shapes using System.Text.Json
public class JsonShapeSerializer : IShapeSerializer
{
    public string FileExtension => ".json";

    public void Serialize(string filePath, IEnumerable<Shape> shapes)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            // Include type information for polymorphic deserialization
            Converters = { new ShapeJsonConverter() }
        };
        
        var wrapper = new ShapeCollectionWrapper { Shapes = new List<Shape>(shapes) };
        var json = JsonSerializer.Serialize(wrapper, options);
        File.WriteAllText(filePath, json);
    }

    public List<Shape> Deserialize(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions
        {
            Converters = { new ShapeJsonConverter() }
        };
        
        var wrapper = JsonSerializer.Deserialize<ShapeCollectionWrapper>(json, options);
        return wrapper?.Shapes ?? new List<Shape>();
    }
}

// Wrapper class for serializing collection of shapes
public class ShapeCollectionWrapper
{
    public List<Shape> Shapes { get; set; } = new List<Shape>();
}