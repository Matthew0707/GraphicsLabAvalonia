using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using GraphicsLabAvalonia.Factories;
using GraphicsLabAvalonia.Plugins;
using GraphicsLabAvalonia.Rendering;
using GraphicsLabAvalonia.Serialization;

namespace GraphicsLabAvalonia;

/// <summary>
/// Dynamically loads shape plugins from .dll files in the Plugins folder.
/// Base application code NEVER changes when adding new plugins.
/// </summary>
public class PluginLoader
{
    private readonly string _pluginsPath;
    private readonly List<IDataProcessor> _loadedProcessors = new();
    public IReadOnlyList<IDataProcessor> Processors => _loadedProcessors;
    public PluginLoader()
    {
        // Plugins folder is next to the executable
        _pluginsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        
        // Create folder if it doesn't exist
        if (!Directory.Exists(_pluginsPath))
            Directory.CreateDirectory(_pluginsPath);
    }
    /// <summary>
    /// Load data processor plugins from Plugins folder
    /// </summary>
    public void LoadProcessors()
    {
        if (!Directory.Exists(_pluginsPath)) return;

        foreach (var dll in Directory.GetFiles(_pluginsPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);
                var processorTypes = assembly.GetTypes()
                    .Where(t => typeof(IDataProcessor).IsAssignableFrom(t)
                                && !t.IsAbstract && !t.IsInterface);

                foreach (var type in processorTypes)
                {
                    var processor = (IDataProcessor)Activator.CreateInstance(type);
                    _loadedProcessors.Add(processor);
                    System.Diagnostics.Debug.WriteLine(
                        $"LOADED PROCESSOR: {processor.ProcessorName}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"Failed to load processor: {ex.Message}");
            }
        }
    }
    /// <summary>
    /// Loads all plugin .dll files from the Plugins folder.
    /// Each plugin registers its factory, renderer, and serializer.
    /// </summary>
    public void LoadAll(ShapeFactoryManager factoryMgr, 
                        RenderManager renderMgr,
                        JsonShapeSerializer serializer)
    {
        if (!Directory.Exists(_pluginsPath))
            return;

        var dllFiles = Directory.GetFiles(_pluginsPath, "*.dll");

        foreach (var dllPath in dllFiles)
        {
            try
            {
                LoadSinglePlugin(dllPath, factoryMgr, renderMgr, serializer);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"ERROR loading plugin {Path.GetFileName(dllPath)}: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Loads one plugin assembly, finds IShapePlugin implementation,
    /// and registers all its components.
    /// </summary>
    private void LoadSinglePlugin(string dllPath, 
                                  ShapeFactoryManager factoryMgr,
                                  RenderManager renderMgr, 
                                  JsonShapeSerializer serializer)
    {
        // Load the .dll assembly
        var assembly = Assembly.LoadFrom(dllPath);

        // Find the class that implements IShapePlugin
        var pluginType = assembly.GetTypes()
            .FirstOrDefault(t => typeof(IShapePlugin).IsAssignableFrom(t) 
                              && !t.IsAbstract && !t.IsInterface);

        if (pluginType == null)
        {
            System.Diagnostics.Debug.WriteLine(
                $"WARNING: No IShapePlugin found in {Path.GetFileName(dllPath)}");
            return;
        }

        // Create plugin instance
        var plugin = (IShapePlugin)Activator.CreateInstance(pluginType);

        // Register factory → shape appears in UI list
        factoryMgr.RegisterFactory(plugin.GetFactory());

        // Register renderer → shape can be drawn
        renderMgr.RegisterRenderer(plugin.GetRenderer());

        // Register in serializer → shape can be saved/loaded
        JsonShapeSerializer.RegisterType(
            plugin.GetShapeTypeName(), 
            plugin.GetShapeFactory());

        System.Diagnostics.Debug.WriteLine(
            $"LOADED: {plugin.PluginName} v{plugin.Version}");
    }
}