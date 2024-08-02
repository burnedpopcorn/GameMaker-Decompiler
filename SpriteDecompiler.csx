// Made with the help of QuantumV and The United Modders Of Pizza Tower Team
using System.Text;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UndertaleModLib.Util;
using System.Collections.Generic;

EnsureDataLoaded();
string spriteFolder = GetFolder(FilePath) + "Decompiled" + Path.DirectorySeparatorChar + "sprites" + Path.DirectorySeparatorChar;
TextureWorker worker = new TextureWorker();
if (Directory.Exists(spriteFolder))
{
    Directory.Delete(spriteFolder, true);
}

public class YyTemplate
{
    public AssetReference parent { get; set; }
    public string resourceType { get; set; }
    public string resourceVersion { get; set; }
    public YyTemplate(){ resourceType = base.GetType().Name; }
    public List<string> tags { get; set; } = new List<string>();
    public string name { get; set; }
}
public class AssetReference
{
	public string name { get; set; }
	public string path { get; set; }
}

public class GMEvent : YyTemplate
{
    public int eventType { get; set; }
    public int eventNum { get; set; }
}

public class GMBaseTrack : YyTemplate
{
	public uint trackColour { get; set; }
	public bool inheritsTrackColour { get; set; } = true;
	public int builtinName { get; set; }
	public int traits { get; set; }
	public int interpolation { get; set; } = 1; // None, Linear
	public List<GMBaseTrack> tracks { get; set; } = new List<GMBaseTrack>();
	public List<GMEvent> events { get; set; } = new List<GMEvent>();
	public bool isCreationTrack { get; set; }
	public List<string> modifiers { get; set; } = new List<string>();
}

public class Keyframe<T> : YyTemplate
{
	public Keyframe()
	{
		name = null;
		tags = null;
	}

	public Guid id { get; set; } = Guid.NewGuid();
    public float Key = 1.0f;
    public float Length = 1.0f;
    public string resourceType = "Keyframe" + "<" + typeof(T).Name + ">";
    public bool Stretch = false;
    public bool Disabled = false;
    public bool IsCreationKey = false;
	public Dictionary<string, T> Channels { get; set; } = new Dictionary<string, T>();
}
public class KeyframeStore<T> : YyTemplate
{
	public KeyframeStore()
	{
        tags = null;
        name = null;
	}

	public List<Keyframe<T>> Keyframes { get; set; } = new List<Keyframe<T>>();

    public string resourceType = "KeyframeStore" + "<" + typeof(T).Name + ">";
}
public class SpriteData : YyTemplate
{
    public int bbox_bottom { get; set; }
    public int bbox_left { get; set; }
    public int bbox_right { get; set; }
    public int bbox_top { get; set; }
    public uint bboxMode { get; set; }
    public int collisionKind = 1; //(1)
    public int collisionTolerance = 0; //(0)
    public bool DynamicTexturePage = false; //(false)
    public bool edgeFiltering = false; //(false)
    public bool For3D = false; //(false)
    public List<FrameData> frames { get; set; }
    public int gridX = 0; //0 until further notice
    public int gridY = 0; //0 until further notice
    public uint height { get; set; }
    public bool HTile = false; //(false)
    public List<LayerData> layers { get; set; }

    public GMNineSliceData nineSlice = new GMNineSliceData();
    public int origin = 0; //no idea what this is (set to 0)

    public AssetReference parent = new AssetReference() { name = "Sprites", path = "folders/Sprites.yy" };
    public bool preMultiplyAlpha = false; //??? (false)


    public float swfPrecision = 2.525f;
    public int type = 0;
    public bool VTile = false;
    public uint width { get; set; }
    public SequenceData sequence { get; set;}

    public AssetReference textureGroupId { get; set;}
}
public class AssetKeyframe : YyTemplate
{
	public AssetReference Id { get; set; }
}
public class SpriteFrameKeyframe : AssetKeyframe
{
    public AssetReference Id { get; set; }
    public string resourceType = "SpriteFrameKeyframe";
    public string resourceVersion = "1.0";
}
public class GMSpriteFramesTrack : GMBaseTrack
{
	public string spriteId { get; set; }
	public KeyframeStore<SpriteFrameKeyframe> keyframes { get; set; }
	public string name { get { return "frames"; } }
}
public class KeyframeData : YyTemplate
{

}
public class FrameData : YyTemplate
{
    public string resourceType = "GMSpriteFrame";
    public string resourceVersion = "1.1";
}
public class LayerData : YyTemplate
{
    public string resourceType = "GMImageLayer";
    public string resourceVersion = "1.0";
    public int blendMode = 0;
    public string displayName = "default";
    public bool isLocked = false;
    public float opacity = 100.0f;
    public bool visible = true;
}
public class SequenceData : YyTemplate
{
    public string resourceType = "GMSequence";
    public string resourceVersion = "1.4";
    public bool autoRecord = true;
    public int backdropHeight = 768;
    public float backdropImageOpacity = 0.5f;
    public bool backdropImagePath = true;
    public int backdropWidth = 1366;
    public float backdropXOffset = 0.0f;
    public float backdropYOffset = 0.0f;
    public bool showBackdrop = true;
    public bool showBackdropImage = true;
    public int timeUnits = 1;
    public int playback = 1;
    public float playbackSpeed = 1;
    public AnimSpeedType playbackSpeedType = AnimSpeedType.FramesPerGameFrame;
    public float volume = 1.0f;
    public float length { get; set; }
    public int xorigin { get; set; }
    public int yorigin { get; set; }
    

    public List<GMSpriteFramesTrack> tracks { get; set; }
}

public class GMNineSliceData : YyTemplate
{
    public string resourceType = "GMNineSliceData";
    public string resourceVersion = "1.0";
    public bool enabled = false;

    public int left = 0;
    public int right = 0;
    public int top = 0;
    public int bottom = 0;
    public uint highlightStyle = 0;
    public uint highlightColour = 1728023040;

    public uint[] guideColour = new uint[4]
    {
        4294902015, 4294902015, 4294902015, 4294902015
    };
    public UndertaleSprite.NineSlice.TileMode[] tileMode = new UndertaleSprite.NineSlice.TileMode[5];
}

Directory.CreateDirectory(spriteFolder);

SetProgressBar(null, "Sprites", 0, Data.Sprites.Count);
StartProgressBarUpdater();

// default just in case
var defaultTexGroup = new AssetReference
{
    name = "Default",
    path = "texturegroups/Default"
};
var texGroups = new Dictionary<UndertaleSprite, AssetReference>();
foreach (UndertaleTextureGroupInfo group in Data.TextureGroupInfo)
{
    var reference = new AssetReference
    {
        name = group.Name.Content,
        path = "texturegroups/" + group.Name.Content
    };
    foreach (UndertaleResourceById<UndertaleSprite, UndertaleChunkSPRT> spr in group.Sprites)
    {
        texGroups.TryAdd(spr.Resource, reference);
    }
    foreach (UndertaleResourceById<UndertaleSprite, UndertaleChunkSPRT> spr in group.SpineSprites)
    {
        texGroups.TryAdd(spr.Resource, reference);
    }
}

await DumpSprites();
worker.Cleanup();

await StopProgressBarUpdater();
HideProgressBar();
ScriptMessage("Export Complete.\n\nLocation: " + spriteFolder);


string GetFolder(string path)
{
    return Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;
}

async Task DumpSprites()
{
    await Task.Run(() => Parallel.ForEach(Data.Sprites, DumpSprite));
}

void DumpSprite(UndertaleSprite sprite)
{
    List<FrameData> framesSpr = new List<FrameData>();
    List<LayerData> layersSpr = new List<LayerData>();
    List<GMSpriteFramesTrack> tracksSpr = new List<GMSpriteFramesTrack>();
    GMSpriteFramesTrack track = new GMSpriteFramesTrack()
    {
        spriteId = null, //idk
        keyframes = new KeyframeStore<SpriteFrameKeyframe>()

    };
    string spritePath = spriteFolder + sprite.Name.Content + Path.DirectorySeparatorChar;
    Directory.CreateDirectory(spritePath);

    string layersFolder = spritePath + "layers" + Path.DirectorySeparatorChar;
    Directory.CreateDirectory(layersFolder);

    string guid2 = Guid.NewGuid().ToString();
    layersSpr.Add(new LayerData() { name = guid2 });
    for (int i = 0; i < sprite.Textures.Count; i++)
    {
        if (sprite.Textures[i]?.Texture != null)
        {
            string guid1 = Guid.NewGuid().ToString();
            framesSpr.Add(new FrameData() { name = guid1 });
            string folderName = guid1;
            string folderPath = layersFolder + folderName + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(folderPath);

            track.keyframes.Keyframes.Add(new Keyframe<SpriteFrameKeyframe>()
            {
                Key = i,
                Channels = new Dictionary<string, SpriteFrameKeyframe>()
                {
                    {
                        "0", new SpriteFrameKeyframe()
                        {
                            Id = new AssetReference
                            {
                                name = guid1,
                                path = $"sprites/{sprite.Name.Content}/{sprite.Name.Content}.yy"
                            }
                        }
                    }
                }

            });

            worker.ExportAsPNG(sprite.Textures[i].Texture, spritePath + guid1 + ".png", null, true);
            worker.ExportAsPNG(sprite.Textures[i].Texture, folderPath + guid2 + ".png", null, true);


        }
    }

    tracksSpr.Add(track);
    SequenceData sequenceSpr = new SequenceData()
    {
        resourceType = "GMSequence",
        resourceVersion = "1.4",
        name = sprite.Name.Content,
        autoRecord = true,
        backdropHeight = 768,
        backdropImageOpacity = 0.5f,
        backdropImagePath = true,
        backdropWidth = 1366,
        backdropXOffset = 0.0f,
        backdropYOffset = 0.0f,
        length = sprite.Textures.Count,
        xorigin = sprite.OriginX,
        yorigin = sprite.OriginY,
        tracks = tracksSpr,
        playbackSpeedType = sprite.GMS2PlaybackSpeedType,
        playbackSpeed = sprite.GMS2PlaybackSpeed
    };
    SpriteData sprdata = new SpriteData()
    {
        resourceType = "GMSprite",
        resourceVersion = "1.0",
        name = sprite.Name.Content,
        bbox_bottom = sprite.MarginBottom,
        bbox_left = sprite.MarginLeft,
        bbox_right = sprite.MarginRight,
        bbox_top = sprite.MarginTop,
        bboxMode = 0,
        collisionKind = sprite.SepMasks switch {
            UndertaleSprite.SepMaskType.AxisAlignedRect => 1,
            UndertaleSprite.SepMaskType.Precise => 4,
            UndertaleSprite.SepMaskType.RotatedRect => 5,
        },
        frames = framesSpr,
        height = sprite.Height,
        layers = layersSpr,
        width = sprite.Width,
        sequence = sequenceSpr,
        textureGroupId = texGroups.GetValueOrDefault(sprite, defaultTexGroup),
        nineSlice = sprite.V3NineSlice == null ? new GMNineSliceData{} : new GMNineSliceData
        {
            enabled = sprite.V3NineSlice.Enabled,
            left = sprite.V3NineSlice.Left,
            right = sprite.V3NineSlice.Right,
            top = sprite.V3NineSlice.Top,
            bottom = sprite.V3NineSlice.Bottom,
            tileMode = sprite.V3NineSlice.TileModes,
        },
    };

    string json = JsonConvert.SerializeObject(sprdata, Formatting.Indented);
    string spriteFileName = sprite.Name.Content + ".yy";
    File.WriteAllText(spritePath + spriteFileName, json);
    IncrementProgressParallel();
}