// GraphicsLabAvalonia/Serialization/ShapeJsonConverter.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using GraphicsLabAvalonia.Models;

namespace GraphicsLabAvalonia.Serialization;

// JSON converter that handles Shape polymorphism
// Each shape type provides its own serialization logic
public class ShapeJsonConverter : JsonConverter<Shape>
{
    // Map of shape type names to their types (built once via reflection)
    private static readonly Dictionary<string, Type> ShapeTypeMap = BuildTypeMap();
    
    private static Dictionary<string, Type> BuildTypeMap()
    {
        var map = new Dictionary<string, Type>();
        
        // Find all concrete Shape types
        var shapeTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(Shape).IsAssignableFrom(t) 
                        && !t.IsAbstract 
                        && t != typeof(Shape));
        
        foreach (var type in shapeTypes)
        {
            map[type.Name] = type;
        }
        
        return map;
    }
    
    public override Shape Read(ref Utf8JsonReader reader, Type typeToConvert, 
                               JsonSerializerOptions options)
    {
        // Store the raw JSON for deserialization
        using var jsonDoc = JsonDocument.ParseValue(ref reader);
        var root = jsonDoc.RootElement;
        
        // Read the type discriminator
        var typeName = root.GetProperty("Type").GetString();
        
        if (ShapeTypeMap.TryGetValue(typeName, out var shapeType))
        {
            // Deserialize to the correct concrete type
            return (Shape)JsonSerializer.Deserialize(root.GetRawText(), shapeType, options);
        }
        
        throw new JsonException($"Unknown shape type: {typeName}");
    }
    
    public override void Write(Utf8JsonWriter writer, Shape value, 
                               JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        // Write type discriminator
        writer.WriteString("Type", value.GetType().Name);
        
        // Write all properties of the concrete type
        var properties = value.GetType().GetProperties()
            .Where(p => p.CanRead && p.DeclaringType != typeof(Shape) 
                        || p.Name == nameof(Shape.Id) 
                        || p.Name == nameof(Shape.X) 
                        || p.Name == nameof(Shape.Y)
                        || p.Name == nameof(Shape.FillColor)
                        || p.Name == nameof(Shape.StrokeColor)
                        || p.Name == nameof(Shape.StrokeThickness));
        
        foreach (var prop in properties)
        {
            var propValue = prop.GetValue(value);
            writer.WritePropertyName(prop.Name);
            JsonSerializer.Serialize(writer, propValue, prop.PropertyType, options);
        }
        
        writer.WriteEndObject();
    }
}